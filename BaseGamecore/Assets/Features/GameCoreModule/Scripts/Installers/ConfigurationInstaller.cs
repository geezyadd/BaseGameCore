using Features.AddressablesConstantsGenerator.Generated;
using Features.SceneLoaderModule.Scripts;
using Features.Zenject.Zenject.Addons.AddressablesConfigurationsLoader;
using Zenject;

namespace GameCore.Installers {
    public class ConfigurationInstaller : Installer<ConfigurationInstaller> {
        public override void InstallBindings() {
            Container.BindConfigurationFromAddressables<SceneLoaderConfig>(Address.Configurations.SceneLoaderConfig_Default)
                .AsSingle();
        }
    }
}
