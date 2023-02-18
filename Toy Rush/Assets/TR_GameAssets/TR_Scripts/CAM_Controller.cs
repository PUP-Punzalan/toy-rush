using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAM_Controller : MonoBehaviour
{
    // Variables
    private TR_SpawnManager spawnManager;
    public Transform player;
    public Vector3 Offset;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<TR_SpawnManager>();

    }

    // Update is called once per frame
    // Repeatedly follow the player.
    void LateUpdate()
    {
        if (spawnManager.isGameActive)
        {
            transform.LookAt(player, new Vector3(0, 180, 0));
        }
    }
}
