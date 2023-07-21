using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class TurnOnCamera : NetworkBehaviour
{

    void Start()
    {
        if(isOwned && NetworkClient.active)
        {
            GetComponent<CinemachineFreeLook>().enabled = true;
        }
    }

    
}
