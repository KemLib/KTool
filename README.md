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
- **Init**: stepped boot pipeline with progress, timeout, and optional scene chaining (`InitManager` / `InitContainer`).
- **MainThreadSafe**: marshal callbacks from any thread onto the Unity main thread (`MainThreadManager`).
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

You can create a new template script, see the SettingCreateScript file in the "Packages/KTool/Editor/AssetCreater/Script" folder.

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

`KTool.Init.InitManager` is a DontDestroyOnLoad singleton that runs an `InitContainer` in a loaded scene, reports progress, then optionally loads the next scene and continues the chain.

Steps run in order. Within a step, every `IIniter` on the listed GameObjects starts together; the step finishes when all returned `IInitTracking` values complete (or, past `TimeLimit`, when every *indispensable* tracker completes).

Setup (Editor):

 1. `GameObject → KTool → Init → Create Container` (or add `InitContainer` yourself).
 2. Fill `Steps`: each step names GameObjects that carry `IIniter` components (e.g. ads SDK init in step 1, ad preload in step 2).
 3. Optional: enable `Next Scene`, pick `Next Scene Name` / `Load Scene Mode`.
 4. `GameObject → KTool → Init → Create Manager` (or add `InitManager`). Enable `Auto Init` to start on `Awake`, or call `Init()` yourself.
 5. Play.

```csharp
using KTool.Init;
using UnityEngine;

// Implement on components referenced by InitStep GameObjects
public class AdsInit : MonoBehaviour, IIniter
{
    public IInitTracking InitBegin()
    {
        // Already done — use shared terminals
        // return IInitTracking.Success; // or IInitTracking.Fail

        InitTrackingSource tracking = new InitTrackingSource(indispensable: true);
        // kick off async work, then:
        // tracking.Progress = 0.5f;
        // tracking.CompleteSuccess(); // or CompleteFail("reason")
        return tracking;
    }

    public void InitEnd()
    {
        // cleanup after the step (or container) ends
    }
}

// Manual start (when Auto Init is off)
InitManager.Instance.Init();                 // active scene
InitManager.Instance.Init("Boot");           // by scene name
InitManager.Instance.Init(0);                // by build index
```

Notes:

- `InitManager` finds `InitContainer` on active root GameObjects of the target scene (`GetComponentInChildren`). Missing / already-running / already-finished containers log an error and abort.
- Concurrent inits are rejected (`IsInitializing`). Extra `InitManager` instances are destroyed in `Awake`.
- Progress: container steps are weighted evenly; if `Next Scene` is on, init ≈ 80% and scene load ≈ 20% of the remaining progress slice. Chained scenes re-split the leftover progress the same way.
- After a successful container with `Next Scene`, the manager loads that scene (`Cron` + `ConditionAsyncOperation`), then runs the new scene’s `InitContainer` if present; if none is found, it treats the chain as success.
- `TimeLimit` (realtime seconds, `≤ 0` = no limit): before the limit, all trackers must finish; after it, only `Indispensable` trackers must finish to advance. A failed indispensable tracker fails the container.
- `InitBegin` exceptions become a failed indispensable `InitTrackingSource` with the exception message. `null` tracking is ignored (sync init with nothing to wait on).
- Wire UI via UnityEvents: manager `onBegin` / `onProgress` / `onSuccess` / `onFail`; container `onBegin` / `onStep` / `onProgress` / `onEnd`. Poll `InitManager.Progress` / `InitContainer.Progress` as needed.

| API | Role |
|--|--|
| `InitManager.Init(...)` | start pipeline (active scene / name / build index / `Scene`) |
| `InitManager.IsInitializing` / `Progress` | in-flight state and 0–1 progress |
| `InitContainer` steps / `TimeLimit` / `NextScene*` | ordered steps, timeout, optional scene chain |
| `IIniter.InitBegin` / `InitEnd` | start work (return tracker) / cleanup |
| `InitTrackingSource` / `IInitTracking.Success`/`Fail` | report progress and complete success/fail |

### MainThreadSafe

`KTool.MainThreadSafe.MainThreadManager` queues `IAction`s (via `KLibStandard.Concurrent.Queue.TwiceQueue`) and drains them on the Unity main thread in `Update`. Bootstraps itself with `[RuntimeInitializeOnLoadMethod]` as a DontDestroyOnLoad singleton (`KTool_MainThreadManager`).

Supports `Action` with 0–4 typed parameters. Use the default queue, or `GetNewQueue()` for an isolated `IQueueIn<IAction>` producer.

```csharp
using System;
using KTool.MainThreadSafe;
using KLibStandard.Concurrent.Queue;

// From a background thread — runs on the next main-thread Update
MainThreadManager.Add(() => Debug.Log("On main thread"));

MainThreadManager.Add((int coin) => Debug.Log($"Coin: {coin}"), 100);

// Optional: dedicated producer queue (useful when many producers enqueue in parallel)
IQueueIn<IAction> queue = MainThreadManager.GetNewQueue();
MainThreadManager.Add(() => Debug.Log("Custom queue"), queue);
```

Notes:

- Manager is created automatically at runtime load; you do not need to place it in a scene.
- `Add` with a `null` custom queue is a no-op for that overload; the default-queue overloads always enqueue.
- Queued action exceptions are logged and do not stop other actions in the same drain.
- Extra manager components are destroyed in `Awake` when an instance already exists.
- `ActionT0`–`ActionT4` are internal wrappers; call `MainThreadManager.Add` rather than constructing them yourself.

| API | Role |
|--|--|
| `MainThreadManager.Add(...)` | enqueue an action (0–4 state args; optional custom queue) |
| `MainThreadManager.GetNewQueue()` | create a new `IQueueIn<IAction>` producer into the shared drain |
| `MainThreadManager.Init` | `[RuntimeInitializeOnLoadMethod]` bootstrap (usually automatic) |

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

`Notify` returns a `WorkState` you can inspect while the batch is running (`ListHandlers`, `Timeout`, `FrameLimit`, `StartTime`, `FrameCount`, `IsDone`).

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

// Start all workers for this key; WorkState tracks the batch until callback runs
WorkState state = WorkManager.Instance.Notify(
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

// Optional: poll while waiting
// if (state.IsDone) { ... }

WorkManager.Instance.Unregister("LoadLevel", loadLevel);
```

`WorkHandler` states: `Pending` → `Success` / `Canceled` / `Failed` / `Timeout` (terminal; only transitions from Pending). Constructed only by the manager (`internal` ctor).

Notes:

- Duplicate registration of the same delegate is ignored.
- Inline lambdas cannot be unregistered later unless you stored them first.
- `Notify` snapshots handlers first, so `Register` / `Unregister` inside a worker is safe (same snapshot rules as Observer).
- Worker exceptions are logged; that worker is omitted from the handler list passed to the callback.
- If no handlers are registered for the key, `Notify` still returns a `WorkState` with an empty `ListHandlers`; the callback runs on the next `Update` (not synchronously).
- On timeout / frame limit, remaining pending handlers are marked `Timeout`, then the callback runs once (`IsDone` stays true afterward).
- `Progress` reads as `1` once the handler is done; set clamps to 0–1.
- Prefer unregister in `OnDisable` / `OnDestroy` to avoid dangling workers.
- Singleton: use `WorkManager.Instance`, or place one in a scene. Extra instances are destroyed in `Awake`.

| API | Role |
|--|--|
| `Register` / `Unregister` / `CheckRegister` | manage workers by key + signature |
| `Notify(...)` → `WorkState` | invoke workers and wait for completion |
| `WorkState.ListHandlers` / `IsDone` / `FrameCount` | inspect the in-flight batch |
| `WorkHandler.Progress` | 0–1 progress (`1` when done) |
| `WorkHandler.Success` / `Fail` / `Cancel` | finish a job |
