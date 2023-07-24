using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InitialCutscene : NetworkBehaviour
{
    public GameObject Cutscene;

    GameObject spawnedScene;

    void Start()
    {
        if(NetworkClient.active && isOwned)
        {
            spawnedScene = Instantiate(Cutscene);
            StartCoroutine(StopCutScene());
        }
    }

    IEnumerator StopCutScene()
    {
        yield return new WaitForSeconds(7.5f);
        Destroy(spawnedScene);
    }
}
