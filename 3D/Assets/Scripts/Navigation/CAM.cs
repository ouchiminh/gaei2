using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gaei.navi
{
    public class CAM : LocalPathProposer
    {
        public Vector3 getCourse(Vector3 dest, Vector3 here, in Dictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)> envmap)
        {
            throw new System.NotImplementedException();
        }
    }
}

