using System;
using Features.AssetLoaderModule.Scripts;
using UnityEngine;
using Zenject;

namespace Features.AssetLoaderModule.Scripts {
        public class ExampleAssetLoaderModuleBootstrapper : MonoInstaller<ExampleAssetLoaderModuleBootstrapper> {
        [SerializeField] private GameObject _exampleAssetLoaderClient;
        public override void InstallBindings() {
            Container.Bind<ExampleAssetLoaderClient>()
                .FromComponentInNewPrefab(_exampleAssetLoaderClient)
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<IAssetLoaderFacadeService>()
                .AsSingle();
            Container.BindInterfacesTo<ResourceAssetLoaderService>()
                .AsSingle();
            Container.BindInterfacesTo<AssetLoaderFacadeService>()
                .AsSingle();
        }
    }
}