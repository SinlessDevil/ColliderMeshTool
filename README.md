# Collider Mesh Tool

Unity Editor toolkit for generating MeshColliders, drawing collision outlines manually, and batch-configuring prefab renderers.

[![Unity](https://img.shields.io/badge/Unity-2023.3%2B-000000?style=flat-square&logo=unity)](https://unity.com/)
[![License](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](LICENSE)
[![Releases](https://img.shields.io/badge/Releases-latest-green?style=flat-square)](https://github.com/SinlessDevil/ColliderMeshTool/releases)

---

## Overview

Collider Mesh Tool builds geometric walls around one or several scene objects by analyzing their outermost vertices and generating a closed mesh boundary from them. Instead of hand-placing box colliders around complex level geometry, you select the objects, tune a few parameters, and the tool produces a single optimized `MeshCollider` that follows their silhouette.

The package ships as three independent modules — use all of them together or pick only what you need.

| Module | Purpose |
| --- | --- |
| `ColliderMeshCreator` | Editor window for automatic and manual collider generation |
| `ConcaveHull` | Runtime geometry API for 2D concave hulls on the XZ plane |
| `PrefabSetupEditor` | Batch renderer and material configuration |

---

## Demo

[![Watch the demo](https://img.youtube.com/vi/ZrX0ZoAVDn0/0.jpg)](https://www.youtube.com/watch?v=ZrX0ZoAVDn0)

---

## Modules

### ColliderMeshCreator

Generates custom MeshColliders either automatically from `MeshFilter` components or manually via `ManualOutlineDrawer` directly in the Scene view.

**Capabilities**

- Concavity, scale factor, and Y-threshold control
- Offset height and extrusion depth
- Optional Catmull–Rom smoothing for curved outlines
- Debug material support

**Opening the window**

```
Tools > Collider Mesh Generator Editor Window
```

**Scene view shortcuts**

| Action | Shortcut |
| --- | --- |
| Add point | <kbd>Q</kbd> |
| Remove point | <kbd>E</kbd> |

[Download release →](https://github.com/SinlessDevil/ColliderMeshTool/releases/tag/collider-mesh-creator-v1.0.0)

---

### ConcaveHull

Lightweight runtime plugin for generating 2D concave hulls on the XZ plane.

**API**

```csharp
Hull.SetConvexHull(List<Node> nodes);
Hull.SetConcaveHull(double concavity, double scaleFactor);
Hull.CleanUp();
```

**Data types**

| Type | Description |
| --- | --- |
| `Node` | 2D point with a unique ID |
| `Line` | Connection between two nodes |

![ConcaveHull example](https://github.com/user-attachments/assets/52d27373-eabb-400f-a69f-d03cb41d4327)

[Download release →](https://github.com/SinlessDevil/ColliderMeshTool/releases/tag/concave-hull-v1.0.0)

---

### PrefabSetupEditor

Configures renderers and materials across prefabs and scene objects in bulk.

**Capabilities**

- Recursive material assignment through object hierarchies
- Filtering and randomization based on mesh name
- Per-renderer configuration of shadow casting, light probe usage, global illumination, and motion vectors

![PrefabSetupEditor](https://github.com/user-attachments/assets/4fa7d16a-4f0a-4d0b-a4b3-8ea4e2391500)

[Download release →](https://github.com/SinlessDevil/ColliderMeshTool/releases/tag/prefab-setup-editor-v1.0.0)

---

## Requirements

| Dependency | Version | Required |
| --- | --- | --- |
| Unity | 2023.3 or newer | Yes |
| [Odin Inspector](https://odininspector.com/) | Latest | Yes |
| [ConcaveHull](https://github.com/SinlessDevil/ColliderMeshTool/releases/tag/concave-hull-v1.0.0) | 1.0.0 | For mesh generation |

---

## Installation

1. Download the `.unitypackage` from the [Releases](https://github.com/SinlessDevil/ColliderMeshTool/releases) page.
2. Import it into your Unity project via `Assets > Import Package > Custom Package`.
3. Install Odin Inspector, and ConcaveHull if you plan to use mesh generation.
4. Open `Tools > Collider Mesh Generator Editor Window` to get started.

---

## Usage

```csharp
// Runtime hull generation example
var nodes = vertices.Select((v, i) => new Node(v.x, v.z, i)).ToList();

Hull.SetConvexHull(nodes);
Hull.SetConcaveHull(concavity: 1.0, scaleFactor: 1);

var outline = Hull.HullConcaveEdges;
```

---

## License

Distributed under the MIT License. See [`LICENSE`](LICENSE) for details.

---

## Author

**SinlessDevil** — [GitHub](https://github.com/SinlessDevil)
