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
        foreach (var b in from x in gameObject.GetComponentsInChildren<MeshRenderer>() select x.bounds)
        {
            world.Encapsulate(b);
        };
        var d = Fuhrer.instance.createDrone(world.min + new Vector3(10, 5, 10));
        Sensor.scanOffset = new Vector3Int((int)System.Math.Floor(world.min.x),(int)System.Math.Floor(world.min.y+2),(int)System.Math.Floor(world.min.z));
        Sensor.scanSize = new Vector3Int(
            (int)System.Math.Ceiling(world.size.x),
            1,
            (int)System.Math.Ceiling(world.size.z));
    }
    public void OnClick() {
        Debug.Log("Minimap clicked!");
    }
}
