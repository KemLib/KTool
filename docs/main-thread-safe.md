# MainThreadSafe

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
