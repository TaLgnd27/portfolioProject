using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 target;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        target.z = transform.position.z;
        if(Vector3.Distance(transform.position, target) > 0.005){
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, target, 0.10f);
        } else
        {
            transform.position = target;
        }
    }
}
