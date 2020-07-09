using UnityEngine;
using System.Collections;

public class Wingrotation : MonoBehaviour
{

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 5));
    }
}
