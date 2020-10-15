using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldController : MonoBehaviour
{
    //singleton
    private static WorldController instance = null;
    public static WorldController Instance => instance;

    //prefabs to spawn + navmesh
    [SerializeField] private List<GameObject> truckPrefabs = default;
    [SerializeField] private GameObject dropPointPrefab = default;
    [SerializeField] private NavMeshSurface navMesh = default;
   
    //ai private data
    [SerializeField]private GameObject depot = default;
    private List<GameObject> dropPoints;
    private List<GameObject> deliveryVehicles;

    //data from UI
    private int numberOfDropPoints = 0;
    private int numberOfDeliveryAgents = 0;
    private int numberOfPackagesMin = 1;//randomly assign number of packages to droppoints in MRA based on mix and max
    private int numberOfPackagesMax = 1;

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
        if (numberOfPackagesMax == 0) 
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
        InstantiateDropPoints(numberOfDropPoints);
        InstantiateVehicles(numberOfDeliveryAgents);
        Depot depotScript = depot.GetComponent<Depot>();
        depotScript.Setup(dropPoints, deliveryVehicles, numberOfPackagesMin, numberOfPackagesMax);  
    }

    private void InstantiateVehicles(int numberOfVehicles) 
    { 
        //TO DO Instantiate prefabs once added
    }

    private void InstantiateDropPoints(int numberOfPackages) 
    { 
        //TO DO:
        //instantiate droppoint prefabs in random locations
        //add to drop points list

    }

    public void RebakeNavMesh() { }


    //properties
    public GameObject GetDepot() { return depot; }

    public List<GameObject> GetDeliveryVehicles() { return deliveryVehicles; }

    public List<GameObject> GetDropPoints() { return dropPoints; }

    ///ui manager to call to set the packages and vehicles from ui for initialisation
    ///essentially just setting properties
    public void SetMaxPackages(int packages) 
    {
        numberOfPackagesMax = packages;
    }

    //extension functions for dynamic addition of packages etc.
    //will need to link to functions that update the simulation
    //public void AddDropPoint() { }
    //public void AddPackage() { }
    //public void SetNumberOfDropPoints(int dropPoints)
    //{
    //    numberOfDropPoints = dropPoints;
    //}
    //public void SetNumberOfDeliveryVehicles(int agents)
    //{
    //    numberOfDeliveryAgents = agents;
    //}
    //public void SetMinPackages(int packages)
    //{
    //    numberOfPackagesMin = packages;
    //}
}
