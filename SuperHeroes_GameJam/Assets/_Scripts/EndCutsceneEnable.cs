using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EndCutsceneEnable : NetworkBehaviour
{
    [SerializeField] private GameObject endcutscene;
    private GameObject hud;
    private void Start()
    {
        hud = GameObject.FindGameObjectWithTag("HUD");
    }

    private void OnDestroy()
    {
        if (NetworkClient.active)
        {
            foreach (var VARIABLE in endcutscene.GetComponent<PlayersInCutscene>().playerlist)
            {
                if (PlayerData.Instance.MyCharacterGO.name.Contains(VARIABLE.name))
                {
                    VARIABLE.SetActive(true);
                }
                else
                {
                    VARIABLE.SetActive(false);
                }
            }
            
            hud.SetActive(false);
            Camera.main.gameObject.SetActive(false);
            GameObject obj = Instantiate(endcutscene);
            
        }
    }
}
