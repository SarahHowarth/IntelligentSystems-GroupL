using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //text that will change
    public Text timerText;
    public Text fitnessMetric;
    public InputField delsInputField;
    public InputField delAgentsInputField;
    //images to toggle the start button between
    public Sprite spriteStopped;
    public Sprite spriteOneSpeed;
    public Sprite spriteTwoSpeed;
    //UI buttons
    public Button startToggle;
    public Button recompute;
    public Button upDelAgents;
    public Button downDelAgents;
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
        startTime = 0;
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
                    break;
                }
            default: break;
        }
    }
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
    void IncrementDeliveries() {
        string fromField = delsInputField.text;
        int toField = Int32.Parse(fromField) + 1;
        delsInputField.text.Replace(fromField, toField.ToString());
    }
    void DecrementDeliveries() {
        string fromField = delsInputField.text;
        int toField = Int32.Parse(fromField) - 1;
        delsInputField.text.Replace(fromField, toField.ToString());
    }

    void SetDelAgents() { }
    void SetDels() { }










    //worldcontroller.intance().
    // Start is called before the first frame update
    void Start()
    {
        //On click - run corresponding function for each button
        recompute.GetComponent<Button>().onClick.AddListener(Recompute);
        startToggle.GetComponent<Button>().onClick.AddListener(ToggleStart);
        upDelAgents.GetComponent<Button>().onClick.AddListener(IncrementDelAgents);
        downDelAgents.GetComponent<Button>().onClick.AddListener(DecrementDelAgents);
        upDels.GetComponent<Button>().onClick.AddListener(IncrementDeliveries);
        downDels.GetComponent<Button>().onClick.AddListener(DecrementDeliveries);

    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }
}
