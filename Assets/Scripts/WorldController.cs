﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldController : MonoBehaviour
{
    //singleton
    private static WorldController instance = null;
    public static WorldController Instance => instance;

    //prefabs to spawn + navmesh
    [SerializeField] private GameObject sedanPrefab;
    [SerializeField] private GameObject utePrefab;
    [SerializeField] private GameObject vanPrefab;
    [SerializeField] private GameObject truckPrefab;
    [SerializeField] private GameObject packagePrefab;
    //[SerializeField] private GameObject dropPointPrefab = default;
    [SerializeField] private NavMeshSurface navMesh = default;

    //ai private data
    [SerializeField] private GameObject depot = default;
    [SerializeField] private List<GameObject> dropPoints;
    private List<GameObject> deliveryVehicles;
    private List<GameObject> packages;

    //data from UI
    //private int numberOfDropPoints = 0;
    //private int numberOfDeliveryAgents = 0;
    private int numberOfPackagesMin = 1;//randomly assign number of packages to droppoints in MRA based on mix and max
    private int numberOfPackagesMax = 1;
    //data for initialisation
    private const float PACKAGE_WEIGHT_MIN = 0.5f;
    private const float PACKAGE_WEIGHT_MAX = 5.0f;


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
        StartSimulation();
        return true;
    }

    private void StartSimulation()
    {
        //setup droppoints
        for (int i = 0; i < dropPoints.Count; i++)
        {
            dropPoints[i].GetComponent<DropPoint>().ID = i;
        }
        //setup delivery agents
        SetupPackages();
        SetupVehicles();
        //rebake navmesh

        //intialise master routing agent with the private ai data so they can start
        Depot depotScript = depot.GetComponent<Depot>();
        depotScript.Setup(dropPoints, deliveryVehicles, packages);
    }

    private void SetupVehicles()
    {
        GameObject createdObject;

        //add 3 of each vehicle for first initialisation
        for (int i = 0; i <= 3; i++) 
        {
            createdObject = Instantiate(sedanPrefab, depot.transform.position, depot.transform.rotation);
            DeliveryAgent createdObjectScript = createdObject.GetComponent<DeliveryAgent>();
            createdObjectScript.Setup(VehicleType.sedan, depot.GetComponent<Depot>());
            deliveryVehicles.Add(createdObject);
        }

        for (int i = 0; i <= 3; i++)
        {
            createdObject = Instantiate(utePrefab, depot.transform.position, depot.transform.rotation);
            DeliveryAgent createdObjectScript = createdObject.GetComponent<DeliveryAgent>();
            createdObjectScript.Setup(VehicleType.ute, depot.GetComponent<Depot>());
            deliveryVehicles.Add(createdObject);
        }

        for (int i = 0; i <= 3; i++)
        {
            createdObject = Instantiate(vanPrefab, depot.transform.position, depot.transform.rotation);
            DeliveryAgent createdObjectScript = createdObject.GetComponent<DeliveryAgent>();
            createdObjectScript.Setup(VehicleType.van, depot.GetComponent<Depot>());
            deliveryVehicles.Add(createdObject);
        }

        for (int i = 0; i <= 3; i++)
        {
            createdObject = Instantiate(truckPrefab, depot.transform.position, depot.transform.rotation);
            DeliveryAgent createdObjectScript = createdObject.GetComponent<DeliveryAgent>();
            createdObjectScript.Setup(VehicleType.truck, depot.GetComponent<Depot>());
            deliveryVehicles.Add(createdObject);
        }

        while (!CheckEnoughTrucks()) 
        {
            //not enough trucks just add another van
            createdObject = Instantiate(vanPrefab, depot.transform.position, depot.transform.rotation);
            DeliveryAgent createdObjectScript = createdObject.GetComponent<DeliveryAgent>();
            createdObjectScript.Setup(VehicleType.van, depot.GetComponent<Depot>());
            deliveryVehicles.Add(createdObject);
        }

        //go through agents and setup ID's
        for (int i = 0; i < deliveryVehicles.Count; i++)
        {
           deliveryVehicles[i].GetComponent<DeliveryAgent>().ID = i;
        }
    }

    private bool CheckEnoughTrucks() 
    {
        float totalPackageWeight = 0.0f;
        int totalTruckWeight = 0;

        foreach (GameObject p in packages) 
        {
            totalPackageWeight += p.GetComponent<Package>().Weight;
        }
        foreach (GameObject d in deliveryVehicles) 
        {
            totalTruckWeight += d.GetComponent<DeliveryAgent>().GetWeight;
        }

        if (totalPackageWeight >= totalTruckWeight) 
        { 
            return false; 
        }
        return true;

    }

    private void SetupPackages() 
    {
        //add package prefab to drop point based on min and max
        foreach (GameObject dp in dropPoints) 
        {
            DropPoint dropPointScript = dp.GetComponent<DropPoint>();
            int packagesToAssign = Random.Range(numberOfPackagesMin, numberOfPackagesMax + 1); //random.range is not inclusive of max number with integer values
            for (int i = 0; i <= packagesToAssign; i++) 
            {
                GameObject newPackage = Instantiate(packagePrefab, depot.transform.position, depot.transform.rotation);
                newPackage.GetComponent<Package>().Destination = dropPointScript;
                packages.Add(newPackage);
            } 
        }

        //setup package ID's and weight
        for (int i = 0; i < packages.Count; i++) 
        {
            Package packageScript = packages[i].GetComponent<Package>();
            packageScript.ID = i;
            packageScript.Weight = Random.Range(PACKAGE_WEIGHT_MIN, PACKAGE_WEIGHT_MAX);
        }
    }

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
    //private void InstantiateDropPoints(int numberOfPackages)
    //{
    //    //TO DO:
    //    //instantiate droppoint prefabs in random locations
    //    //add to drop points list

    //}
}
