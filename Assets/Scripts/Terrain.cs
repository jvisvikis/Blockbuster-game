using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Terrain : MonoBehaviour
{
    Rigidbody rigidbody;
    Conductor conductor;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        conductor = Conductor.conductor;
    }

    // Update is called once per frame
    void Update()
    {
        //Move the terrain towards the player at speed determined by the Conductor
        transform.position += Vector3.back * Time.deltaTime * conductor.getLevelSpeed();

        if (transform.position.z <= conductor.getBoundary())
        {
            transform.position = new Vector3(Random.Range(-20.0f, 20.0f), transform.position.y, 60.0f);
        }
    }
}
