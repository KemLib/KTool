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

- **Advertisement**: easily implement advertising display features in the application, see [docs/advertisement.md](docs/advertisement.md) and [KTool.Admob].
- **Attribute**: Inspector property attributes for picking assets, components, and Unity built-in values (`KTool.Attribute`), see [docs/attribute.md](docs/attribute.md).
- **Script Creater**: editor to create scripts from available templates, automatically adding Namespaces by folder, see [docs/script-creater.md](docs/script-creater.md).
- **FileIo**: file read/write library in asset or data.
- **Cron**: wait for conditions then run callbacks (`CronObject`), see [docs/cron.md](docs/cron.md).
- **Init**: stepped boot pipeline with progress, timeout, and optional scene chaining (`InitManager` / `InitContainer`), see [docs/init.md](docs/init.md).
- **MainThreadSafe**: marshal callbacks from any thread onto the Unity main thread (`MainThreadManager`), see [docs/main-thread-safe.md](docs/main-thread-safe.md).
- **Observer**: string-key pub/sub event bus (`ObserverManager`), see [docs/observer.md](docs/observer.md).
- **Worker**: string-key async work fan-out with progress and timeout (`WorkManager`), see [docs/worker.md](docs/worker.md).
