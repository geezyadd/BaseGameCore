# GameCoreModule

Ядро игровой архитектуры: bootstrap, глобальный DI-контекст и сцены меню/сессии.

## Структура

```
GameCoreModule/
├── GameResources/
│   ├── Resources/
│   │   └── ProjectContext.prefab   # app-wide DI root + ProjectContextInstaller
│   └── Scenes/
│       ├── BootstrapScene.unity    # entry, SceneContext без installer
│       ├── GlobalScene.unity       # contract: Global + GlobalSceneInstaller
│       ├── MenuScene.unity         # parent: Global + MenuSceneInstaller
│       └── SessionScene.unity      # parent: Global + SessionSceneInstaller
└── Scripts/
    ├── GameCoreModule.asmdef
    └── Installers/
        ├── ProjectContextInstaller.cs
        ├── GlobalSceneInstaller.cs
        ├── MenuSceneInstaller.cs
        └── SessionSceneInstaller.cs
```

## Иерархия Zenject

```
ProjectContext (+ ProjectContextInstaller)
  └── GlobalScene (contract: Global)
        ├── MenuScene (parent: Global)
        └── SessionScene (parent: Global)

BootstrapScene — отдельный SceneContext без parent/contract и без installer.
```

## Правила

- App-wide / cross-scene инфраструктуру биндить в `ProjectContextInstaller`.
- Shared gameplay/session-independent сервисы сцены — в `GlobalSceneInstaller`.
- Локальные биндинги меню/сессии — только в соответствующих scene installers.
- `BootstrapScene` не получает installer: она только стартовая точка.
- `MenuScene` / `SessionScene` загружаются additive при уже загруженной `GlobalScene`.
- Не дублировать биндинги между Project / Global / child-сценами.
- `ProjectContext.prefab` должен лежать в папке `Resources` (сейчас: `GameResources/Resources`).

## Следующие шаги

- Bootstrap flow: загрузка Global + Menu.
- Переключение Menu ↔ Session без выгрузки Global.
- Наполнение installers реальными модулями (SceneLoader, AssetLoader и т.д.).
