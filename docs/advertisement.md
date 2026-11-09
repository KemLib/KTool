# Advertisement

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
