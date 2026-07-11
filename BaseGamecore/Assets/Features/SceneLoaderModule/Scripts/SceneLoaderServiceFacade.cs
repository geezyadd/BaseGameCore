using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Features.SceneLoaderModule.Scripts {
    public class SceneLoaderServiceFacade : ISceneLoaderService {
        private readonly BuildInSceneLoaderService _buildInSceneLoaderService;
        private readonly AddressablesSceneLoaderService _addressablesSceneLoaderService;
        private readonly SceneLoaderConfig _sceneLoaderConfiguration;
        
        internal SceneLoaderServiceFacade(
            BuildInSceneLoaderService buildInSceneLoaderService,
            AddressablesSceneLoaderService addressablesSceneLoaderService,
            SceneLoaderConfig sceneLoaderConfiguration) {
            _addressablesSceneLoaderService = addressablesSceneLoaderService;
            _buildInSceneLoaderService = buildInSceneLoaderService;
            _sceneLoaderConfiguration = sceneLoaderConfiguration;
        }
    
        public async UniTask LoadSceneAsync(string sceneToLoad, bool unloadRedundant) {
            if (_sceneLoaderConfiguration.SceneAddressablesInGroup.Contains(sceneToLoad))
                await _addressablesSceneLoaderService.LoadSceneAsync(sceneToLoad, unloadRedundant);
            else
                await _buildInSceneLoaderService.LoadSceneAsync(sceneToLoad, unloadRedundant);
        }
        
        public async UniTask LoadScenesAsync(List<string> scenesToLoad, string activeScene, bool unloadRedundant) {
            List<string> scenesNotInAddressables = scenesToLoad.Except(_sceneLoaderConfiguration.SceneAddressablesInGroup).ToList();

            if (scenesNotInAddressables.Any() is false) {
                await _addressablesSceneLoaderService.LoadScenesAsync(scenesToLoad, unloadRedundant);
                SetActiveScene(activeScene);
                return;
            }

            if (scenesNotInAddressables.Count == scenesToLoad.Count) {
                await _buildInSceneLoaderService.LoadScenesAsync(scenesToLoad, unloadRedundant);
                SetActiveScene(activeScene);
                return;
            }

            await _buildInSceneLoaderService.LoadScenesAsync(scenesNotInAddressables, unloadRedundant);
            await _addressablesSceneLoaderService.LoadScenesAsync(scenesToLoad.Except(scenesNotInAddressables).ToList(), unloadRedundant);
            SetActiveScene(activeScene);
        }
        
        private void SetActiveScene(string activeScene) {
            if (SceneManager.GetActiveScene().name == activeScene)
                return;
            
            Scene newActiveScene = SceneManager.GetSceneByName(activeScene);
            SceneManager.SetActiveScene(newActiveScene);
        }

        public async UniTask UnloadSceneAsync(string sceneToUnload) {
            if (_sceneLoaderConfiguration.SceneAddressablesInGroup.Contains(sceneToUnload))
                await _addressablesSceneLoaderService.UnloadSceneAsync(sceneToUnload);
            else
                await _buildInSceneLoaderService.UnloadSceneAsync(sceneToUnload);
        }

        public async UniTask UnloadScenesAsync(List<string> scenesToUnload) {
            List<string> scenesNotInAddressables = scenesToUnload.Except(_sceneLoaderConfiguration.SceneAddressablesInGroup)
                .ToList();

            if (scenesNotInAddressables.Any() is false) {
                await _addressablesSceneLoaderService.UnloadScenesAsync(scenesToUnload);
                return;
            }

            if (scenesNotInAddressables.Count == scenesToUnload.Count) {
                await _buildInSceneLoaderService.UnloadScenesAsync(scenesToUnload);
                return;
            }

            await _buildInSceneLoaderService.UnloadScenesAsync(scenesNotInAddressables);
            await _addressablesSceneLoaderService.UnloadScenesAsync(scenesToUnload.Except(scenesNotInAddressables)
                .ToList());
        }
    }
}