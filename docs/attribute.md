# Attribute

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
