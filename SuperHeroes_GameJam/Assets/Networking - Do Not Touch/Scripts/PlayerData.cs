using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class PlayerData : NetworkBehaviour
{
    // Start is called before the first frame update
    [SyncVar(hook = nameof(OnUsernameChanged))]
    public string username = "Player";
    public GameObject usernameCanvas;
    public GameObject characterSelectionCanvas;
    public TMP_InputField textField;
    private GameObject mycharactergo;
    public GameObject[] characterPrefabs;
    private int buttonIndex = -1;
    public GameObject MyCharacterGO
    {
        get => mycharactergo;
        set => mycharactergo = value;
    }
    public static PlayerData Instance;
    
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        Instance = this;
    }

    private void OnDestroy()
    {
        if(Instance==this)
            Instance = null;
    }

    private void Start()
    {
        if (hasAuthority)
        {
            usernameCanvas.SetActive(true);
        }
    }

    void OnUsernameChanged(string oldUsername, string newUsername)
    {
        Debug.Log("Name Changed = "+ username);
    }

    public void ConfirmClick()
    {
        if (hasAuthority)
        {
            username = textField.text;
            usernameCanvas.SetActive(false);
            characterSelectionCanvas.SetActive((true));
        }
    }

    [Client]
    public void CharacterSelected(int index)
    {
        MyCharacterGO = characterPrefabs[index];
        characterSelectionCanvas.SetActive(false);
        cmdspawn(index);
    }
    
    [Command(requiresAuthority = true)]
     public void cmdspawn(int index)
    {
        NetworkIdentity playerIdentity = connectionToClient.identity;
        GameObject playerGameObject = playerIdentity.gameObject;
        GameObject obj = Instantiate(characterPrefabs[index]);
        NetworkServer.Spawn(obj,playerGameObject);
        GetPlayerCharacter(connectionToClient,obj.GetComponent<NetworkIdentity>().netId);
        RoomData.instance.AddPlayerObj(obj);
    }

     [TargetRpc]
     void GetPlayerCharacter(NetworkConnection conn, uint netid)
     {
         MyCharacterGO = NetworkClient.spawned[netid].gameObject;
     }


}
