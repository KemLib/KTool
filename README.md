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
- **Attribute**: Inspector property attributes for picking assets, components, and Unity built-in values (`KTool.Attribute`).
- **Script Creater**: editor to create scripts from available templates, automatically adding Namespaces by folder.
- **FileIo**: file read/write library in asset or data.
- **Cron**: wait for conditions then run callbacks (`CronObject`).
- **Init**: stepped boot pipeline with progress, timeout, and optional scene chaining (`InitManager` / `InitContainer`).
- **MainThreadSafe**: marshal callbacks from any thread onto the Unity main thread (`MainThreadManager`).
- **Observer**: string-key pub/sub event bus (`ObserverManager`).
- **Worker**: string-key async work fan-out with progress and timeout (`WorkManager`).

### Advertisement

`KTool.Advertisement` is a provider-agnostic ad API: abstract formats, lifecycle, and per-show tracking. Real SDKs (e.g. AdMob) live in a separate package such as [KTool.Admob]. Until a provider sets `instance`, each format falls back to a uGUI Demo via `AdDemoManager`.

Formats: `AdAppOpen`, `AdBanner`, `AdInterstitial`, `AdRewarded` — all inherit `AdBase` (`MonoBehaviour`). State: `AdState` (`None` / `Loaded` / `Destroy`) plus `IsShow`. `IsReady` means loaded and not currently showing.

```csharp
using KTool.Advertisement;

// Prefetch
AdRewarded.Instance.Load();

// Show once — tracking covers this call only
IAdRewardedTracking tracking = AdRewarded.Instance.Show("level_win");
if (!tracking.IsComplete)
{
    Debug.Log(tracking.ErrorMessage);
    return;
}

tracking.OnAdReceivedReward += (AdRewarded src, AdRewardReceived reward, string placement) =>
{
    if (reward.IsReward) { /* grant */ }
};
tracking.OnAdHidden += (AdRewarded src, string placement) =>
{
    /* resume gameplay */
};

// Lifetime listeners on the ad object (every show)
AdRewarded.Instance.OnAdRevenuePaid += (AdBase src, AdRevenuePaid revenue, string placement) =>
{
    /* analytics */
};

// Banner: show / hide
IAdBannerTracking banner = AdBanner.Instance.Show("home");
AdBanner.Instance.Hide();
```

`Show(placement)` flow (all formats):

1. Save current placement; assign the new one (empty → `"unknown_placement"`).
2. Call abstract `OnShow(out error)`.
3. On success: return a complete tracking source and store it for dual-dispatch.
4. On failure: return an incomplete tracking source (`IsComplete == false`) and restore the old placement.

Events fire on both the ad object and the current tracking source (`PushEvent_*`). Banner also has `Hide` / `OnAdExpanded`; Rewarded adds `OnAdReceivedReward` / `AdRewardReceived`.

Implement a provider: subclass a format, override `Load` / `OnShow` / `Destroy` (and `OnHide` for banner), assign `protected static instance` in `Awake`/`Init`, and map SDK callbacks to `PushEvent_*`.

Notes:

- Access via `AdAppOpen.Instance` / `AdBanner.Instance` / `AdInterstitial.Instance` / `AdRewarded.Instance`. If `instance` is null, Demo ads load from Resources (`KTool/Advertisement/Demo/AdManagerDemo`) as DontDestroyOnLoad.
- Subscribe on the tracking returned by `Show` for one impression; subscribe on the ad object for analytics across shows.
- `IsAutoReload` is serialized on `AdBase` but unused by the core — providers decide reload policy.
- `AdRevenuePaid` carries source, network, id, type, country, placement, value, and currency. Demo fullscreen ads emit value `0` on hide.
- Banner Demo toggles UI Images by `AdPosition`; AppOpen / Interstitial / Rewarded Demo use a timed panel. Production builds should register a real provider early so Demo is never shown by mistake.
- Consent, mediation, and waterfall are out of scope here — they belong in the SDK package.

| API | Role |
|--|--|
| `AdBase.Load` / `Destroy` / `IsReady` / `IsShow` | shared lifecycle |
| `AdAppOpen` / `AdInterstitial`.`Show` → `IAdTracking` | fullscreen show + per-show tracking |
| `AdRewarded.Show` → `IAdRewardedTracking` | rewarded show (+ `OnAdReceivedReward`) |
| `AdBanner.Show` → `IAdBannerTracking` / `Hide` | banner show/hide (+ `OnAdExpanded`) |
| `AdTrackingSource` / `IAdTracking` | `IsComplete`, `ErrorMessage`, display/hide/click/revenue events |
| `AdRevenuePaid` / `AdRewardReceived` | revenue and reward payloads |
| `AdType` / `AdState` / `AdSize` / `AdPosition` | enums for type, lifecycle, banner layout |
| `AdDemoManager` / `AdDemo*` | built-in fake ads when no provider is set |

### Attribute

`KTool.Attribute` adds Inspector helpers as `PropertyAttribute`s. Runtime markers live under `Packages/KTool/Runtime/Attribute`; matching `PropertyDrawer`s live under `Packages/KTool/Editor/Attribute`. Put attributes on serialized fields only (`AllowMultiple = false`, `Inherited = false`).

Two fill styles:

- **Get\*** — auto-sync the field from a source (folder or hierarchy). Best on arrays: the drawer reloads the whole array when the first element draws.
- **Select\*** — dropdown to pick one value (or null when allowed).

```csharp
using KTool.Attribute;
using KTool.FileIo;
using UnityEngine;

public class AttributeDemo : MonoBehaviour
{
    [GetAsset("Assets/Sprites", ExtensionType.Png)]
    public Sprite[] sprites;

    [SelectAsset("Assets/Prefabs", "prefab", allowNull: true)]
    public GameObject prefab;

    [GetComponent(GetComponentType.InGameObject_Children)]
    public Collider2D[] colliders;

    [SelectComponent(GetComponentType.InChildren, allowInactive: false, allowNull: true)]
    public Rigidbody2D body;

    [SelectLayer] public int layerId;
    [SelectLayer] public string layerName;

    [SelectSortingLayer] public string sortingLayer;
    [SelectScene] public string sceneName;
    [SelectTag] public string tagName;
    [SelectAgentId] public int agentTypeId;

    [DisabledChange]          // toggle Enable/Disable in Inspector
    public string lockedId;

    [DisabledChange(false)]   // always read-only
    public int fixedValue;
}
```

| Attribute | Field types | Description |
|--|--|--|
| `GetAsset(folder, extension)` | `string[]`, `Object[]` (and subclasses) | Fill array with all matching assets in `folder`. `extension` is `ExtensionType` or a string (normalized via `PathUnit`). |
| `SelectAsset(folder, extension, allowNull = false)` | `string`, `string[]`, `Object`, `Object[]` | Dropdown of assets in `folder` (optional `<Null>`). Uses `AssetFinder` / `AssetDatabase`. |
| `GetComponent(type = InGameObject, allowInactive = false)` | `Component`, `Component[]` | Auto-assign components from this GameObject and/or children (`GetComponentType`). Target must be a `MonoBehaviour`. |
| `SelectComponent(type = InGameObject, allowInactive = false, allowNull = false)` | `Component`, `Component[]` | Dropdown of matching components; labels look like `GameObject - Type - Id[n]`. |
| `SelectAgentId` | `int`, `int[]` | Dropdown of NavMesh agent type IDs (`AttributeUnit.Array_Agent`). |
| `SelectLayer` | `int`, `int[]`, `string`, `string[]` | Dropdown of named layers. `int` stores **layer id** (0–31), not popup index. |
| `SelectSortingLayer` | `int`, `int[]`, `string`, `string[]` | Dropdown of sorting layer names / indices. |
| `SelectScene` | `int`, `int[]`, `string`, `string[]` | Dropdown of **enabled** scenes in Build Settings (Editor-only list). |
| `SelectTag` | `int`, `int[]`, `string`, `string[]` | Dropdown of project tags (Editor-only list). |
| `DisabledChange(enableChange = true)` | any serializable field | `true`: Enable/Disable toggle around the field. `false`: always disabled. |

`GetComponentType` (shared by Get/Select Component):

| Value | Search scope |
|--|--|
| `InGameObject` | Current GameObject only |
| `InChildren` | Direct children only |
| `InAllChildren` | All descendants |
| `InGameObject_Children` | Current + direct children |
| `InGameObject_AllChildren` | Current + all descendants |

Notes:

- Asset attributes depend on `KTool.FileIo` (`PathUnit`, `ExtensionType`, `AssetFinder`). Folder paths are project-relative (e.g. `Assets/...`).
- Shared Editor helpers: `EditorGui_Draw` (popups / array sync), `AttributeUnit` (layers, tags, scenes, sorting layers, NavMesh agents).
- Wrong field types show an error label in the Inspector instead of drawing the default field.
- Scene / tag option lists are empty outside the Editor; prefer these attributes for authoring, not player runtime discovery.

### Script Creater

Editor tool under `KTool.AssetCreater` that creates C# scripts from templates, fills placeholders (namespace, access modifier, type name), and writes the file next to the selected folder/asset.

Usage:

 1. In the Project window, select a folder or an existing asset.
 2. Right-click → `KTool` → `Create Script`.
 3. Pick a template, adjust namespace / name / access modifier as needed.
 4. Click **Save**.

Built-in templates (`Packages/KTool/Editor/AssetCreater/Script/Templates`):

| Template | Description |
|--|--|
| MonoBehaviour | standard `MonoBehaviour` class |
| MonoBehaviour_Instance | singleton-style `MonoBehaviour` |
| ScriptableObject | `ScriptableObject` with `CreateAssetMenu` |
| CustomEditor_MonoBehaviour | `CustomEditor` for a MonoBehaviour |
| Class / Class_Serializable | plain / `[Serializable]` class |
| Interface / Struct / Enum | interface / struct / enum |

Namespace is derived from the selected folder path:

- Strip leading `Assets` or `Packages`.
- If the third path segment is `Runtime`, drop it.
- If the third path segment is `Editor`, move it to the end of the namespace.
- Path segments are joined with `.` (package ids that already contain dots, e.g. `com.kem.ktool`, stay as one segment).

Examples:

| Folder | Namespace |
|--|--|
| `Assets/MyGame/Runtime/Scripts` | `MyGame.Scripts` |
| `Assets/MyGame/Editor/Scripts` | `MyGame.Scripts.Editor` |
| `Packages/com.kem.ktool/Runtime/FileIo` | `com.kem.ktool.FileIo` |
| `Packages/com.kem.ktool/Editor/AssetCreater` | `com.kem.ktool.AssetCreater.Editor` |

Template placeholders (configured via `SettingReplace*` assets):

| Placeholder | Role |
|--|--|
| `[Namespace]` | namespace from folder (editable in the window) |
| `[AccessModifier]` | access modifier dropdown |
| `[NameScript]` | type / file name |
| `[NameScriptEditor]` | editor class name (CustomEditor template) |

Customize templates or replace rules via `SettingCreateScript` (cloned to `Assets/KTool/Editor/SettingCreateScript.asset` on first load, or edit the package copy under `Packages/KTool/Editor/AssetCreater/Script`).

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

`Notify` returns a `WorkState` you can inspect while the batch is running (`ListWorker`, `Timeout`, `FrameLimit`, `StartTime`, `FrameCount`, `IsDone`). The completion callback receives `IReadOnlyList<IWorker>`.

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
    callback: (IReadOnlyList<IWorker> workers) =>
    {
        foreach (var w in workers)
        {
            if (w.IsSuccess) { /* ok */ }
            else if (w.IsFailed) { /* fail */ }
            else if (w.IsTimeout) { /* timed out */ }
            else if (w.IsCanceled) { /* canceled */ }
        }
    },
    timeout: 10f,   // seconds (Time.time); <= 0 = no time limit
    frameLimit: -1  // Update polls; <= 0 = no frame limit
);

// Optional: poll while waiting
// if (state.IsDone) { ... }
// foreach (var w in state.ListWorker) { ... }

WorkManager.Instance.Unregister("LoadLevel", loadLevel);
```

`WorkHandler` states: `Pending` → `Success` / `Canceled` / `Failed` / `Timeout` (terminal; only transitions from Pending). Constructed only by the manager (`internal` ctor). `Timeout` is applied by `WorkState` (internal); callers finish work with `Success` / `Fail` / `Cancel`. `IWorker` exposes status plus `Cancel()`.

Notes:

- Duplicate registration of the same delegate is ignored.
- Inline lambdas cannot be unregistered later unless you stored them first.
- `Notify` snapshots handlers first, so `Register` / `Unregister` inside a worker is safe (same snapshot rules as Observer).
- Worker exceptions are logged; that worker is omitted from `ListWorker` / the callback list.
- If no handlers are registered for the key, `Notify` still returns a `WorkState` with an empty `ListWorker`; the callback runs on the next `Update` (not synchronously).
- On time timeout, remaining pending handlers are marked `Timeout`, then the callback runs once.
- On frame limit: after `FrameLimit` Update polls, if any handler is still pending, remaining pending handlers are marked `Timeout`, then the callback runs once (`IsDone` stays true afterward).
- `Progress` reads as `1` once the handler is done; set is ignored after done and otherwise clamps to 0–1.
- Prefer unregister in `OnDisable` / `OnDestroy` to avoid dangling workers.
- Singleton: use `WorkManager.Instance`, or place one in a scene. Extra instances are destroyed in `Awake`.

| API | Role |
|--|--|
| `Register` / `Unregister` / `CheckRegister` | manage workers by key + signature |
| `Notify(...)` → `WorkState` | invoke workers and wait for completion |
| `WorkState.ListWorker` / `IsDone` / `FrameCount` | inspect the in-flight batch (`IReadOnlyList<IWorker>`) |
| `IWorker.Progress` / status / `Cancel` | read progress & outcome; cancel from outside |
| `WorkHandler.Success` / `Fail` / `Cancel` | finish a job from the registered handler |
