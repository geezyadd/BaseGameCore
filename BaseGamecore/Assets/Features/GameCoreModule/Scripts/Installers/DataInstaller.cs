using System;
using Features.GameFlowStateMachineModule;
using Zenject;

namespace GameCore.Installers {
    public class DataInstaller : Installer<DataInstaller> {
        public override void InstallBindings() {
            Bind(typeof(GameFlowStateLifecycleEventClass));
        }

        public void Bind(Type type) {
            Container.Bind(type).AsSingle();
        }
    }
}