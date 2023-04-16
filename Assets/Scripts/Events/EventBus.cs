using System;
using Buildings;

namespace Events
{
    public class EventBus
    {
        public static event Action<Building> OnSelectBuildingToPlace;

        public static void PlaceBuilding(Building obj)
        {
            OnSelectBuildingToPlace?.Invoke(obj);
        }
    }
}