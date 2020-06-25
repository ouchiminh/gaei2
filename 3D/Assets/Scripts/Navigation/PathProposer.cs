using gaei.navi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gaei.navi {
    using ReadOnlyEnvMap = IReadOnlyDictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)>;
    public interface GlobalPathProposer
    {
        IEnumerable<Area> getPath(Vector3 dest, Vector3 here, in ReadOnlyEnvMap envmap);
    }

    public interface LocalPathProposer
    {
        Vector3 getCourse(Vector3 dest, Vector3 here, in ReadOnlyEnvMap envmap);
    }
}
