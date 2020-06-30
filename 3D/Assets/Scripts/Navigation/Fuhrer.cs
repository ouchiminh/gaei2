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
        private int deactivateFlag;
        public static Fuhrer instance { get; private set; }
        public Fuhrer()
        {
            drones_ = new List<DroneCtrl>();
            demandPoints_ = new LinkedList<Area>();
            supplyPoints_ = new List<Area>();
            instance = this;
            deactivateFlag = 0;
        }
        private void Start()
        {
            drone_prefab_ = Resources.Load("drone 1")as GameObject;
            supplyPoints_.Add(new Area(62, 4, -20));
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
            if (drone.status == DroneCtrl.Status.homing)
            {
                Area drone_area = new Area(drone.transform.position);
                int min = Area.distance(drone_area, supplyPoints_[0]);
                int index = 0;
                for(int i = 0; i < supplyPoints_.Count; ++i)
                {
                    int tmp = Area.distance(drone_area, supplyPoints_[i]);
                    if (min > tmp)
                    {
                        min = tmp;
                        index = i;
                    }
                }
                drone.setDestination(supplyPoints_[index]);
            }
            else if (drone.status == DroneCtrl.Status.idle)
            {

            }
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
        public void raiseDroneDeactivateFlag()
        {
            deactivateFlag++;
            SearchAndDestroy();
        }
        public void SearchAndDestroy()
        {
            for(int i=0;i<drones_.Count && deactivateFlag > 0; ++i)
            {
                if (drones_[i].status == DroneCtrl.Status.idle)
                {
                    UnityEngine.Object.Destroy(drones_[i].gameObject);
                    drones_.RemoveAt(i--);
                    --deactivateFlag;
                }
            }
        }
        public IReadOnlyList<DroneCtrl> drones { get => drones_; }
        private List<DroneCtrl> drones_ = new List<DroneCtrl>();
        private IReadOnlyCollection<Area> demandPoints { get => demandPoints_; }
        private LinkedList<Area> demandPoints_ = new LinkedList<Area>();
        public IReadOnlyList<Area> supplyPoints { get => supplyPoints_; }
        private List<Area> supplyPoints_ = new List<Area>();
    }
}
