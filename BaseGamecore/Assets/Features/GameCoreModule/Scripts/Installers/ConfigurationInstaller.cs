using Features.AddressablesConstantsGenerator.Generated;
using Features.MuffinZenject.Zenject.Addons.AddressablesConfigurationsLoader;
using Features.SceneLoaderModule.Scripts;
using Zenject;

namespace GameCore.Installers {
    public class ConfigurationInstaller : Installer<ConfigurationInstaller> {
        public override void InstallBindings() {
            Container.BindConfigurationFromAddressables<SceneLoaderConfig>(Address.Configurations.SceneLoaderConfig_Default)
                .AsSingle();
        }
    }
}
