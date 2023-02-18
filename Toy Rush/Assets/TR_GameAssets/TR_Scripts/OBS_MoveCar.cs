using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBS_MoveCar : MonoBehaviour
{
    // Variables
    private TR_SpawnManager carPrefab;
    public float carSpeed;
    private int boundary = 20;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    // Move car
    void Update()
    {
        transform.Translate(Vector3.right * carSpeed * Time.deltaTime);

        if ((transform.position.x < -boundary) || (transform.position.x > boundary))
        {
            Destroy(gameObject);
        }
    }
}
