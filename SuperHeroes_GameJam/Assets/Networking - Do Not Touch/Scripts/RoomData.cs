using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RoomData : NetworkBehaviour
{
    public static RoomData instance;
    public List<GameObject> playerObjs = new List<GameObject>();

    // Start is called before the first frame update
    public override void OnStartServer()
    {
        base.OnStartServer();
        instance = this;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
            playerObjs.Clear();
        }
    }

    public void AddPlayerObj(GameObject obj)
    {
        if(playerObjs.Contains(obj))
            return;
        
        playerObjs.Add(obj);
    }

    public void RemovePlayerObj(GameObject obj)
    {
        if(!playerObjs.Contains(obj))
            return;
        
        playerObjs.Remove(obj);
    }

}
