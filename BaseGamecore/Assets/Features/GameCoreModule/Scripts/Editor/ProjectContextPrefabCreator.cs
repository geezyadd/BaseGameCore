#if UNITY_EDITOR
using GameCore.Installers;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace GameCore.Editor {
    public static class ProjectContextPrefabCreator {
        private const string ResourcesFolder = "Assets/Features/GameCoreModule/GameResources/Resources";
        private const string PrefabPath = ResourcesFolder + "/ProjectContext.prefab";

        [MenuItem("GameCore/Create ProjectContext Prefab")]
        public static void Create() {
            if (!AssetDatabase.IsValidFolder("Assets/Features/GameCoreModule/GameResources")) {
                AssetDatabase.CreateFolder("Assets/Features/GameCoreModule", "GameResources");
            }

            if (!AssetDatabase.IsValidFolder(ResourcesFolder)) {
                AssetDatabase.CreateFolder("Assets/Features/GameCoreModule/GameResources", "Resources");
            }

            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            if (existing != null) {
                Debug.Log($"ProjectContext prefab already exists at '{PrefabPath}'");
                Selection.activeObject = existing;
                return;
            }

            var gameObject = new GameObject("ProjectContext");
            try {
                var projectContext = gameObject.AddComponent<ProjectContext>();
                var installer = gameObject.AddComponent<ProjectContextInstaller>();
                projectContext.Installers = new[] { installer };

                var prefab = PrefabUtility.SaveAsPrefabAsset(gameObject, PrefabPath);
                Selection.activeObject = prefab;
                Debug.Log($"Created ProjectContext prefab at '{PrefabPath}'");
            }
            finally {
                Object.DestroyImmediate(gameObject);
            }
        }
    }
}
#endif
