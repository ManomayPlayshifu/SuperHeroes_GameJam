using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Box : NetworkBehaviour
{
    public bool CanDamage = false;



    private void OnTriggerEnter(Collider other)
    {
        if (!NetworkClient.active)
        {
            return;
        }

        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy) && CanDamage)
        {
            uint id = PlayerData.Instance.MyCharacterGO.GetComponent<NetworkIdentity>().netId;
            CanDamage = false;
            enemy.DecreaseEnemyHealth(25,id);
        }
    }

    public void TurnOnDamage()
    {
        CanDamage = true;

    }

    public void TurnOffDamage(float delay)
    {
        StartCoroutine(DamageOff(delay));
    }

    IEnumerator DamageOff(float delay)
    {
        yield return new WaitForSeconds(delay);
        CanDamage = false;
    }
}
