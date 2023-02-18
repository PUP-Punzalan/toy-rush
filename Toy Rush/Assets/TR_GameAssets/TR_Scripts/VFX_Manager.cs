using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Manager : MonoBehaviour
{
    // Variables
    private TR_SpawnManager spawnManager;
    public ParticleSystem pointVFX;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<TR_SpawnManager>();
    }

    /* - This will be called whenever a correct point dropped in the correct shape basket.
       - Play VFX and SFX.
       - Update score. */
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Shape 1") && gameObject.CompareTag("Shape1Bench"))
        {
            pointVFX.Play();
            spawnManager.UpdateScore(4);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Shape 2") && gameObject.CompareTag("Shape2Bench"))
        {
            pointVFX.Play();
            spawnManager.UpdateScore(6);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Shape 3") && gameObject.CompareTag("Shape3Bench"))
        {
            pointVFX.Play();
            spawnManager.UpdateScore(8);
            Destroy(other.gameObject);
        }
    }
}
