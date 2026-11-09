# Script Creater

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
