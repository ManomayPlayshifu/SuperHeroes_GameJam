using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCharacterAnims;
using Cinemachine;
using System;

public class DPSPowers : MonoBehaviour
{
    public GameObject FreeLook;
    [Space]
    public GameObject BulletPrefab;
    [Space]
    public Transform[] BulletSpawns;

    Transform Target = null;

    bool Shoot;

    float Timer;
    public float CoolDown;

    public RPGCharacterInputController RPGCharacterInputController;

    RaycastHit Hit;

    public void BulletAttack()
    {
        Shoot = true;
        Timer = 1f;
    }

    public void BulletAttackStop()
    {
        Shoot = false;
        Timer = 0f;
    }

    private void Update()
    {
        if (Shoot)
        {
            Timer += Time.deltaTime;

            if(Timer >= CoolDown)
            {
                int random = UnityEngine.Random.Range(0, 4);

                if (random == 0)
                {
                    RPGCharacterInputController.SlowMotion(2f);
                }


                foreach (Transform spawnPoint in BulletSpawns)
                {
                    var bullet = Instantiate(BulletPrefab, spawnPoint.position, spawnPoint.rotation);
                    //bullet.GetComponent<Bullet>().Target = Target;

                }
                Timer = 0f;
            }
        }
    }

  

    //private void FixedUpdate()
    //{
    //    if (Shoot)
    //    {
    //        Vector3 pos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    //        LayerMask mask = LayerMask.GetMask("Enemy");
    //        if (Physics.Raycast(Camera.main.ScreenPointToRay(pos), out Hit, 20f, mask))
    //        {
    //            Target = Hit.transform;
    //        }
    //        else
    //        {
    //            Target = null;
    //        }

    //    }
    //}

}
