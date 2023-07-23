using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Waypoints : NetworkBehaviour
{
    private List<Transform> nodes = new List<Transform>();

    private Enemy enemy;
    private int currenttarget=-1;

    [SerializeField] private GameObject enemyprefab;
    [SerializeField] private Transform spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        if(!NetworkServer.active)
            return;

        GameObject obj = Instantiate(enemyprefab,spawnPosition.position,Quaternion.identity);
        NetworkServer.Spawn(obj);

        enemy = obj.GetComponent<Enemy>();
        
        foreach (Transform var in transform)
        {
            nodes.Add(var);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!NetworkServer.active)
            return;
        if(enemy==null)
            return;
        
        if (enemy.movemode == Enemy.MoveMode.Attack)
        {
            enemy.waypointtarget = null;
            return;
        }
           
        
        if (enemy.waypointtarget == null || currenttarget<0)
        {
            currenttarget = 0;
            enemy.waypointtarget = nodes[currenttarget].gameObject;
        }
        else
        {
            if (Vector3.Distance(enemy.gameObject.transform.position, enemy.waypointtarget.transform.position)<2f)
            { 
                Debug.Log("Changing Target");
                
                
                currenttarget++;
                if (currenttarget >= nodes.Count)
                {
                    currenttarget = 0;
                }
                enemy.waypointtarget = nodes[currenttarget].gameObject;
            }
        }
    }
}
