# 🧱 Collider Mesh Tool

**Collider Mesh Tool** is a powerful Unity Editor utility that combines three key systems:
📐 MeshCollider generation,  
✍️ manual outline drawing,  
🛠 and batch prefab configuration.

> ✅ Built with Odin Inspector  
> ✅ Supports both algorithmic and manual mesh generation  
> ✅ All modules can be used independently

## ✨ Modules

### 📦 ColliderMeshCreator
Editor window for generating custom MeshColliders:
- 🔹 Automatically from MeshFilter objects
- 🔹 Or manually using `ManualOutlineDrawer` in the Scene view

**Features:**
- Concavity, scale factor, and Y-threshold control  
- Offset height and extrusion depth  
- Optional Catmull-Rom smoothing for curved outlines  
- Debug material support

**Editor Window:**  
`Tools > Collider Mesh Generator Editor Window`


https://github.com/user-attachments/assets/e4840d39-c845-424d-ace3-f5f83c73e188

https://github.com/user-attachments/assets/662e35fc-51b7-4b02-90de-df26595698be



![manual-draw](https://github.com/user-attachments/assets/23b4fcb7-6650-4e89-912b-775de6a5075c)
![image](https://github.com/user-attachments/assets/5288205c-d1f6-4791-94ad-f718115696ab)
![image](https://github.com/user-attachments/assets/f7aa8582-9a20-4713-bf42-5bedef77c5a4)

### 🔧 Quick Controls
| Action            | Shortcut |
|-------------------|----------|
| Add point         | `Q`      |
| Remove point      | `E`      |
| Open editor       | `Tools > Collider Mesh Generator` |

---
👉 [View Release Collider Mesh Tool](https://github.com/SinlessDevil/ColliderMeshTool/releases/tag/collider-mesh-creator-v1.0.0)

### 📦 ConcaveHull v1.0.0 — Geometry API
Lightweight runtime plugin for generating 2D concave hulls on the XZ plane.

**API:**
- `Hull.SetConvexHull(List<Node>)`
- `Hull.SetConcaveHull(concavity, scaleFactor)`
- `Hull.CleanUp()`

**Data Types:**
- `Node` – 2D point with ID
- `Line` – connection between two Nodes
  
![hull-example](https://github.com/user-attachments/assets/52d27373-eabb-400f-a69f-d03cb41d4327)  

---
👉 [View Release ConcaveHull ](https://github.com/SinlessDevil/ColliderMeshTool/releases/tag/concave-hull-v1.0.0)

### 📦 PrefabSetupEditor v1.0.0
Efficient tool for setting up renderers and materials across prefabs and scene objects.

**Features:**
- Recursive material assignment
- Filter and randomize based on mesh name
- Configure:
  - Shadow casting
  - Light probe usage
  - Global illumination
  - Motion vectors and more

![prefab-editor](https://github.com/user-attachments/assets/b2c48312-dabe-4191-9e40-ac59bf64b620) 

---
👉 [View Release PrefabSetupEditor](https://github.com/SinlessDevil/ColliderMeshTool/releases/tag/prefab-setup-editor-v1.0.0)

## 🧰 Requirements
- Unity **2021.3+**
- ✅ [Odin Inspector](https://odininspector.com/) (Required)
- ✅ [ConcaveHull](https://github.com/SinlessDevil/EcsStickmanSurvivors/releases/tag/ConcaveHull-v1.0.0) (for mesh generation)

## 🚀 Installation
1. Download the `.unitypackage` from [Releases](https://github.com/SinlessDevil/ColliderMeshTool/releases)
2. Import it into your Unity project
3. Install Odin Inspector and (optionally) ConcaveHull
4. Done! 🎉
