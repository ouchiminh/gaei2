﻿using gaei.navi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gaei.navi {
    public interface GlobalPathProposer
    {
        public IEnumerable<Area> getPath(Vector3 dest, Vector3 here, Dictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)> envmap);
    }

    public interface LocalPathProposer
    {
        public Vector3 getCourse(Vector3 dest, Vector3 here, Dictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)> envmap);
    }
}
