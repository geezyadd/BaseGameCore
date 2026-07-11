using System;
using Cysharp.Threading.Tasks;

namespace Features.GameFlowStateMachineModule {
    public interface IGameFlowStateMachineService {
        Type CurrentStateType { get; }
        bool IsTransitioning { get; }

        UniTask EnterAsync<TState>() where TState : class, IGameFlowState;
    }
}
