using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using gaei.navi;

public class TerrainInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Bounds world = default;
        int i = 0;
        foreach (var b in from x in gameObject.GetComponentsInChildren<MeshRenderer>() select x.bounds)
        {
            world.Encapsulate(b);
            Fuhrer.instance.addDemandPoint(new Area(b.max - new Vector3(1, 1, 1)));
            Fuhrer.instance.addSupplyPoint(new Area(b.max - new Vector3(1, 1, 1)));
        };
        Fuhrer.instance.createDrone(new Vector3(20,20,150));
        Sensor.scanOffset = new Vector3Int((int)System.Math.Floor(world.min.x),(int)System.Math.Floor(world.min.y),(int)System.Math.Floor(world.min.z));
        Sensor.scanSize = new Vector3Int(
            (int)System.Math.Ceiling(world.size.x),
            (int)System.Math.Ceiling(world.size.y),
            (int)System.Math.Ceiling(world.size.x));
    }
    private async void FixedUpdate()
    {
        //Sensor.scan(null, null);
    }
}
