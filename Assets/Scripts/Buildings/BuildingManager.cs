using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Buildings
{
    public class BuildingManager
    {
        private static readonly Dictionary<string, BuildingVisualData> BUILDINGS;
        private static readonly Dictionary<string, Sprite> BUILDING_ICONS;
        private static readonly MeshFilter PREVIEW_OBJECT;

        private static readonly Material MATERIAL;
    
        static BuildingManager()
        {
            BUILDINGS = new Dictionary<string, BuildingVisualData>();
            BUILDING_ICONS = new Dictionary<string, Sprite>();
            PREVIEW_OBJECT = Object.Instantiate(Resources.Load<MeshFilter>("Buildings/Place Preview"));
            MATERIAL = PREVIEW_OBJECT.GetComponent<MeshRenderer>().material;
        }
    
        public static GameObject GetPreview(Building building)
        {
            var id = building.identifier;
            if (!BUILDINGS.ContainsKey(id))
            {
                var mesh = building.prefab.GetComponent<MeshFilter>().sharedMesh;
                BUILDINGS.Add(id, new BuildingVisualData(mesh, mesh.subMeshCount, building.prefab.transform.localScale));
            }

            if (!PREVIEW_OBJECT.gameObject.activeSelf) PREVIEW_OBJECT.gameObject.SetActive(true);

            var buildingVisualData = BUILDINGS[id];
            var materials = new Material[buildingVisualData.SubMeshCount];
            Array.Fill(materials, MATERIAL);
            
            PREVIEW_OBJECT.mesh = buildingVisualData.Mesh;
            PREVIEW_OBJECT.GetComponent<MeshRenderer>().materials = materials;
            PREVIEW_OBJECT.transform.localScale = buildingVisualData.Scale;

            return PREVIEW_OBJECT.gameObject;
        }

        public static void DisablePreview()
        {
            if (PREVIEW_OBJECT.gameObject.activeSelf) PREVIEW_OBJECT.gameObject.SetActive(false);
        }

        public static Sprite GetIcon(Building building)
        {
            var id = building.identifier;
            if (!BUILDING_ICONS.ContainsKey(id)) 
            {
                BUILDING_ICONS.Add(id, SpriteCreator.Instance.Create(building.prefab));
            }

            return BUILDING_ICONS[id];
        }
        
        private struct BuildingVisualData
        {
            public readonly Mesh Mesh;
            public readonly int SubMeshCount;
            public readonly Vector3 Scale;

            public BuildingVisualData(Mesh mesh, int subMeshCount, Vector3 scale)
            {
                Mesh = mesh;
                SubMeshCount = subMeshCount;
                Scale = scale;
            }
        }
    }
}