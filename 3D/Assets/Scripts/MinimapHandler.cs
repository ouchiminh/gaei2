using gaei.navi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapHandler : MonoBehaviour, IPointerClickHandler
{
    public GameObject minimapCameraObject;
    private Camera camera_;

    void Start()
    {
        camera_ = minimapCameraObject.GetComponent<Camera>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData.pressPosition);
        var worldCoord = camera_.ViewportToWorldPoint(eventData.pressPosition / 200.0f);
        worldCoord.y = 50;
        //if(!Physics.BoxCast(worldCoord, Vector3.one * 0.5f, new Vector3(0, -1, 0),out RaycastHit hit, default, 100, LayerMask.GetMask("Earth")))
        //    return;
        if (!Physics.Raycast(worldCoord, new Vector3(0, -1, 0),out RaycastHit hit, 100, LayerMask.GetMask("Earth"))) 
            return;
        Debug.Log(hit.point);
        Fuhrer.instance.addDemandPoint(new Area(hit.point));
    }

}
