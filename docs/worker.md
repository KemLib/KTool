# Worker

`KTool.Worker.WorkManager` is a DontDestroyOnLoad singleton that fans out work to all registered handlers for a key, then waits until every `WorkHandler` finishes (or hits timeout / frame limit / cancel).

Supports handlers with `WorkHandler` plus 0–4 typed parameters. Signature must match between `Register` / `Unregister` / `Notify` / `CheckRegister`.

`Notify` returns a `WorkState` you can inspect while the batch is running (`ListWorker`, `Progress`, `Timeout`, `FrameLimit`, `StartTime`, `FrameCount`, status flags). The completion callback is `Action<WorkState>` and runs once when the batch reaches a terminal state.

```csharp
using System;
using KTool.Worker;

// Keep a reference — Unregister must use the same delegate instance
Action<WorkHandler, string> loadLevel = (WorkHandler job, string levelId) =>
{
    // start async work, then:
    job.Progress = 1f;
    job.Success(); // or job.Fail()
};

WorkManager.Instance.Register("LoadLevel", loadLevel);

// Start all workers for this key; WorkState tracks the batch until callback runs
WorkState state = WorkManager.Instance.Notify(
    "LoadLevel",
    "level_01",
    callback: (WorkState batch) =>
    {
        if (batch.IsSuccess) { /* all handlers succeeded */ }
        else if (batch.IsFailed) { /* at least one handler failed / non-success */ }
        else if (batch.IsTimeout) { /* time or frame limit */ }
        else if (batch.IsCanceled) { /* SetCancel() */ }

        foreach (var w in batch.ListWorker)
        {
            if (w.IsSuccess) { /* ok */ }
            else if (w.IsFailed) { /* fail */ }
            else if (w.IsTimeout) { /* timed out */ }
            else if (w.IsCanceled) { /* canceled with the batch */ }
        }
    },
    timeout: 10f,   // seconds (Time.time); <= 0 = no time limit
    frameLimit: -1  // Update polls; <= 0 = no frame limit
);

// Optional: poll while waiting, or cancel the whole batch
// if (state.IsDone) { ... }
// state.Progress
// state.SetCancel();

WorkManager.Instance.Unregister("LoadLevel", loadLevel);
```

## States

`WorkHandler` and `WorkState` share the same outcome model (`StateType`): `Pending` → `Success` / `Canceled` / `Failed` / `Timeout` (terminal; only transitions from `Pending`).

| Type | Who finishes it |
|--|--|
| `WorkHandler` | Handler code calls public `Success()` / `Fail()`. `Cancel()` / `Timeout()` are `internal` — applied by `WorkState`. |
| `WorkState` | Manager polls via `CheckAllDone` → success / failed / timeout; callers cancel the batch with public `SetCancel()`. |

`WorkHandler` is constructed only by the registry (`internal` ctor).

Batch outcome:

- **Success** — every handler in `ListWorker` is done and successful (empty `ListWorker` counts as success).
- **Failed** — every handler is done, and at least one is not success.
- **Timeout** — time or frame limit hit while something is still pending; remaining pending handlers are marked `Timeout`, then the callback runs once.
- **Canceled** — `SetCancel()` while still pending; remaining pending handlers are marked `Canceled`, then the callback runs once.

Aggregate flags (`batch.IsTimeout` / `IsCanceled`) describe the batch. Individual handlers that already finished keep their own outcome.

## Notes

- Duplicate registration of the same delegate is ignored.
- Inline lambdas cannot be unregistered later unless you stored them first.
- `Notify` snapshots handlers first, so `Register` / `Unregister` inside a worker is safe (same snapshot rules as Observer).
- Worker exceptions are logged; that worker is omitted from `ListWorker` (and thus from batch wait / cancel / timeout). If the handler already started async work before throwing, that work is not tracked.
- `Register` / `Notify` arity must match. Handlers registered as `Action<WorkHandler, T>` are only invoked by `Notify<T>(...)`; a plain `Notify(key, callback)` will not call them (empty batch → success on next `Update`).
- If no handlers are registered for the key (or none match the arity), `Notify` still returns a `WorkState` with an empty `ListWorker`; the callback runs on the next `Update` (not synchronously). Sync `Success()` inside a handler also completes on the next `Update`, not in the same call as `Notify`.
- `WorkHandler.Progress` reads as `1` once the handler is done; set is ignored after done and otherwise clamps to 0–1. `WorkState.Progress` is the average of handler progress (updated each poll before completion).
- Terminal transition fires the callback at most once (`TryCompareExchange` from `Pending`).
- Prefer unregister in `OnDisable` / `OnDestroy` to avoid dangling workers.
- Singleton: use `WorkManager.Instance`, or place one in a scene. Extra instances are destroyed in `Awake`. Destroying the active manager clears registries and in-flight `WorkState` entries without calling their callbacks.

## API

| API | Role |
|--|--|
| `Register` / `Unregister` / `CheckRegister` | manage workers by key + signature |
| `Notify(...)` → `WorkState` | invoke matching workers and wait for completion |
| `WorkState.ListWorker` | `IReadOnlyList<WorkHandler>` for this batch |
| `WorkState.IsDone` / `IsSuccess` / `IsFailed` / `IsTimeout` / `IsCanceled` | batch outcome |
| `WorkState.Progress` / `FrameCount` / `StartTime` | inspect while in flight |
| `WorkState.SetCancel()` | cancel the batch from outside (marks pending handlers, runs callback once) |
| `WorkHandler.Progress` / status flags | read progress & outcome per job |
| `WorkHandler.Success` / `Fail` | finish a job from the registered handler |
