using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(!NetworkClient.active)
            return;
        
        transform.LookAt(Camera.main.gameObject.transform);
        
    }
}
