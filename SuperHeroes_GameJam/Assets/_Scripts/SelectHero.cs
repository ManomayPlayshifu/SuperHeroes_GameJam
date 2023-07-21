using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHero : MonoBehaviour
{
    public GameObject Tank;
    public GameObject DPS;
    public GameObject Support;

    public Transform SpawnPoint;

    public GameObject camera;

    public void Hero1()
    {
        //camera.SetActive(false);
        // Join the player with that Hero
        Instantiate(Tank, SpawnPoint.position, SpawnPoint.rotation);
        gameObject.SetActive(false);
    }

    public void Hero2()
    {
        //camera.SetActive(false);
        // Join the player with that Hero

        Instantiate(DPS, SpawnPoint.position, SpawnPoint.rotation);
        gameObject.SetActive(false);
    }

    public void Hero3()
    {
        //camera.SetActive(false);
        // Join the player with that Hero
        Instantiate(Support, SpawnPoint.position, SpawnPoint.rotation);
        gameObject.SetActive(false);

    }

}
