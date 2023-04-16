using Buildings;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingButton : MonoBehaviour
    {
        [SerializeField] private Building building;
        [SerializeField] private Image image;
        
        private void Start()
        {
            image.sprite = BuildingManager.GetIcon(building);
        }

        public void Select()
        {
            EventBus.PlaceBuilding(building);
        }
    }
}