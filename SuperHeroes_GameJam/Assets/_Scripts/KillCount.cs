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
            Camera.main.gameObject.SetActive(false);
            SpawnedTimeline = Instantiate(CutScene);
            float duration = (float)SpawnedTimeline.transform.GetChild(0).GetComponent<PlayableDirector>().playableAsset.duration;
            StartCoroutine(Disable(duration));

        }
    }

    IEnumerator Disable( float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnedTimeline.SetActive(false);
        Camera.main.gameObject.SetActive(true);
    }

}
