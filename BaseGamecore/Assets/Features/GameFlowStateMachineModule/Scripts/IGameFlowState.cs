using System;
using Cysharp.Threading.Tasks;

namespace Features.GameFlowStateMachineModule {
    public interface IGameFlowState {
        event Action Activated;
        event Action Deactivated;

        UniTask EnterAsync();
        UniTask ExitAsync();
    }
}
