using System;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Features.Zenject.Zenject.Addons.AsyncInstaller {
    public abstract class AsyncInstaller<TDerived> : InstallerBase where TDerived : AsyncInstaller<TDerived> {
        public static async UniTask Install(DiContainer container) =>
            await container.Instantiate<TDerived>().InstallBindingsAsync();

        public override void InstallBindings() =>
            throw new Exception("AsyncInstaller should use InstallBindingsAsync() method");

        public abstract UniTask InstallBindingsAsync();
    }
}