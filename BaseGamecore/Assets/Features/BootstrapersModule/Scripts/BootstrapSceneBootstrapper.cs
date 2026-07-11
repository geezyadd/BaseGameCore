using Cysharp.Threading.Tasks;
using Features.AddressablesConstantsGenerator.Generated;
using Features.SceneLoaderModule.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Features.BootstrapersModule {
    public sealed class BootstrapSceneBootstrapper : MonoBehaviour {
        private ISceneLoaderService _sceneLoaderService;
        private bool _started;

        [Inject]
        private void Construct(ISceneLoaderService sceneLoaderService) {
            _sceneLoaderService = sceneLoaderService;
        }

        private void Start() {
            BootstrapAsync().Forget(Debug.LogException);
        }

        private async UniTask BootstrapAsync() {
            if (_started)
                return;

            _started = true;
            Scene bootstrapScene = gameObject.scene;

            await _sceneLoaderService.LoadSceneAsync(Address.Scenes.GlobalScene, false);

            if (bootstrapScene.IsValid() && bootstrapScene.isLoaded)
                await SceneManager.UnloadSceneAsync(bootstrapScene);
        }
    }
}
