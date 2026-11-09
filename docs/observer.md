# Observer

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
