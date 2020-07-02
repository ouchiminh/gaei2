using gaei.navi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowDroneToggleButton : MonoBehaviour
{
    public GameObject dropdownObject;
    public GameObject mainCamera;
    private Dropdown dropdown_;
    private CameraCtrl camera_;
    private Toggle toggle_;
    private void Start()
    {
        dropdown_ = dropdownObject.GetComponent<Dropdown>();
        camera_ = mainCamera.GetComponent<CameraCtrl>();
        toggle_ = gameObject.GetComponent<Toggle>();
    }
    public void onValueChange()
    {
        var value = gameObject.GetComponent<Toggle>().isOn;
        Debug.Log("toggle " + value.ToString() + dropdown_.value.ToString());
        if (value && dropdown_.value < Fuhrer.instance.drones.Count)
            camera_.attached = Fuhrer.instance.drones[dropdown_.value].gameObject;
        else
            camera_.attached = null;
    }
}
