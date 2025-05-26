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
        private static void OpenWindow()
        {
            GetWindow<PrefabSetupEditorWindow>().Show();
        }

        [BoxGroup("Root Object")] 
        [LabelText("Target Prefab or Scene Object")]
        [SerializeField]
        private GameObject _rootObject;
        
        [BoxGroup("Set Material To Children")] 
        [LabelText("Target Material")]
        [SerializeField]
        private Material _targetMaterial;

        [BoxGroup("Set Material To Children")] 
        [Space] 
        [LabelText("Override All Material Slots")]
        [SerializeField]
        private bool _overrideAllSlots = true;

        [FormerlySerializedAs("TargetMaterialIndex")]
        [BoxGroup("Set Material To Children")]
        [ShowIf("@!_overrideAllSlots")]
        [LabelText("Target Material Slot Index")]
        [MinValue(0)]
        [SerializeField]
        private int _targetMaterialIndex = 0;

        [BoxGroup("Set Material To Children")]
        [Button(ButtonSizes.Large)]
        [GUIColor(0.3f, 0.8f, 1f)]
        private void ApplyMaterialToChildren()
        {
            if (_targetMaterial == null || _rootObject == null)
            {
                Debug.LogError("<color=red>Please assign both a material and a root object.</color>");
                return;
            }

            int count = 0;
            MeshRenderer[] meshRenderers = _rootObject.GetComponentsInChildren<MeshRenderer>(true);
            SkinnedMeshRenderer[] skinnedRenderers = _rootObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);

            foreach (Renderer renderer in meshRenderers.Cast<Renderer>().Concat(skinnedRenderers))
            {
                Material[] mats = renderer.sharedMaterials;

                if (_overrideAllSlots)
                {
                    for (int i = 0; i < mats.Length; i++)
                    {
                        mats[i] = _targetMaterial;
                    }
                }
                else if (_targetMaterialIndex < mats.Length)
                {
                    mats[_targetMaterialIndex] = _targetMaterial;
                }

                renderer.sharedMaterials = mats;
                count++;
            }

            Debug.Log($"<color=green>Replaced materials in {count} renderers under '{_rootObject.name}'.</color>");
        }

        [BoxGroup("Set Random Material for Matching Mesh")]
        [LabelText("Mesh Name Contains")]
        [SerializeField]
        private string _meshNameContains;
        
        [BoxGroup("Set Random Material for Matching Mesh")]
        [LabelText("Possible Materials")]
        [SerializeField]
        public Material[] _materialsToApply;
        
        [BoxGroup("Set Random Material for Matching Mesh")]
        [Button(ButtonSizes.Large)]
        [GUIColor(0.6f, 1f, 0.6f)]
        private void ApplyRandomMaterialToMatchingMeshNames()
        {
            if (string.IsNullOrWhiteSpace(_meshNameContains) || _rootObject == null || 
                _materialsToApply == null || _materialsToApply.Length == 0)
            {
                Debug.LogError("<color=red>Please assign a mesh name, root object, and at least one material.</color>");
                return;
            }

            int count = 0;
            MeshFilter[] meshFilters = _rootObject.GetComponentsInChildren<MeshFilter>(true);
            SkinnedMeshRenderer[] skinnedMeshes = _rootObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);

            foreach (MeshFilter meshFilter in meshFilters)
            {
                if (meshFilter.sharedMesh == null || !meshFilter.sharedMesh.name.Contains(_meshNameContains)) 
                    continue;
                
                Renderer renderer = meshFilter.GetComponent<Renderer>();
                if (renderer == null) 
                    continue;
                
                ApplyRandomMaterialToRenderer(renderer);
                count++;
            }

            foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshes)
            {
                if (skinnedMeshRenderer.sharedMesh == null || 
                    !skinnedMeshRenderer.sharedMesh.name.Contains(_meshNameContains)) 
                    continue;
                
                ApplyRandomMaterialToRenderer(skinnedMeshRenderer);
                count++;
            }

            Debug.Log($"<color=green>Replaced materials in {count} renderers where mesh name contains '{_meshNameContains}'.</color>");
        }

        private void ApplyRandomMaterialToRenderer(Renderer renderer)
        {
            Material randomMaterial = _materialsToApply[Random.Range(0, _materialsToApply.Length)];
            Material[] materials = renderer.sharedMaterials;

            for (int i = 0; i < materials.Length; i++)
                materials[i] = randomMaterial;

            renderer.sharedMaterials = materials;
        }

        [BoxGroup("Configure MeshRenderer Settings")]
        [LabelText("Cast Shadows")]
        [SerializeField]
        private ShadowCastingMode _castShadows = ShadowCastingMode.On;

        [BoxGroup("Configure MeshRenderer Settings")]
        [LabelText("Receive Global Illumination")]
        [SerializeField]
        private ReceiveGI _receiveGI = ReceiveGI.Lightmaps;

        [BoxGroup("Configure MeshRenderer Settings")]
        [LabelText("Contribute Global Illumination")]
        [SerializeField]
        private bool _contributeGI = true;

        [BoxGroup("Configure MeshRenderer Settings")]
        [LabelText("Light Probes")]
        [SerializeField]
        private LightProbeUsage _lightProbes = LightProbeUsage.BlendProbes;

        [BoxGroup("Configure MeshRenderer Settings")]
        [LabelText("Motion Vectors")]
        [SerializeField]
        private MotionVectorGenerationMode _motionVectors = MotionVectorGenerationMode.Object;

        [BoxGroup("Configure MeshRenderer Settings")]
        [LabelText("Dynamic Occlusion")]
        [SerializeField]
        private bool _dynamicOcclusion = true;
        
        [BoxGroup("Configure MeshRenderer Settings")]
        [Button(ButtonSizes.Large)]
        [GUIColor(1f, 0.85f, 0.3f)]
        private void ApplyRendererSettings()
        {
            if (_rootObject == null)
            {
                Debug.LogError("<color=red>Assign a root object for MeshRenderer settings.</color>");
                return;
            }

            MeshRenderer[] renderers = _rootObject.GetComponentsInChildren<MeshRenderer>(true);
            int count = 0;

            foreach (MeshRenderer renderer in renderers)
            {
                renderer.shadowCastingMode = _castShadows;
                renderer.receiveGI = _receiveGI;
                renderer.allowOcclusionWhenDynamic = _dynamicOcclusion;
                renderer.motionVectorGenerationMode = _motionVectors;
                renderer.lightProbeUsage = _lightProbes;
                renderer.receiveShadows = true;
                renderer.gameObject.isStatic = _contributeGI;

                count++;
            }

            Debug.Log($"<color=green>Applied MeshRenderer settings to {count} objects under '{_rootObject.name}'.</color>");
        }
        
        [BoxGroup("Add Collider To Meshes")]
        [LabelText("Collider Type")]
        [SerializeField]
        private ColliderType _colliderType;

        [BoxGroup("Add Collider To Meshes")]
        [LabelText("Remove Existing Colliders First")]
        [SerializeField]
        private bool _removeExistingColliders = true;

        [BoxGroup("Add Collider To Meshes")]
        [Button(ButtonSizes.Large)]
        [GUIColor(1f, 0.5f, 0.5f)]
        private void AddCollidersToMeshes()
        {
            if (_rootObject == null)
            {
                Debug.LogError("<color=red>Assign a root object first.</color>");
                return;
            }

            int count = 0;
            MeshFilter[] meshFilters = _rootObject.GetComponentsInChildren<MeshFilter>(true);

            foreach (MeshFilter meshFilter in meshFilters)
            {
                GameObject gameObject = meshFilter.gameObject;

                if (_removeExistingColliders)
                {
                    Collider[] colliders = gameObject.GetComponents<Collider>();
                    foreach (Collider collider in colliders)
                    {
                        DestroyImmediate(collider);   
                    }
                }

                switch (_colliderType)
                {
                    case ColliderType.Box:
                        gameObject.AddComponent<BoxCollider>();
                        break;
                    case ColliderType.Sphere:
                        gameObject.AddComponent<SphereCollider>();
                        break;
                    case ColliderType.Capsule:
                        gameObject.AddComponent<CapsuleCollider>();
                        break;
                    case ColliderType.Mesh:
                        var mc = gameObject.AddComponent<MeshCollider>();
                        mc.sharedMesh = meshFilter.sharedMesh;
                        mc.convex = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                count++;
            }

            Debug.Log($"<color=green>Added {_colliderType} to {count} mesh objects under '{_rootObject.name}'.</color>");
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