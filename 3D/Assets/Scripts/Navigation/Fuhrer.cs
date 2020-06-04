using gaei.navi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace gaei.navi
{
    public class Fuhrer
    {
        public void addDemandPoint(Area area) {
            demandPoints_.Add(area);
            assignDrone();
        }
        public void addSupplyPoint(Area area) {
            supplyPoints_.Add(area);
            assignDrone();
        }
        public void updateDroneState(DroneCtrl drone)
        {
            assignDrone(drone);
        }
        public void createDrone() { }

        private void assignDrone(DroneCtrl drone = null) {
            // TODO:droneがnullならば暇なドローンを探して需要点を割り当て
        }

        private List<DroneCtrl> drones_ = new List<DroneCtrl>();
        private List<Area> demandPoints_ = new List<Area>();
        private List<Area> supplyPoints_ = new List<Area>();
    }
}
