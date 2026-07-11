using Features.AssetLoaderModule.Scripts.Installers;
using Features.SceneLoaderModule.Scripts;
using Zenject;

namespace GameCore.Installers {
    public class ProjectContextInstaller : MonoInstaller<ProjectContextInstaller> {
        public override void InstallBindings() {
            AssetLoaderServiceModuleInstaller.Install(Container);
            ConfigurationInstaller.Install(Container);
            SceneLoaderServiceModuleInstaller.Install(Container);
        }
    }
}
