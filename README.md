# KTool

KTool is a tool library that supports Unity developers in simplifying their work.

## How do I get set up?

 1. Open Package Manager window
 2. Click button [+]
 3. Select [Add package from git URL...]
 4. Paste url: <https://github.com/KemLib/KTool.git?path=/Packages/KTool>
 5. Click button [Add]

If you want to use the latest version, replace the URL: <https://github.com/KemLib/KTool.git?path=/Packages/KTool#develop>

## Features

- **Advertisement**: easily implement advertising display features in the application, see more [KTool.Admob].
- **Attribute**: the attribute allow you to easily select the value of the variable in the Inspector.
- **Script Creater**: editor to create scripts from available templates, automatically adding Namespaces by folder.
- **FileIo**: file read/write library in asset or data.
- **Cron**: wait for conditions then run callbacks (`CronObject`).
- **Init**: object initialization system.
- **Observer**: string-key pub/sub event bus (`ObserverManager`).
- **Worker**: string-key async work fan-out with progress and timeout (`WorkManager`).

### Attribute

|Attribute|Variable Type|Description|
|--|--|--|
|GetAsset|string[], Object[]|get all assets in the folder|
|GetComponent|Component, Component[]|get all component in the current GameObject or in children of current GameObject|
|SelectAsset|string, string[], Object, Object[]|dropdown list to select assets in the folder|
|SelectComponent|Component, Component[]|dropdown list to select component in the current GameObject or in children of current GameObject|
|SelectAgentId|int, int[]|dropdown list to select AgentId|
|SelectLayer|int, int[], string, string[]|dropdown list to select Layer|
|SelectSortingLayer|int, int[], string, string[]|dropdown list to select SortingLayer|
|SelectScene|int, int[], string, string[]|dropdown list to select Scene|
|SelectTag|int, int[], string, string[]|dropdown list to select Tag|

### Script Creater

 1. Right-click on the Project window.
 2. Select KTool -> Create Script
 3. Enter class name
 4. Click [Save]

You can create a new template script, see the SettingCreateScript file in the "Assets/KTool/AssetCreater/Editor/Script" folder.

### Cron

`KTool.Cron.CronManager` (internal) is a DontDestroyOnLoad singleton driven by `Update`. Jobs are built with `CronObject`: add one or more `Condition`s (all must complete), then `Callback`s, then `Run()`.

```csharp
using KTool.Cron;
using UnityEngine;
using UnityEngine.SceneManagement;

CronObject.Create()
    .Add(ConditionReadTime.Create(2f))
    .Add(ConditionDelegate.Create(() => SomeSystem.IsReady))
    .Add(CallbackAction.Create(() => Debug.Log("Ready")))
    .Run();

// Callback with captured state (0–4 typed parameters)
CronObject.Create()
    .Add(ConditionAsyncOperation.Create(SceneManager.LoadSceneAsync("Level1")))
    .Add(CallbackAction.Create((string scene) => Debug.Log($"Loaded {scene}"), "Level1"))
    .Run();
```

Notes:

- Conditions are ANDed: every added condition must complete before any callback runs.
- `Run()` may finish synchronously (conditions already met on the first `Update()` inside `Run()`).
- Do not reuse the same `Condition` or `Callback` instance across jobs or after `IsComplete` is true — `Add` ignores null, already-complete, and duplicate instances.
- `Cancel()` marks the job complete without running callbacks.
- `ConditionTime` uses scaled time (`Time.deltaTime`); `ConditionReadTime` uses `Time.realtimeSinceStartup`.
- `ConditionFunc` / `ConditionDelegate`: exceptions are logged; the condition stays pending. A `null` predicate completes immediately.
- `ConditionTask` completes when `Task.IsCompleted` is true (includes RanToCompletion, faulted, and canceled).
- `ConditionAsyncOperation.Create(op)` waits on `isDone`; `Create(op, progress)` completes when `progress` reaches the target or when `isDone` is true.
- Callback / predicate exceptions are logged; other callbacks on the same job still run.
- Prefer a new `CronObject` and fresh conditions per job; first `Run()` creates `KTool_CronManager`. Extra manager components are destroyed in `Awake`.

| API | Role |
|--|--|
| `CronObject.Create` / `Add` / `Run` / `Cancel` | build and schedule a job |
| `ConditionTime` / `ConditionReadTime` / `ConditionFrame` | scaled time / real time / frame delays |
| `ConditionFunc` / `ConditionDelegate.Create` | custom predicates (`Delegate` supports 0–4 state args) |
| `ConditionAsyncOperation` / `ConditionResourceRequest` / `ConditionTask` | Unity async / ResourceRequest / Task |
| `CallbackAction.Create` | run code when all conditions pass (0–4 state args) |

### Init

Suppose you need to create an application that displays ads, first you need to initialize the ads, then download the ads, and wait for the ads to be ready to display.

 1. Create InitContainer
 2. In the InitContainer, pull the ad initialization object into step 1, pull the ad loading object into step 2.
 3. If you want to load the scene after completing the work, check AfterInit.
 4. Create InitManager
 5. Play

### Observer

`KTool.Observer.ObserverManager` is a DontDestroyOnLoad singleton event bus keyed by `string`.

Supports `Action` with 0–4 typed parameters. Signature must match between `Register` / `Unregister` / `Notify` / `CheckRegister`.

```csharp
using System;
using KTool.Observer;

// Keep a reference — Unregister must use the same delegate instance
Action<int> handler = (int coin) =>
{
    Debug.Log($"Coin: {coin}");
};

ObserverManager.Instance.Register("OnCoinChanged", handler);

// Notify all listeners of that key + signature
ObserverManager.Instance.Notify("OnCoinChanged", 100);

ObserverManager.Instance.Unregister("OnCoinChanged", handler);
```

Notes:

- Duplicate registration of the same delegate is ignored.
- `Register` / `Unregister` / `Notify` / `CheckRegister` signatures must match exactly (e.g. `Action<int>` is not notified by parameterless `Notify`).
- Inline lambdas cannot be unregistered later unless you stored them in a field/variable first.
- `Notify` snapshots the listener list first, so `Register` / `Unregister` inside a callback is safe (new listeners miss this notify; just-removed ones may still run once).
- Listener exceptions are logged and do not stop other listeners.
- Prefer unregister in `OnDisable` / `OnDestroy` to avoid dangling callbacks.
- Singleton: use `ObserverManager.Instance`, or place one in a scene. Extra instances are destroyed in `Awake`.

### Worker

`KTool.Worker.WorkManager` is a DontDestroyOnLoad singleton that fans out work to all registered handlers for a key, then waits until every `WorkHandler` finishes (or hits timeout / frame limit).

Supports handlers with `WorkHandler` plus 0–4 typed parameters. Signature must match between `Register` / `Unregister` / `Notify` / `CheckRegister`.

```csharp
using System;
using System.Collections.Generic;
using KTool.Worker;

// Keep a reference — Unregister must use the same delegate instance
Action<WorkHandler, string> loadLevel = (WorkHandler job, string levelId) =>
{
    // start async work, then:
    job.Progress = 1f;
    job.Success(); // or job.Fail() / job.Cancel()
};

WorkManager.Instance.Register("LoadLevel", loadLevel);

// Start all workers for this key, then receive results
WorkManager.Instance.Notify(
    "LoadLevel",
    "level_01",
    callback: (List<WorkHandler> handlers) =>
    {
        foreach (var h in handlers)
        {
            if (h.IsSuccess) { /* ok */ }
            else if (h.IsFailed) { /* fail */ }
            else if (h.IsTimeout) { /* timed out */ }
        }
    },
    timeout: 10f,   // seconds; -1 = no time limit
    frameLimit: -1  // frames; -1 = no frame limit
);

WorkManager.Instance.Unregister("LoadLevel", loadLevel);
```

`WorkHandler` states: `Pending` → `Success` / `Canceled` / `Failed` / `Timeout` (terminal; only transitions from Pending).

Notes:

- Duplicate registration of the same delegate is ignored.
- Inline lambdas cannot be unregistered later unless you stored them first.
- `Notify` snapshots handlers first, so `Register` / `Unregister` inside a worker is safe (same snapshot rules as Observer).
- Worker exceptions are logged; that worker is omitted from the returned handler list.
- Prefer unregister in `OnDisable` / `OnDestroy` to avoid dangling workers.
- Singleton: use `WorkManager.Instance`, or place one in a scene. Extra instances are destroyed in `Awake`.

| API | Role |
|--|--|
| `Register` / `Unregister` / `CheckRegister` | manage workers by key + signature |
| `Notify(..., callback, timeout, frameLimit)` | invoke workers and wait for completion |
| `WorkHandler.Progress` | 0–1 progress |
| `WorkHandler.Success/Fail/Cancel` | finish a job |
