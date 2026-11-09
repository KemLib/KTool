# Worker

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
