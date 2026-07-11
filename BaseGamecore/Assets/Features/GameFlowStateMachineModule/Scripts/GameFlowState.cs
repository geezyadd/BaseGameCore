using System;
using Cysharp.Threading.Tasks;

namespace Features.GameFlowStateMachineModule {
    public abstract class GameFlowState : IGameFlowState {
        public event Action Activated;
        public event Action Deactivated;

        public async UniTask EnterAsync() {
            await OnEnterAsync();
            Activated?.Invoke();
        }

        public async UniTask ExitAsync() {
            Deactivated?.Invoke();
            await OnExitAsync();
        }

        protected virtual UniTask OnEnterAsync() => UniTask.CompletedTask;
        protected virtual UniTask OnExitAsync() => UniTask.CompletedTask;
    }
}
