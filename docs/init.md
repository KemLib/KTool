# Init

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
