using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCam : MonoBehaviour
{
    private static GameObject instance;

    void Awake()
    {
        if(instance != null )
        {
            Destroy(instance);            
        }
        instance = this.gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
