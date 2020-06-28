using UnityEngine;
using System;

public class CameraCtrl : MonoBehaviour
{
    GameObject attatched_;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if(attatched_ == null)
        {
            cameraRotate();
            freeCamera();
        }
    }
    void cameraRotate()
    {
        transform.eulerAngles += new Vector3(-diffscale*Input.GetAxis("MouseY"), diffscale*Input.GetAxis("MouseX"), 0);
    }
    void freeCamera()
    {
        var posdiff = default(Vector3);
        var eulerangle = transform.rotation.eulerAngles;
        var sight = new Vector3((float)Math.Sin(toRad(eulerangle.x)), (float)Math.Sin(toRad(eulerangle.y)), (float)Math.Sin(toRad(eulerangle.z)));
        var forward = Input.GetAxis("Forward");
        var horizontal = Input.GetAxis("Right");
        var vertical = Input.GetAxis("Vertical");
        posdiff += transform.right * horizontal *diffscale;
        posdiff += transform.forward * forward *diffscale;
        posdiff += vertical * new Vector3(0, diffscale, 0);
        gameObject.transform.position += posdiff;
    }
    const float diffscale = 0.1f;
    float toRad(float angle)
    {
        return (float)Math.IEEERemainder(angle, 360) / 360 * 2 * (float)Math.PI;
    }
}
