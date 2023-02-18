using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTS_PointsGrab : MonoBehaviour
{
    // Variables
    private GameObject player;

    // Start is called before the first frame update
    private Vector3 offset = new Vector3(0, 2, 0);
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    // Repeatedly change the point position relative to the player's position with an offset.
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
