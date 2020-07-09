using gaei.navi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DroneList : Dropdown
{
    protected override void Start()
    {
        base.Start();
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        options.Clear();
        for (int i = 0; i < Fuhrer.instance.drones.Count; ++i)
            options.Add(new OptionData(i.ToString()));
        Debug.Log(options.Count);

        base.OnPointerClick(eventData);
    }
}
