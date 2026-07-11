using UnityEngine;
using Zenject;

namespace Features.AssetLoaderModule.Scripts {
    public class ExampleAssetLoaderClient : MonoBehaviour {
        private IAssetLoaderFacadeService _assetLoaderFacadeService;
        private IResourceAssetLoaderService _resourceAssetLoaderService;
        private IAssetLoaderFacadeService _addressablesAssetLoaderFacadeService;

        [Inject]
        private void InjectDependencies(IAssetLoaderFacadeService assetLoaderFacadeService, IResourceAssetLoaderService resourceAssetLoaderService,
            IAssetLoaderFacadeService addressablesAssetLoaderFacadeService) {
            _addressablesAssetLoaderFacadeService = addressablesAssetLoaderFacadeService;
            _resourceAssetLoaderService = resourceAssetLoaderService;
            _assetLoaderFacadeService = assetLoaderFacadeService;
        }

        private void Start() {
            //Loads asset from resources and puts it in default group
            ExampleAsset exampleAsset = _resourceAssetLoaderService.LoadAsset<ExampleAsset>("ExampleAssetResources");
            //Loads asset from resources and puts it in permanent group
            ExampleAsset permanentExampleAsset = _resourceAssetLoaderService.LoadAsset<ExampleAsset>("ExampleAssetResources", "Permanent");
            
            //Instantiate loaded assets so they can say hello twice
            Instantiate(exampleAsset);
            Instantiate(permanentExampleAsset);
            
            //Take already loaded asset from resources and instantiate him one more time
            exampleAsset = _resourceAssetLoaderService.LoadAsset<ExampleAsset>("ExampleAssetResources");
            Instantiate(exampleAsset);
            
            //Release assets in default group
            _resourceAssetLoaderService.ReleaseAssetsInGroup();
            //Load asset from resources again and put it in default group and instantiate
            exampleAsset = _resourceAssetLoaderService.LoadAsset<ExampleAsset>("ExampleAssetResources");
            Instantiate(exampleAsset);
            
            //Take already loaded asset which was put in permanent group and instantiate it
            permanentExampleAsset = _resourceAssetLoaderService.LoadAsset<ExampleAsset>("ExampleAssetResources", "Permanent");
            Instantiate(permanentExampleAsset);

            //Release assets from memory of all groups (Default and Permanent)
            _resourceAssetLoaderService.ReleaseAllAssets();
        }
    }
}