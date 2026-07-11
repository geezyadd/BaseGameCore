using System;
using System.Threading.Tasks;
using Features.GameFlowStateMachineModule.Installers;
using Features.GameFlowStateMachineModule.States;
using NUnit.Framework;
using Zenject;

namespace Features.GameFlowStateMachineModule.Tests {
    public sealed class GameFlowStateMachineServiceTests : ZenjectUnitTestFixture {
        private IGameFlowStateMachineService _stateMachine;
        private GameFlowStateLifecycleEventClass _lifecycleEvents;

        [SetUp]
        public void SetUp() {
            Container.Bind<GameFlowStateLifecycleEventClass>().AsSingle();
            GameFlowStateMachineModuleInstaller.Install(Container);
            Container.ResolveRoots();

            _stateMachine = Container.Resolve<IGameFlowStateMachineService>();
            _lifecycleEvents = Container.Resolve<GameFlowStateLifecycleEventClass>();
        }

        [Test]
        public async Task StartsWithGlobalState() {
            await _stateMachine.EnterAsync<GlobalGameFlowState>();

            Assert.That(_stateMachine.CurrentStateType, Is.EqualTo(typeof(GlobalGameFlowState)));
        }

        [Test]
        public async Task SupportsGlobalMenuSessionMenuFlow() {
            await _stateMachine.EnterAsync<GlobalGameFlowState>();
            await _stateMachine.EnterAsync<MenuGameFlowState>();
            await _stateMachine.EnterAsync<SessionGameFlowState>();
            await _stateMachine.EnterAsync<MenuGameFlowState>();

            Assert.That(_stateMachine.CurrentStateType, Is.EqualTo(typeof(MenuGameFlowState)));
        }

        [Test]
        public void RejectsInvalidInitialState() {
            Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _stateMachine.EnterAsync<MenuGameFlowState>());
        }

        [Test]
        public async Task RejectsDuplicateState() {
            await _stateMachine.EnterAsync<GlobalGameFlowState>();

            Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _stateMachine.EnterAsync<GlobalGameFlowState>());
        }

        [Test]
        public async Task PublishesLifecycleEventsAfterSuccessfulTransition() {
            Type enteredState = null;
            Type exitedState = null;
            _lifecycleEvents.OnStateEntered += stateType => enteredState = stateType;
            _lifecycleEvents.OnStateExited += stateType => exitedState = stateType;

            await _stateMachine.EnterAsync<GlobalGameFlowState>();
            await _stateMachine.EnterAsync<MenuGameFlowState>();

            Assert.That(exitedState, Is.EqualTo(typeof(GlobalGameFlowState)));
            Assert.That(enteredState, Is.EqualTo(typeof(MenuGameFlowState)));
        }
    }
}
