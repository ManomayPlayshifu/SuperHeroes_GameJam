using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    GameObject cam;
    private void Start()
    {
        cam = Camera.main.gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!NetworkClient.active)
            return;

        if(cam!=null)
        transform.LookAt(Camera.main.gameObject.transform);
        
    }
}
