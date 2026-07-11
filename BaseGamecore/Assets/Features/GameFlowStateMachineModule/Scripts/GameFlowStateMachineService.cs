using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Features.GameFlowStateMachineModule.States;

namespace Features.GameFlowStateMachineModule {
    public sealed class GameFlowStateMachineService : IGameFlowStateMachineService {
        private readonly IReadOnlyDictionary<Type, IGameFlowState> _states;
        private readonly GameFlowStateLifecycleEventClass _lifecycleEvents;
        private IGameFlowState _currentState;

        public GameFlowStateMachineService(List<IGameFlowState> states, GameFlowStateLifecycleEventClass lifecycleEvents) {
            _states = states.ToDictionary(state => state.GetType());
            _lifecycleEvents = lifecycleEvents;
        }

        public Type CurrentStateType => _currentState?.GetType();
        public bool IsTransitioning { get; private set; }

        public async UniTask EnterAsync<TState>() where TState : class, IGameFlowState {
            Type nextStateType = typeof(TState);

            if (IsTransitioning)
                throw new InvalidOperationException("A GameFlow state transition is already in progress.");

            if (CurrentStateType == nextStateType)
                throw new InvalidOperationException($"GameFlow is already in {nextStateType.Name}.");

            if (IsTransitionAllowed(CurrentStateType, nextStateType) is false)
                throw new InvalidOperationException(
                    $"GameFlow transition from {CurrentStateType?.Name ?? "None"} to {nextStateType.Name} is not allowed.");

            if (_states.TryGetValue(nextStateType, out IGameFlowState nextState) is false)
                throw new InvalidOperationException($"GameFlow state {nextStateType.Name} is not registered.");

            IsTransitioning = true;
            Type previousStateType = CurrentStateType;

            try {
                if (_currentState != null) {
                    await _currentState.ExitAsync();
                    _lifecycleEvents.InvokeOnStateExited(previousStateType);
                }

                await nextState.EnterAsync();
                _currentState = nextState;
                _lifecycleEvents.InvokeOnStateEntered(nextStateType);
            }
            finally {
                IsTransitioning = false;
            }
        }

        private static bool IsTransitionAllowed(Type currentStateType, Type nextStateType) {
            if (currentStateType == null)
                return nextStateType == typeof(GlobalGameFlowState);

            if (currentStateType == typeof(GlobalGameFlowState))
                return nextStateType == typeof(MenuGameFlowState);

            if (currentStateType == typeof(MenuGameFlowState))
                return nextStateType == typeof(SessionGameFlowState);

            if (currentStateType == typeof(SessionGameFlowState))
                return nextStateType == typeof(MenuGameFlowState);

            return false;
        }
    }
}
