using System.Collections.Generic;
using UnityEngine;

namespace Features.SceneLoaderModule.Scripts {
    [CreateAssetMenu(fileName = nameof(SceneLoaderConfig) + "_Default",
        menuName = "Configurations/SceneLoader/" + nameof(SceneLoaderConfig))]
    public class SceneLoaderConfig : ScriptableObject {
        [field: SerializeField] public List<string> SceneAddressablesInGroup { get; set; }
    }
}