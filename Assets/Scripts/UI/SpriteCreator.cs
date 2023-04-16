using UnityEngine;

namespace UI
{
    public class SpriteCreator : MonoBehaviour
    {
        public static SpriteCreator Instance { get; private set; }

        [SerializeField] private Camera cam;
        [SerializeField] private Transform marker;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public Sprite Create(GameObject go)
        {
            var renderer = Instantiate(go, marker).GetComponent<Renderer>();
            var model = renderer.bounds;
            var renderTexture = new RenderTexture(512, 512, 24);

            cam.targetTexture = renderTexture;
            cam.clearFlags = CameraClearFlags.Depth;
            cam.enabled = true;
            cam.transform.LookAt(renderer.transform);

            // Calculate the distance to the object based on the bounds size
            var distance = (model.size.y / 2.0f) / Mathf.Tan(Mathf.Deg2Rad * cam.fieldOfView / 2.0f);
            
            // Move the camera to the correct distance
            var camTransform = cam.transform;
            camTransform.position = model.center - distance * camTransform.forward * 6f;

            // Adjust the field of view to fit the object
            var newFOV = Mathf.Atan((model.size.y / 2.0f) / distance) * 2.0f * Mathf.Rad2Deg;
            cam.fieldOfView = newFOV;
            
            var icon = GetIcon(renderTexture);
            cam.enabled = false;
            DestroyImmediate(renderer.gameObject);

            return icon;
        }

        private Sprite GetIcon(RenderTexture renderTexture)
        {
            var resX = cam.pixelWidth;
            var resY = cam.pixelHeight;

            var clipX = 0;
            var clipY = 0;

            if (resX > resY)
            {
                clipX = resX - resY;
            } else if (resY > resX)
            {
                clipY = resY - resX;
            }

            var texture = new Texture2D(resX - clipX, resY - clipY, TextureFormat.RGBA32, false);
            cam.Render();
            Graphics.CopyTexture(renderTexture, texture);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }
}