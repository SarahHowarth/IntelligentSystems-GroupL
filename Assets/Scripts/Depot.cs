using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Depot : MasterRoutingAgent
{
    private Transform position;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Transform Position{
        get { return position; }
        set { position = value; }
    }
}
