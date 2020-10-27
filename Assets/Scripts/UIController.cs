using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    //singleton
    public static UIController Instance => Instance;

    //text that will change
    public Text timerText;
    public Text fitnessDemand;
    public Text fitnessDistance;
    public Text fitnessGenerations;
    public Text maxParcelsPerPt;
    //images to toggle the start button between
    public Sprite spriteStopped;
    public Sprite spriteOneSpeed;
    public Sprite spriteTwoSpeed;
    //UI buttons
    public Button startToggle;
    public Button recompute;
    //public Button upDelAgents;
    //public Button downDelAgents;
    public Button upDels;
    public Button downDels;

    private float startTime;
    private Modes mode = Modes.stopped; 
    private enum Modes
    {
        stopped,
        oneSpeed,
        twoSpeed
    }

    void Recompute() {
        startTime = Time.time;
        SetDels();
        WorldController.Instance.TryToStart();

        //pauses the simulaion, ready to start when user hits the start button
        mode = Modes.twoSpeed;
        ToggleStart();
    }
    void ToggleStart() {
        switch (mode)
        {
            case Modes.stopped:
                {
                    //go to next mode -> 1x speed
                    Time.timeScale = 1;
                    mode = Modes.oneSpeed;
                    startToggle.image.sprite = spriteOneSpeed;
                    WorldController.Instance.Resume();
                    break;
                }
            case Modes.oneSpeed:
                {
                    //go to next mode -> 2x speed
                    Time.timeScale = 2;
                    mode = Modes.twoSpeed;
                    startToggle.image.sprite = spriteTwoSpeed;
                    break;
                }
            case Modes.twoSpeed:
                {
                    //go to next mode -> stopped
                    Time.timeScale = 0;
                    mode = Modes.stopped;
                    startToggle.image.sprite = spriteStopped;
                    WorldController.Instance.Pause();
                    break;
                }
            default: break;
        }
    }
    
   /* Left for potential extension 
    void IncrementDelAgents() {
        string fromField = delAgentsInputField.text;
        int toField = Int32.Parse(fromField) + 1;
        delAgentsInputField.text.Replace(fromField, toField.ToString());
    }
    void DecrementDelAgents() {
        string fromField = delAgentsInputField.text;
        int toField = Int32.Parse(fromField) + 1;
        delAgentsInputField.text.Replace(fromField, toField.ToString());
    }
   */

    //increment the maximum parcels for delivery
    void IncrementDeliveries() {
        if ((maxParcelsPerPt != null) && (maxParcelsPerPt.text != "3"))
        {
            int val = (Int32.Parse(maxParcelsPerPt.text)) + 1;
            maxParcelsPerPt.text = val.ToString();

        }
    }

    //decrement the maximum parcels for delivery
    void DecrementDeliveries() {
        if ((maxParcelsPerPt != null) && (maxParcelsPerPt.text != "1"))
        {
            int val = (Int32.Parse(maxParcelsPerPt.text)) - 1;
            maxParcelsPerPt.text = val.ToString();

        }
    }

    /* for potential extension, void SetDelAgents() { }*/
    void SetDels() {
        WorldController.Instance.SetMaxPackages(Int32.Parse(maxParcelsPerPt.text));
    }

    public void SetFitness(double totalDemand, double totalDistance)
    {
        fitnessDistance.text = totalDistance.ToString();
        fitnessDemand.text = totalDemand.ToString();
    }

    public void NumberGenerations(int gens)
    {
        fitnessGenerations.text = gens.ToString();
    }

    //worldcontroller.intance().
    // Start is called before the first frame update
    void Start()
    {
        //On click - run corresponding function for each button
        recompute.GetComponent<Button>().onClick.AddListener(Recompute);
        startToggle.GetComponent<Button>().onClick.AddListener(ToggleStart);
        
        /* Note - these are here for potential extension
        upDelAgents.GetComponent<Button>().onClick.AddListener(IncrementDelAgents);
        downDelAgents.GetComponent<Button>().onClick.AddListener(DecrementDelAgents);
        */
        
        upDels.GetComponent<Button>().onClick.AddListener(IncrementDeliveries);
        downDels.GetComponent<Button>().onClick.AddListener(DecrementDeliveries);
        
        //compute based on initial/default values
        Recompute();
    }

    // Update is called once per frame
    void Update()
    {
        float tDiff = Time.time - startTime;
        string minutes = ((int)tDiff / 60).ToString();
        string seconds = (tDiff % 60).ToString("f2");
        timerText.text = minutes + ":" + seconds;

        /* not necessary due to timescale, just present as backup until tested
        switch (mode)
        {
            case Modes.stopped:
                {
                    break;
                }
            case Modes.oneSpeed | Modes.twoSpeed:
                {
                    
                    break;
                }
            default: break;
        }*/
    }
}
