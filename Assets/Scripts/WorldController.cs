using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    //singleton
    private static WorldController instance = null;
    public static WorldController Instance => instance;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (instance != null && instance != this)
            Destroy(this);

        instance = this;
    }
}
