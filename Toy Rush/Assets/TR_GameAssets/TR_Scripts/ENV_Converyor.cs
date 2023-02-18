using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENV_Converyor : MonoBehaviour
{
    // Variables
    Rigidbody rbody;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = rbody.position;
        rbody.position += Vector3.back * speed * Time.fixedDeltaTime;
        rbody.MovePosition(pos);
    }
}
