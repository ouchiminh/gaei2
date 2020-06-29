using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gaei.navi;

public class AddDroneButton : MonoBehaviour
{   
    public void OnClick(){
        Debug.Log("clicked");
        Fuhrer.instance.createDrone();
    }
}
