using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldController : MonoBehaviour
{
    //singleton
    private static WorldController instance = null;
    public static WorldController Instance => instance;

    [SerializeField] private NavMeshSurface navMesh = default;
   
    //ai private data
    [SerializeField]private GameObject depot;
    private List<GameObject> dropPoints;
    private List<GameObject> deliveryVehicles;

    //data from UI
    private int packagesToDeliver = 0; //equates to the number of drop points? 1 package per place 
    private int numberOfDeliveryAgents = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null && instance != this)
            Destroy(this);

        instance = this; 
    }
    

    /// <summary>
    /// UI to call to start simulation
    /// returns true or false if can start 
    /// </summary>
    /// <returns></returns>
    public bool TryToStart()
    {
        if (packagesToDeliver == 0 || numberOfDeliveryAgents == 0) 
        {
            return false;
        }
        //TO DO: need to rebake navmesh first
        StartSimulation();
        return true;
    }

    private void StartSimulation() 
    {
        //To Do intialise master routing agent with the private ai data so they can start 
    
    }

    private void InstantiateVehicles(int numberOfVehicles) 
    { 
        //TO DO Instantiate prefabs once added
    }

    private void InstantiateDropPoints(int numberOfPackages) 
    { 
        //TO DO instantiate prefabs once added 
    }

    
    //properties
    public GameObject GetDepot() { return depot; }

    public List<GameObject> GetDeliveryVehicles() { return deliveryVehicles; }

    public List<GameObject> GetDropPoints() { return dropPoints; }

    ///ui manager to call to set the packages and vehicles from ui for initialisation
    public void SetPackagesToDeliver(int packages) 
    { 
        packagesToDeliver = packages;
        InstantiateDropPoints(packagesToDeliver);
    }

    public void SetNumberOfDeliveryVehicles(int agents) 
    {
        numberOfDeliveryAgents = agents;
        InstantiateVehicles(numberOfDeliveryAgents);
    }





    //extension functions for dynamic addition of packages etc.
    //will need to link to functions that update the simulation
    public void AddDropPoint() { }
    public void AddPackage() { }
    public void RebakeNavMesh() { }


}
