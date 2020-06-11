using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gaei.navi {

    public class Pilot : GlobalPathProposer
    {
        public IEnumerable<Area> getPath(Vector3 dest, Vector3 here, Dictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)> envmap)
        {
            throw new System.NotImplementedException();
        }
    }
}

