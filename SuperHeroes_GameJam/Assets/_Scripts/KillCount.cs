using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class KillCount : NetworkBehaviour
{
    [SyncVar(hook = nameof(SpawnCinematic))]
    int Killcount = 0;

    public int EnemiesToKill = 3;
    public GameObject CutScene;

    GameObject SpawnedTimeline;

    

    public int _KillCount { get => Killcount; set => Killcount = value; }



    void SpawnCinematic(int old, int newvalue)
    {
        if (newvalue == EnemiesToKill && NetworkClient.active)
        {
            SpawnedTimeline = Instantiate(CutScene);
            StartCoroutine(Disable(5f));

        }
    }

    IEnumerator Disable( float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnedTimeline.SetActive(false);
    }

}
