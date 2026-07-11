using Cysharp.Threading.Tasks;
using Features.GameFlowStateMachineModule;
using Features.GameFlowStateMachineModule.States;
using UnityEngine;
using Zenject;

namespace Features.BootstrapersModule {
    public sealed class GlobalSceneBootstrapper : MonoBehaviour {
        private IGameFlowStateMachineService _gameFlowStateMachine;
        private bool _started;

        [Inject]
        private void Construct(IGameFlowStateMachineService gameFlowStateMachine) {
            _gameFlowStateMachine = gameFlowStateMachine;
        }

        private void Start() {
            StartGameFlowAsync().Forget(Debug.LogException);
        }

        private async UniTask StartGameFlowAsync() {
            if (_started)
                return;

            _started = true;
            await _gameFlowStateMachine.EnterAsync<GlobalGameFlowState>();
            await _gameFlowStateMachine.EnterAsync<MenuGameFlowState>();
        }
    }
}
