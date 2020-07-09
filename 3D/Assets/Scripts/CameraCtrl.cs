using UnityEngine;
using System;

public class CameraCtrl : MonoBehaviour
{
    public GameObject attached;
    private float cameraOffset_ = 5;
    // Start is called before the first frame update
    // Update is called once per frame
    private void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        cameraRotate();
        if (attached == null)
        {
            freeCamera();
        }
        else attachedCamera();
    }
    void cameraRotate()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Cursor.visible = true;
        if (Input.GetKeyUp(KeyCode.LeftControl))
            Cursor.visible = false;
        if(!Cursor.visible) transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
    }
    void freeCamera()
    {
        var posdiff = default(Vector3);
        var forward = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Jump");
        posdiff += transform.right * horizontal *diffscale;
        posdiff += transform.forward * forward *diffscale;
        posdiff += vertical * new Vector3(0, diffscale, 0);
        gameObject.transform.position += posdiff;
    }
    void attachedCamera()
    {
        cameraOffset_ += -Input.GetAxis("Vertical") * diffscale;
        transform.position = attached.transform.position - transform.forward * cameraOffset_;
    }
    const float diffscale = 0.1f;
    float toRad(float angle)
    {
        return (float)Math.IEEERemainder(angle, 360) / 360 * 2 * (float)Math.PI;
    }
}
