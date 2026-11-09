# Cron

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
