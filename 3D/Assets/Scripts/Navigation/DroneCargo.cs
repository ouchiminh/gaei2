using gaei.navi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCargo : MonoBehaviour
{
    public DroneCtrl dc_;
    public MeshRenderer mr_;
    // Start is called before the first frame update
    void Start()
    {
        mr_.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mr_.enabled = (dc_.status == DroneCtrl.Status.delivery);
    }
}
