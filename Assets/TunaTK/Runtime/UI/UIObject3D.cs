// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 17/01/2020 09:17 AM
// Created On: CHRONOS

using Sirenix.OdinInspector;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using TunaTK.Utility;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TunaTK.UI
{
    [RequireComponent(typeof(Image), typeof(RectTransform))]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class UIObject3D : InitialiseBehaviour
    {
        [Serializable]
        public class RotateSettings
        {
            public bool x;
            public bool y;
            public bool z;
            public float speed;
        }

        [FormerlySerializedAs("Target")] public GameObject target;
        public bool alwaysRender = true;

        [Header("Object Settings")]
        [SerializeField, Range(-180, 180)] private float xRotation = 45;
        [SerializeField, Range(-180, 180)] private float yRotation = 45;
        [SerializeField, Range(-180, 180)] private float zRotation;

        [SerializeField] private bool rotateOnSpot;
        
        [SerializeField, ShowIf("rotateOnSpot")] private RotateSettings rotateSettings = new RotateSettings();

        [Header("Rendering Settings")]
        [SerializeField] private Color backgroundColor = Color.white;
        [SerializeField] private Vector2Int textureSize = new Vector2Int(-1, -1);

        private RenderTexture renderTexture;
        private Texture2D texture;
        private Sprite sprite;

        private Image image;
        private GameObject targetHolder;
        private new Camera camera;

        private const string LogTag = "[UIObject3D]";

        private Vector3 actualRot = Vector3.zero;
        private int objectLayer = -1;

        private Transform Scene
        {
            get
            {
                GameObject scenesHolderObj = GameObject.Find("UI 3D Scenes");
                scenesHolderObj ??= new GameObject("UI 3D Scenes");

                Transform scenesHolder = scenesHolderObj.transform;
                string sceneName = $"{target.name} Scene";

                Transform parent = null;
                foreach(Transform child in scenesHolder)
                {
                    if(child.name != sceneName)
                        continue;

                    parent = child;
                    break;
                }

                parent ??= CreateObjectWithParent(sceneName, scenesHolder).transform;

                return parent;
            }
        }

        private int ObjectLayer
        {
            get
            {
                if(objectLayer != -1)
                    return objectLayer;

                objectLayer = LayerMask.NameToLayer("UIObject3D");

            #if UNITY_EDITOR
                if(objectLayer != -1)
                    return objectLayer;

                ManageLayer();
                objectLayer = LayerMask.NameToLayer("UIObject3D");
            #endif

                return objectLayer;
            }
        }

        private int TextureWidth => textureSize.x > 0 ? textureSize.x : (int)image.rectTransform.rect.width;
        private int TextureHeight => textureSize.y > 0 ? textureSize.y : (int)image.rectTransform.rect.height;

    #if UNITY_EDITOR

        [InitializeOnLoadMethod]
        public static void ManageLayer()
        {
            int layer = LayerMask.NameToLayer("UIObject3D");
            // layer already exists; nothing to do here
            if(layer != -1) 
                return;

            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset"));
            SerializedProperty layers = tagManager.FindProperty("layers");

            if(layers == null || !layers.isArray)
            {
                Debug.LogWarning($"{LogTag} Unable to set up layers. You can resolve this issue by manually adding a layer named 'UIObject3D'.");
                return;
            }

            bool set = false;

            // start off at 8 - layers 1 - 7 cannot be set here
            for(int i = 8; i < layers.arraySize; i++)
            {
                SerializedProperty element = layers.GetArrayElementAtIndex(i);

                if(element.stringValue != "") 
                    continue;
                
                element.stringValue = "UIObject3D";
                set = true;

                // we're done
                break;
            }

            if(set)
            {
                Debug.Log($"{LogTag} Layer 'UIObject3D' created.");
                tagManager.ApplyModifiedProperties();
            }
            else
            {
                Debug.LogWarning($"{LogTag} Unable to create Layer 'UIObject3D' - no blank layers found to replace! Please create the layer 'UIObject3d' manually in order to continue.");
            }
        }

    #endif

        //@cond
        // Use this for initialization
        protected override Task OnInitialisation(params object[] _params)
        {
            image = gameObject.GetComponent<Image>();

            actualRot.x = xRotation;
            actualRot.y = yRotation;
            actualRot.z = zRotation;

            CreateSetup();
            
            return Task.CompletedTask;
        }

        // Called once per frame once initialised
        protected override void InitialisedUpdate()
        {
            UpdateRotation();

            if(targetHolder != null)
                targetHolder.transform.localRotation = Quaternion.Euler(actualRot);

            if(alwaysRender)
                Render();
        }
        //@endcond

        /// <summary> Updates the visual rotation of the object in the scene by rotating the actual object. </summary>
        private void UpdateRotation()
        {
            if(!rotateOnSpot) 
                return;
            
            if(rotateSettings.x)
                actualRot.x += rotateSettings.speed * Time.deltaTime;

            if(rotateSettings.y)
                actualRot.y += rotateSettings.speed * Time.deltaTime;

            if(rotateSettings.z)
                actualRot.z += rotateSettings.speed * Time.deltaTime;
        }

        /// <summary> Generates the render texture of the camera for the object by forcing the camera to render a frame. </summary>
        public void Render()
        {
            if(camera == null)
                return;

            Rect rect = new Rect(0, 0, TextureWidth, TextureHeight);

            RenderTexture.active = renderTexture;

            // If we don't manually clear the buffer, we end up with a copy of the target in the background
            GL.Clear(false, true, backgroundColor);

            camera.backgroundColor = backgroundColor;
            camera.Render();

            texture.ReadPixels(rect, 0, 0);
            texture.Apply();

            RenderTexture.active = null;
        }

        /// <summary> Creates the sprite if it doesn't already exist, but if it does, just return it. </summary>
        private Sprite GetSprite()
        {
            sprite ??= Sprite.Create(texture, new Rect(0, 0, TextureWidth, TextureHeight), new Vector2(0.5f, 0.5f));
            sprite.name = $"{target.name} Sprite";

            return sprite;
        }

        /// <summary> Creates the texture if it doesn't already exist, but if it does, just return it. </summary>
        private Texture2D GetTexture()
        {
            texture ??= new Texture2D(TextureWidth, TextureHeight, TextureFormat.ARGB32, false, false)
            {
                name = $"{target.name} Texture"
            };

            return texture;
        }

        /// <summary> Creates the default setup of a 3D UI scene including the render texture, the camera and the scene object itself. </summary>
        private void CreateSetup()
        {
            ClearChildren(Scene);

            renderTexture = new RenderTexture(TextureWidth, TextureHeight, 16, RenderTextureFormat.ARGB32)
            {
                name = $"{target.name} Render Texture"
            };
            renderTexture.Create();

            image.sprite = sprite;

            // Create camera
            GameObject targetCamera = CreateObjectWithParent("Target Camera", Scene);
            targetCamera.layer = ObjectLayer;
            targetCamera.transform.localPosition = new Vector3(0, 0, -2);
            camera = targetCamera.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.Color;
            camera.backgroundColor = backgroundColor;
            camera.cullingMask &= (1 << ObjectLayer);
            camera.targetTexture = renderTexture;

            // Create target obj
            targetHolder = CreateObjectWithParent("Target Holder", Scene);
            targetHolder.layer = ObjectLayer;
            targetHolder.transform.localPosition = Vector3.zero;
            targetHolder.transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation);

            // Create target instance
            GameObject targetInst = CreateObjectWithParent(target.name, targetHolder.transform);
            targetInst.layer = ObjectLayer;

            MeshRenderer rend = targetInst.AddComponent<MeshRenderer>();
            rend.material = target.GetComponent<MeshRenderer>().sharedMaterial;

            MeshFilter filter = targetInst.AddComponent<MeshFilter>();
            filter.mesh = target.GetComponent<MeshFilter>().sharedMesh;
        }

        /// <summary> Creates a new GameObject with the passed name, making it a child of the passed <paramref name="_parent"/>.</summary>
        /// <param name="_name">The name the object will be created with.</param>
        /// <param name="_parent">The object to be made the new one's parent.</param>
        private static GameObject CreateObjectWithParent(string _name, Transform _parent)
        {
            GameObject newObj = new GameObject(_name);
            newObj.transform.parent = _parent;
            newObj.transform.Reset();

            return newObj;
        }

        /// <summary> Destroys all children GameObjects of a specific passed Transform. </summary>
        /// <param name="_transform">The parent object to destroy the children of.</param>
        private static void ClearChildren(Transform _transform)
        {
            foreach(Transform child in _transform)
                Destroy(child.gameObject);
        }
    }
}