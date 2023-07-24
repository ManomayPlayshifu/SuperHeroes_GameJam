using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private GameObject axe;
    public float damageAmount = 10f;
    public LayerMask playerLayer; // Set this in the Inspector to the layer that represents the player.

    private bool isAttacking = false;
    private Enemy _enemy;
    private void Start()
    {
        _enemy = transform.GetComponent<Enemy>();
    }

    private void Update()
    {
        if(!_enemy.EnableAxeCollider)
            return;
        if (isAttacking)
            return;

        Collider[] hitColliders = Physics.OverlapBox(axe.transform.position+new Vector3(0,0,0.5f), transform.localScale / 2f, Quaternion.identity, playerLayer);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                isAttacking = true;
                Debug.Log("Attack hit");

                col.gameObject.GetComponent<PlayerHealth>()._Health -= 10f;

               Invoke("DamagePlayer",0f);
                
                Invoke("ResetAttack", _enemy.AttackSpeed); 
                break;
            }
        }
    }
    
    private void ResetAttack()
    {
        isAttacking = false;
    }

    public void DamagePlayer()
    {
        _enemy.SpawnHitEffect(transform);
    }
}
