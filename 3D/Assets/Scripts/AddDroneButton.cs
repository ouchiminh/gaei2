using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gaei.navi;

public class AddDroneButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(){
        Debug.Log ("clicked");
        Fuhrer.instance.createDrone();
    }
}
