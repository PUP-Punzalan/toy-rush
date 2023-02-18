using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTS_PointsStick : MonoBehaviour
{
    // Variables
    private TR_SpawnManager spawnManager;
    private Rigidbody rb;
    public ParticleSystem dropVFX;
    private Quaternion pointRotation;
    private bool isRotated = false;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<TR_SpawnManager>();
        rb = GetComponent<Rigidbody>();
        pointRotation = transform.rotation;
    }

    // Update is called once per frame
    // Rotate point
    void Update()
    {
        transform.Rotate(0, 6.0f * 10.0f * Time.deltaTime, 0);
    }

    /* - This will be called whenever the point touches the sensor. 
       - Stop the rotation of point.
       - Play VFX. */
    private void OnTriggerEnter(Collider other)
    {
        spawnManager.soundEffects.clip = spawnManager.dropSFX;
        spawnManager.soundEffects.Play();

        dropVFX.Play();
        
        if (!isRotated)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            transform.rotation = pointRotation;

            isRotated = true;
            Debug.Log("Dumikit");
        }
    }
}
