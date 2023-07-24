using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Fists : MonoBehaviour
{
    public bool CanDamage = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!NetworkClient.active)
        {
            return;
        }

        uint id = PlayerData.Instance.MyCharacterGO.GetComponent<NetworkIdentity>().netId;

        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy) && CanDamage)
        {
            CanDamage = false;
            enemy.DecreaseEnemyHealth(15, id);
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
