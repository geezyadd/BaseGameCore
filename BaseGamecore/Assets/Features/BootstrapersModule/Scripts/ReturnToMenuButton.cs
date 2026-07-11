using Cysharp.Threading.Tasks;
using Features.GameFlowStateMachineModule;
using Features.GameFlowStateMachineModule.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.BootstrapersModule {
    [RequireComponent(typeof(Button))]
    public sealed class ReturnToMenuButton : MonoBehaviour {
        private IGameFlowStateMachineService _gameFlowStateMachine;
        private Button _button;
        private bool _isTransitioning;

        [Inject]
        private void Construct(IGameFlowStateMachineService gameFlowStateMachine) {
            _gameFlowStateMachine = gameFlowStateMachine;
        }

        private void Awake() {
            _button = GetComponent<Button>();
        }

        private void OnEnable() {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable() {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick() {
            ReturnToMenuAsync().Forget(Debug.LogException);
        }

        private async UniTask ReturnToMenuAsync() {
            if (_isTransitioning || _gameFlowStateMachine.IsTransitioning)
                return;

            _isTransitioning = true;
            _button.interactable = false;

            try {
                await _gameFlowStateMachine.EnterAsync<MenuGameFlowState>();
            }
            finally {
                _isTransitioning = false;

                if (_button != null)
                    _button.interactable = true;
            }
        }
    }
}
