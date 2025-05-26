using System;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Code.Editors
{
    public class PrefabSetupEditorWindow : OdinEditorWindow
    {
        [MenuItem("Tools/Prefab Batch Editor Window")]
        private static void OpenWindow() => GetWindow<PrefabSetupEditorWindow>().Show();

        [BoxGroup("Root Object")]
        [LabelText("Target Prefab or Scene Object")]
        [SerializeField] private GameObject _rootObject;

        // -------------------- Material Setup --------------------

        [BoxGroup("Set Material To Children")]
        [LabelText("Target Material")]
        [SerializeField] private Material _targetMaterial;

        [BoxGroup("Set Material To Children")]
        [Space]
        [LabelText("Override All Material Slots")]
        [SerializeField] private bool _overrideAllSlots = true;

        [FormerlySerializedAs("TargetMaterialIndex")]
        [BoxGroup("Set Material To Children")]
        [ShowIf("@!_overrideAllSlots")]
        [LabelText("Target Material Slot Index"), MinValue(0)]
        [SerializeField] private int _targetMaterialIndex = 0;

        [BoxGroup("Set Material To Children")]
        [Button(ButtonSizes.Large)]
        [GUIColor(0.3f, 0.8f, 1f)]
        private void ApplyMaterialToChildren()
        {
            if (!ValidateRootAndMaterial()) 
                return;

            Renderer[] renderers = GetAllRenderers();
            foreach (Renderer renderer in renderers)
                ReplaceMaterial(renderer);

            Debug.Log($"<color=green>Replaced materials in {renderers.Length} renderers under '{_rootObject.name}'.</color>");
        }

        private bool ValidateRootAndMaterial()
        {
            if (_rootObject != null && _targetMaterial != null) 
                return true;
            
            Debug.LogError("<color=red>Assign both a Root Object and Material.</color>");
            return false;
        }

        private Renderer[] GetAllRenderers()
        {
            MeshRenderer[] meshRenderers = _rootObject.GetComponentsInChildren<MeshRenderer>(true);
            SkinnedMeshRenderer[] skinnedRenderers = _rootObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            return meshRenderers.Cast<Renderer>().Concat(skinnedRenderers).ToArray();
        }

        private void ReplaceMaterial(Renderer renderer)
        {
            Material[] materials = renderer.sharedMaterials;
            if (_overrideAllSlots)
            {
                for (int i = 0; i < materials.Length; i++)
                    materials[i] = _targetMaterial;
            }
            else if (_targetMaterialIndex < materials.Length)
            {
                materials[_targetMaterialIndex] = _targetMaterial;
            }
            renderer.sharedMaterials = materials;
        }

        // -------------------- Random Material By Mesh Name --------------------

        [BoxGroup("Set Random Material for Matching Mesh")]
        [LabelText("Mesh Name Contains")]
        [SerializeField] private string _meshNameContains;

        [BoxGroup("Set Random Material for Matching Mesh")]
        [LabelText("Possible Materials")]
        [SerializeField] private Material[] _materialsToApply;

        [BoxGroup("Set Random Material for Matching Mesh")]
        [Button(ButtonSizes.Large)]
        [GUIColor(0.6f, 1f, 0.6f)]
        private void ApplyRandomMaterialToMatchingMeshNames()
        {
            if (_rootObject == null || string.IsNullOrWhiteSpace(_meshNameContains) || _materialsToApply?.Length == 0)
            {
                Debug.LogError("<color=red>Assign mesh name filter, materials and root object.</color>");
                return;
            }

            int count = 0;
            MeshFilter[] meshFilters = _rootObject.GetComponentsInChildren<MeshFilter>(true);
            foreach (MeshFilter filter in meshFilters)
            {
                if (filter.sharedMesh == null || !filter.sharedMesh.name.Contains(_meshNameContains))
                    continue;

                Renderer renderer = filter.GetComponent<Renderer>();
                if (renderer == null) continue;

                ApplyRandomMaterialToRenderer(renderer);
                count++;
            }

            SkinnedMeshRenderer[] skinnedRenderers = _rootObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            foreach (SkinnedMeshRenderer skinned in skinnedRenderers)
            {
                if (skinned.sharedMesh == null || !skinned.sharedMesh.name.Contains(_meshNameContains))
                    continue;

                ApplyRandomMaterialToRenderer(skinned);
                count++;
            }

            Debug.Log($"<color=green>Applied random materials to {count} renderers matching '{_meshNameContains}'.</color>");
        }

        private void ApplyRandomMaterialToRenderer(Renderer renderer)
        {
            Material randomMat = _materialsToApply[Random.Range(0, _materialsToApply.Length)];
            Material[] mats = renderer.sharedMaterials;
            for (int i = 0; i < mats.Length; i++)
                mats[i] = randomMat;

            renderer.sharedMaterials = mats;
        }

        // -------------------- Renderer Settings --------------------

        [BoxGroup("Configure MeshRenderer Settings")]
        [LabelText("Cast Shadows")]
        [SerializeField] private ShadowCastingMode _castShadows = ShadowCastingMode.On;

        [BoxGroup("Configure MeshRenderer Settings")]
        [LabelText("Receive Global Illumination")]
        [SerializeField] private ReceiveGI _receiveGI = ReceiveGI.Lightmaps;

        [BoxGroup("Configure MeshRenderer Settings")]
        [LabelText("Contribute Global Illumination")]
        [SerializeField] private bool _contributeGI = true;

        [BoxGroup("Configure MeshRenderer Settings")]
        [LabelText("Light Probes")]
        [SerializeField] private LightProbeUsage _lightProbes = LightProbeUsage.BlendProbes;

        [BoxGroup("Configure MeshRenderer Settings")]
        [LabelText("Motion Vectors")]
        [SerializeField] private MotionVectorGenerationMode _motionVectors = MotionVectorGenerationMode.Object;

        [BoxGroup("Configure MeshRenderer Settings")]
        [LabelText("Dynamic Occlusion")]
        [SerializeField] private bool _dynamicOcclusion = true;

        [BoxGroup("Configure MeshRenderer Settings")]
        [Button(ButtonSizes.Large)]
        [GUIColor(1f, 0.85f, 0.3f)]
        private void ApplyRendererSettings()
        {
            if (_rootObject == null)
            {
                Debug.LogError("<color=red>Assign a Root Object.</color>");
                return;
            }

            MeshRenderer[] renderers = _rootObject.GetComponentsInChildren<MeshRenderer>(true);
            foreach (var renderer in renderers)
            {
                renderer.shadowCastingMode = _castShadows;
                renderer.receiveGI = _receiveGI;
                renderer.allowOcclusionWhenDynamic = _dynamicOcclusion;
                renderer.motionVectorGenerationMode = _motionVectors;
                renderer.lightProbeUsage = _lightProbes;
                renderer.receiveShadows = true;
                renderer.gameObject.isStatic = _contributeGI;
            }

            Debug.Log($"<color=green>Updated settings for {renderers.Length} MeshRenderers under '{_rootObject.name}'.</color>");
        }

        // -------------------- Add Collider --------------------

        [BoxGroup("Add Collider To Meshes")]
        [LabelText("Collider Type")]
        [SerializeField] private ColliderType _colliderType;

        [BoxGroup("Add Collider To Meshes")]
        [LabelText("Remove Existing Colliders First")]
        [SerializeField] private bool _removeExistingColliders = true;

        [BoxGroup("Add Collider To Meshes")]
        [Button(ButtonSizes.Large)]
        [GUIColor(1f, 0.5f, 0.5f)]
        private void AddCollidersToMeshes()
        {
            if (_rootObject == null)
            {
                Debug.LogError("<color=red>Assign a Root Object.</color>");
                return;
            }

            MeshFilter[] meshFilters = _rootObject.GetComponentsInChildren<MeshFilter>(true);
            int count = 0;

            foreach (MeshFilter meshFilter in meshFilters)
            {
                GameObject gameObject = meshFilter.gameObject;

                if (_removeExistingColliders)
                {
                    foreach (var collider in gameObject.GetComponents<Collider>())
                        DestroyImmediate(collider);
                }

                AddCollider(gameObject, meshFilter);
                count++;
            }

            Debug.Log($"<color=green>Added {_colliderType} colliders to {count} objects under '{_rootObject.name}'.</color>");
        }

        private void AddCollider(GameObject go, MeshFilter mf)
        {
            switch (_colliderType)
            {
                case ColliderType.Box:
                    go.AddComponent<BoxCollider>();
                    break;
                case ColliderType.Sphere:
                    go.AddComponent<SphereCollider>();
                    break;
                case ColliderType.Capsule:
                    go.AddComponent<CapsuleCollider>();
                    break;
                case ColliderType.Mesh:
                    var mc = go.AddComponent<MeshCollider>();
                    mc.sharedMesh = mf.sharedMesh;
                    mc.convex = false;
                    break;
            }
        }
    }

    public enum ColliderType
    {
        Box,
        Sphere,
        Capsule,
        Mesh
    }
}
