using gaei.navi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace gaei.navi
{
    public class Fuhrer : MonoBehaviour
    {
        private GameObject drone_prefab_;
        public static Fuhrer instance { get; private set; }
        public Fuhrer()
        {
            drones_ = new List<DroneCtrl>();
            demandPoints_ = new LinkedList<Area>();
            supplyPoints_ = new List<Area>();
            instance = this;
        }
        private void Start()
        {
            drone_prefab_ = Resources.Load("drone 1")as GameObject;
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
        public DroneCtrl createDrone(Vector3? pos = null) {
            drones_.Add((Object.Instantiate(drone_prefab_, pos ?? supplyPoints_.First().center, default(Quaternion)) as GameObject).GetComponent<DroneCtrl>());
            return drones_.Last();
        }

        private void assignDrone(DroneCtrl drone = null) {
            // TODO:droneがnullならば暇なドローンを探して需要点を割り当て
        }
        public IReadOnlyList<DroneCtrl> drones { get => drones_; }
        private List<DroneCtrl> drones_ = new List<DroneCtrl>();
        private IReadOnlyCollection<Area> demandPoints { get => demandPoints_; }
        private LinkedList<Area> demandPoints_ = new LinkedList<Area>();
        public IReadOnlyList<Area> supplyPoints { get => supplyPoints_; }
        private List<Area> supplyPoints_ = new List<Area>();
    }
}
