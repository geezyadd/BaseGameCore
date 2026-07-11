using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Features.SceneLoaderModule.Scripts.Editor {
    [CustomEditor(typeof(SceneLoaderConfig))]
    public class AchievementsTypeConfigurationEditor : UnityEditor.Editor {
        private int _selectedGroupIndex = -1;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            SceneLoaderConfig config = (SceneLoaderConfig)target;

            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null) {
                ShowError("Addressable Asset Settings not found!");
                return;
            }

            List<string> groupNames = GetAddressableGroups(settings);

            if (groupNames.Count > 0) {
                _selectedGroupIndex = DrawGroupSelectionPopup(groupNames);
                HandleFillButton(config, groupNames);
            }
            else {
                ShowWarning("No Addressable Groups found.");
            }
        }

        private List<string> GetAddressableGroups(AddressableAssetSettings settings) {
            return settings.groups.Select(group => group.Name).ToList();
        }

        private int DrawGroupSelectionPopup(List<string> groupNames) {
            return EditorGUILayout.Popup("Select Addressable Group", _selectedGroupIndex, groupNames.ToArray());
        }

        private void HandleFillButton(SceneLoaderConfig config, List<string> groupNames) {
            if (GUILayout.Button("Fill scene list from addressable")) {
                if (IsValidGroupSelected(groupNames)) {
                    FillConfigFromAddressable(config, groupNames[_selectedGroupIndex]);
                }
                else {
                    ShowWarning("No Addressable Group selected.");
                }
            }
        }

        private bool IsValidGroupSelected(List<string> groupNames) {
            return _selectedGroupIndex >= 0 && _selectedGroupIndex < groupNames.Count;
        }

        private void FillConfigFromAddressable(SceneLoaderConfig config, string groupName) {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            AddressableAssetGroup selectedGroup = settings.groups.FirstOrDefault(group => group.Name == groupName);

            if (selectedGroup == null) {
                ShowError("Selected Addressable Group not found.");
                return;
            }

            List<string> sceneAddresses = GetSceneAddressesFromGroup(selectedGroup);
            config.SceneAddressablesInGroup = sceneAddresses;

            EditorUtility.SetDirty(config);
        }

        private List<string> GetSceneAddressesFromGroup(AddressableAssetGroup selectedGroup) {
            return selectedGroup.entries.Where(entry => entry.AssetPath.EndsWith(".unity")).Select(entry => entry.AssetPath).ToList();
        }

        private void ShowError(string message) {
            Debug.LogError(message);
        }

        private void ShowWarning(string message) {
            Debug.LogWarning(message);
        }
    }
}