using gaei.navi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace gaei.navi
{
    public class Fuhrer
    {
        static Fuhrer instance_;
        public static Fuhrer instance { get {
                return instance_ = instance_ ?? new Fuhrer();
            }
        }
        public void addDemandPoint(Area area) {
            demandPoints_.AddLast(area);
            assignDrone();
        }
        public void addSupplyPoint(Area area) {
            supplyPoints_.Add(area);
        }
        public void updateDroneState(DroneCtrl drone)
        {
        }
        public void deactivateDemandPoint(Area area)
        {
            demandPoints_.Remove(area);
        }
        public void createDrone() {
        }

        private void assignDrone(DroneCtrl drone = null) {
            // TODO:droneがnullならば暇なドローンを探して需要点を割り当て
        }

        private List<DroneCtrl> drones_ = new List<DroneCtrl>();
        private LinkedList<Area> demandPoints_ = new LinkedList<Area>();
        private List<Area> supplyPoints_ = new List<Area>();
    }
}
