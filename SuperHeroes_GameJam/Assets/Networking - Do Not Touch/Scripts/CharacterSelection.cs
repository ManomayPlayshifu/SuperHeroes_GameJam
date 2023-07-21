using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public Button[] buttons;
    private PlayerData _playerData;
    private void Start()
    {
        _playerData = transform.root.gameObject.GetComponent<PlayerData>();
       buttons[0].onClick.AddListener(() => OnClickCharacter(0));
       buttons[1].onClick.AddListener(() => OnClickCharacter(1));
       buttons[2].onClick.AddListener(() => OnClickCharacter(2));

     
    }

    void OnClickCharacter(int index)
    {
        _playerData.CharacterSelected(index);
    }
    

    

}
