using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using Cinemachine;

public class PlayerHealth : NetworkBehaviour
{
    [SyncVar(hook = nameof(HealthChange))]
    float Health = 300;

    float MaxHealth = 300;

    public float _Health { get => Health; set => Health = value; }

    public CinemachineFreeLook cinemachineFreeLook;

    bool HitRecently = false;

    Coroutine corutine;

    private GameObject obj;

    private void Start()
    {
        obj = GameObject.FindGameObjectWithTag("HUD");
        obj.transform.GetChild(0).GetComponent<Slider>().value = (Health / MaxHealth);

    }

    void HealthChange(float old, float newvalue)
    {
        if(NetworkClient.active && isOwned)
        {
            obj.transform.GetChild(0).GetComponent<Slider>().value = (Health / MaxHealth);
        }

        if (newvalue <= 0f && NetworkServer.active)
        {

            
        }
    }

    IEnumerator HitTimer()
    {
        yield return new WaitForSeconds(5f);
        while(Health<MaxHealth)
            Health += Time.deltaTime * 8;
    }

}



