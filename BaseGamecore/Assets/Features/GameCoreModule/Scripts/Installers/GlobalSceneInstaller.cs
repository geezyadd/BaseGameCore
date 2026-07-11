using Features.GameFlowStateMachineModule;
using Features.GameFlowStateMachineModule.Installers;
using Zenject;

namespace GameCore.Installers {
    public class GlobalSceneInstaller : MonoInstaller<GlobalSceneInstaller> {
        public override void InstallBindings() {
            DataInstaller.Install(Container);
            GameFlowStateMachineModuleInstaller.Install(Container);
            Container.BindInterfacesTo<GameFlowSceneTransitionSystem>().AsSingle().NonLazy();
        }
    }
}
