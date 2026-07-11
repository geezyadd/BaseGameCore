using System;
using System.Collections.Generic;
using Features.AddressablesConstantsGenerator.Generated;
using Features.GameFlowStateMachineModule.States;

namespace Features.GameFlowStateMachineModule {
    internal static class GameFlowStateSceneMapper {
        private static readonly IReadOnlyDictionary<Type, string> StateToScene =
            new Dictionary<Type, string> {
                { typeof(MenuGameFlowState), Address.Scenes.MenuScene },
                { typeof(SessionGameFlowState), Address.Scenes.SessionScene },
            };

        public static bool TryGetScene(Type stateType, out string sceneName) {
            if (stateType == null) {
                sceneName = null;
                return false;
            }

            return StateToScene.TryGetValue(stateType, out sceneName);
        }
    }
}
