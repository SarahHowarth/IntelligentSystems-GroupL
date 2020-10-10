using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Depot : MasterRoutingAgent
{
    private Transform position;

    public Transform Position{
        get { return position; }
        set { position = value; }
    }
}
