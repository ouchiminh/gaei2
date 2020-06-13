using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var c = gameObject.GetComponent<TerrainCollider>();
        gaei.navi.Sensor.scanOffset = new Vector3Int((int)c.bounds.min.x,(int)c.bounds.min.y,(int)c.bounds.min.z);
        gaei.navi.Sensor.scanSize = new Vector3Int((int)c.bounds.size.x,(int)c.bounds.size.y,(int)c.bounds.size.z);

    }
}
