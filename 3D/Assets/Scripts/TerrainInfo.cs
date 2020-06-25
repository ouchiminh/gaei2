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
            if((i++ & 0x100) > 0)Fuhrer.instance.addDemandPoint(new Area(b.max + new Vector3(1, 1, 1)));
            if((i++ & 0x200) > 0)Fuhrer.instance.addSupplyPoint(new Area(b.max + new Vector3(1, 1, 1)));
        };
        Fuhrer.instance.createDrone();
        Sensor.scanOffset = new Vector3Int((int)System.Math.Floor(world.min.x),(int)System.Math.Floor(world.min.y),(int)System.Math.Floor(world.min.z));
        Sensor.scanSize = new Vector3Int(
            (int)System.Math.Ceiling(world.size.x)/30,
            (int)System.Math.Ceiling(world.size.y)/3,
            (int)System.Math.Ceiling(world.size.x)/30);
    }
    private async void FixedUpdate()
    {
        Sensor.scan(null,null);
    }
}
