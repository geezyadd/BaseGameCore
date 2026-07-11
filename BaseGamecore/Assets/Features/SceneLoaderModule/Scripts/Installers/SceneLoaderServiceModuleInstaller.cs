using Features.SceneLoaderModule.Scripts;
using Zenject;

namespace Features.SceneLoaderModule.Scripts {
    public class SceneLoaderServiceModuleInstaller : Installer<SceneLoaderServiceModuleInstaller> {
        public override void InstallBindings() {
            Container.Bind<BuildInSceneLoaderService>()
                .AsSingle();
            
            Container.Bind<AddressablesSceneLoaderService>()
                .AsSingle();
            
            Container.BindInterfacesTo<SceneLoaderServiceFacade>()
                .AsSingle();
        }
    }
}
