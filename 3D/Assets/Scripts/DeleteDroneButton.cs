using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gaei.navi;

public class DeleteDroneButton : MonoBehaviour
{
    public void OnClick(){
        Debug.Log("clicked(delete)");
        Fuhrer.instance.raiseDroneDeactivateFlag();
    }
}
