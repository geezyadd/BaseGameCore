using System;
using Cysharp.Threading.Tasks;
using Features.SceneLoaderModule.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Features.GameFlowStateMachineModule {
    public sealed class GameFlowSceneTransitionSystem : IInitializable, IDisposable {
        private readonly GameFlowStateLifecycleEventClass _lifecycleEvents;
        private readonly ISceneLoaderService _sceneLoaderService;
        private Type _previousStateType;

        public GameFlowSceneTransitionSystem(
            GameFlowStateLifecycleEventClass lifecycleEvents,
            ISceneLoaderService sceneLoaderService) {
            _lifecycleEvents = lifecycleEvents;
            _sceneLoaderService = sceneLoaderService;
        }

        public void Initialize() {
            _lifecycleEvents.OnStateEntered += OnStateEntered;
        }

        public void Dispose() {
            _lifecycleEvents.OnStateEntered -= OnStateEntered;
        }

        private void OnStateEntered(Type stateType) {
            Type previousStateType = _previousStateType;
            _previousStateType = stateType;
            ApplyStateAsync(stateType, previousStateType).Forget(Debug.LogException);
        }

        private async UniTask ApplyStateAsync(Type currentStateType, Type previousStateType) {
            if (GameFlowStateSceneMapper.TryGetScene(previousStateType, out string sceneToUnload))
                await _sceneLoaderService.UnloadSceneAsync(sceneToUnload);

            if (GameFlowStateSceneMapper.TryGetScene(currentStateType, out string sceneToLoad) is false)
                return;

            await LoadAndActivateAsync(sceneToLoad);
        }

        private async UniTask LoadAndActivateAsync(string sceneName) {
            await _sceneLoaderService.LoadSceneAsync(sceneName, false);

            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.IsValid() && scene.isLoaded)
                SceneManager.SetActiveScene(scene);
        }
    }
}
