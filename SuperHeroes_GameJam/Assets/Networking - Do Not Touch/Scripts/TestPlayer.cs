using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class TestPlayer : NetworkBehaviour
{
    [SerializeField] private float speed = 1f;

    [SerializeField] private float rotationSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasAuthority)
            return;

        if (!NetworkClient.active)
            return;
        
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);
        
        Debug.Log(PlayerData.Instance.MyCharacterGO.name);
    }
}
