using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gaei.navi {
    using EnvMap = System.Collections.Generic.Dictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)>;
    public class Navigator : MonoBehaviour
    {
        GlobalPathProposer globalPathProposer_;
        LocalPathProposer localPathProposer_;
        List<Area> path_;
        void Start()
        {
            globalPathProposer_ = new Pilot();
            localPathProposer_ = new CAM();
        }
        public Vector3 getNextCourse(in EnvMap envmap) {

        }
        public void setDestination(Area dest) {
            path_ = globalPathProposer_.getPath()
        }
    }
}
