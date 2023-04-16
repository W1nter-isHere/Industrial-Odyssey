using UnityEngine;

namespace Buildings
{
    [CreateAssetMenu(fileName = "New Building", menuName = "Industrial Odyssey/New Building", order = 0)]
    public class Building : ScriptableObject
    {
        public string identifier;
        public GameObject prefab;
    }
}