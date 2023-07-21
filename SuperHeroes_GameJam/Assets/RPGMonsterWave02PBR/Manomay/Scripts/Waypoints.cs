using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    private List<Transform> nodes = new List<Transform>();

    [SerializeField] private Enemy enemy;

    private int currenttarget=-1;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform var in transform)
        {
            nodes.Add(var);
        }
    }

    // Update is called once per frame
    void Update()
    {
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
