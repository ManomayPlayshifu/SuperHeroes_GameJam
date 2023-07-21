using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SuperNetworkManager : NetworkManager
{
    public static SuperNetworkManager instance;

    public override void Awake()
    {
        base.Awake();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
        }
    }

    public void SetPlayerUsername(string username)
    {
        PlayerData playerData = playerPrefab.GetComponent<PlayerData>();
        if (playerData != null)
        {
            playerData.username = username;
        }
    }
    
    // Start is called before the first frame update
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);
    }
    

}
