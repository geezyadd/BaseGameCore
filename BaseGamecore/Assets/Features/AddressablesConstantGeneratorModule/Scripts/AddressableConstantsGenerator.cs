using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Features.CodeGeneratorModule.CustomCodeGeneratorModule.Scripts.Editor.Core;
using IncrementalSourceGenerator.Utils;
using JetBrains.Annotations;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using ICodeGenerator = Features.CodeGeneratorModule.CustomCodeGeneratorModule.Scripts.Editor.Core.ICodeGenerator;
using Tools = IncrementalSourceGenerator.Utils.Tools;

namespace Features.AddressablesConstantGeneratorModule.Scripts {
    /// <summary>
    ///     Class for generated address from addressables.
    /// </summary>
    [Generator, PublicAPI, HelpURL("https://github.com/AnnulusGames/UnityCodeGen")]
    public class AddressableConstantsGenerator : ICodeGenerator {
        private static bool SkipGroup(string groupName) => 
            string.Equals(groupName, BUILT_IN_DATA_GROUP_NAME, StringComparison.Ordinal);

        private const string BUILT_IN_DATA_GROUP_NAME = "BuiltInData";

        private const string BUILD_FILE_NAME = "Address";
        private const string PUBLIC_STRING_CONST = "public const string";
        private const string PUBLIC_STRING_LIST_STATIC = "public static List<string>";
        private const string PUBLIC_STRING_LIST_LIST_STATIC = "public static List<List<string>>";
        private const string TARGET_NAMESPACE = "Features.AddressablesConstantsGenerator.Generated";

        public void Execute(GeneratorContext context) {
            string filePath = "Assets/Features/AddressablesConstantGeneratorModule/Generated/";
            context.OverrideFolderPath(filePath);
            context.AddCode(Tools.BuildFileName(BUILD_FILE_NAME), Tools.GetSourceText(Source));
        }

        private string Source(StringBuilder builder, StringWriter writer, IndentedTextWriter text) {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            text.AddUsing("System.Collections.Generic");
            text.AddNamespace(TARGET_NAMESPACE);

            text.OpenBlock();
            text.Write("public partial class " + BUILD_FILE_NAME);
            text.OpenBlock();

            string allAddressablesGroups = string.Empty;
            string allAddressablesGroupNames = string.Empty;

            foreach (AddressableAssetGroup addressableAssetGroup in settings.groups) {
                string groupName = addressableAssetGroup.Name.Replace(" ", string.Empty);
                groupName = groupName.Replace("-", string.Empty);
                if (SkipGroup(groupName))
                    continue;
                if(groupName.Contains("DuplicateAssetIsolation"))
                    continue;

                allAddressablesGroupNames += groupName + ".ADDRESSABLE_GROUP_NAME,";
                allAddressablesGroups += groupName + ".AllAddressablesInGroup,";
                text.Write("public static partial class " + groupName);
                text.OpenBlock();
                List<AddressableAssetEntry> addressableAssetEntries = addressableAssetGroup.entries
                    .Where(addressableAssetEntry => !addressableAssetEntry.IsSubAsset).ToList();
                
                string allAddressablesInGroup = string.Empty;
                
                text.AddAssignStatement(@$"{PUBLIC_STRING_CONST} {"ADDRESSABLE_GROUP_NAME"}",  @$"""{groupName}""");

                foreach (AddressableAssetEntry addressableAssetEntry in addressableAssetEntries) {
                    string replace = addressableAssetEntry.address.Replace(" ", string.Empty)
                        .Replace("-", string.Empty)
                        .Replace(@"(", "_")
                        .Replace(@")", string.Empty);

                    if (replace.Contains("/") || replace.Contains(@"\"))
                        replace = replace.Substring(replace.LastIndexOf("/", StringComparison.Ordinal) + 1);

                    if (replace.Contains(@"\"))
                        replace = replace.Substring(replace.LastIndexOf(@"\", StringComparison.Ordinal) + 1);

                    if (replace.Contains("."))
                        replace = replace.Substring(0, replace.LastIndexOf(@".", StringComparison.Ordinal));

                    text.AddAssignStatement(@$"{PUBLIC_STRING_CONST} {replace}", @$"""{addressableAssetEntry.address}""");
                    allAddressablesInGroup += @"""" + addressableAssetEntry.address + @""",";
                }
                
                text.AddAssignStatement(@$"{PUBLIC_STRING_LIST_STATIC} {"AllAddressablesInGroup"}", "new List<string>() {" + allAddressablesInGroup + "}");

                text.CloseBlock();
            }
            
            text.Write("public static partial class " + "Groups");
            text.OpenBlock();
            text.AddAssignStatement(@$"{PUBLIC_STRING_LIST_LIST_STATIC} {"AllAddressablesGroups"}", "new List<List<string>>() {" + allAddressablesGroups + "}");
            text.AddAssignStatement(@$"{PUBLIC_STRING_LIST_STATIC} {"AllAddressablesGroupNames"}", "new List<string>() {" + allAddressablesGroupNames + "}");
            text.CloseBlock();

            text.CloseBlock();
            text.CloseBlock();

            return writer.ToString();
        }
    }
}