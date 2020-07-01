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
            supplyPoints_.Add(new Area(8, 4, 22));
            supplyPoints_.Add(new Area(-59, 4, -31));
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
            if (drone.status == DroneCtrl.Status.homing && supplyPoints.Count > 0)
            {
                Area drone_area = new Area(drone.transform.position);
                var res = supplyPoints_.FindMin(x => Area.distance(x, drone_area)); 
                drone.setDestination(res);
            }
            else if (drone.status == DroneCtrl.Status.idle)
            {
                assignDrone(drone);
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
            if (drone == null)
            {
                var res = (from x in drones_ where x.status == DroneCtrl.Status.idle select x).FindMin(x => Area.distance(new Area(x.transform.position), demandPoints.Last()));
                res.setDestination(demandPoints.Last());
                demandPoints_.RemoveLast();
            }
            else if(demandPoints.Count > 0)
            {
                Area d_area = new Area(drone.transform.position);
                var res = demandPoints_.FindMin(x => Area.distance(x, d_area));
                drone.setDestination(res);
                demandPoints_.Remove(res);
            }
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
