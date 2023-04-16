using Events;
using UnityEngine;

namespace Buildings
{
    public class ObjectPlacer : MonoBehaviour
    {
        private static readonly Color CAN_PLACE = new(0.06f, 0.89f, 1f, 0.6f);
        private static readonly Color CAN_NOT_PLACE = new(0.91f, 0.13f, 0.16f, 0.6f);

        [SerializeField] private Building toPlace;
        [SerializeField] private Terrain terrain;
        [SerializeField] private float maximumPlaceAngle;

        private Camera _camera;
        private bool _validBuilding;
        private Transform _preview;
        private MeshRenderer _previewRenderer;
        private bool _couldPlace;
        private static readonly int COLOR = Shader.PropertyToID("_Color");

        private void Start()
        {
            _camera = GetComponent<Camera>();
            UpdatedBuilding();
        }

        private void OnEnable()
        {
            EventBus.OnSelectBuildingToPlace += SetPlacing;
        }

        private void OnDisable()
        {
            EventBus.OnSelectBuildingToPlace -= SetPlacing;
        }

        private void Update()
        {
            if (!_validBuilding) return;
        
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, int.MaxValue, LayerMask.GetMask("Placement Obstacle"))) return;
        
            _preview.position = hit.point;
            
            // Calculate the normal vector of the terrain at the hit point
            var terrainData = terrain.terrainData;
            var normal= hit.collider.gameObject.CompareTag("Terrain") ? terrain.terrainData.GetInterpolatedNormal(hit.point.x / terrainData.size.x, hit.point.z / terrainData.size.z) : hit.normal;

            // Calculate the rotation required to align the object with the terrain
            var rotation = Quaternion.FromToRotation(_preview.up, normal) * _preview.rotation;
            _preview.rotation = rotation;
            
            var canPlace = rotation.eulerAngles.x + rotation.eulerAngles.z <= maximumPlaceAngle;
            if (_couldPlace != canPlace)
            {
                foreach (var material in _previewRenderer.materials)
                {
                    material.SetColor(COLOR, canPlace ? CAN_PLACE : CAN_NOT_PLACE);
                }
            }
            
            if (canPlace && Input.GetMouseButtonDown(0))
            {
                Instantiate(toPlace.prefab, _preview.position, _preview.rotation);
            }
            
            _couldPlace = canPlace;
        }

        private void SetPlacing(Building building)
        {
            toPlace = building;
            UpdatedBuilding();
        }

        private void UpdatedBuilding()
        {
            _validBuilding = toPlace != null;
            _preview = _validBuilding ? BuildingManager.GetPreview(toPlace).transform : null;
            _previewRenderer = _preview != null ? _preview.GetComponent<MeshRenderer>() : null;
            if (!_validBuilding)
            {
                BuildingManager.DisablePreview();
            }
        }
    }
}