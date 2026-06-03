using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    private Transform mainCameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        if(Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(mainCameraTransform != null)
        {
            transform.rotation = mainCameraTransform.rotation;
        }
    }
}
