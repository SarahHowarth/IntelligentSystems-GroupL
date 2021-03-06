﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACLMessage : MonoBehaviour
{
 
    private string sender;
    private string receiver;
    private string performative;
    private string content;
    private List<GameObject> gameObjectContent;

    public ACLMessage()
    {
        sender = "";
        receiver = "";
        performative = "";
        content = "";
    }

    public string Sender
    {
        get
        {
            return sender;
        }
        set
        {
            sender = value;
        }
    }

    public string Receiver
    {
        get
        {
            return receiver;
        }
        set
        {
            receiver = value;
        }
    }

    public string Performative
    {
        get
        {
            return performative;
        }
        set
        {
            performative = "";
            bool found = CheckPerformative(value.ToString());
           
            if (!found)
                throw new
                InvalidPerformativeException("Performative not in the list:" + value);
            else
                performative = value;
        }
    }

    //for string content
    public string Content
    {
        get
        {
            return content;
        }
        set
        {
            content = value;
        }
    }

    //for game object content - routes and packages
    //can use string to pass the constraints
    public List<GameObject> GameObjectContent
    {
        get
        {
            return gameObjectContent;
        }
        set
        {
            gameObjectContent = value;
        }
    }

    public string AclMessage
    {
        get
        {
        
            string temp = "";
            temp = "(" + performative.ToLower() +
            ":sender (agent-identifier :name " + sender + ") " +
            ":receiver (set (agent-identifier :name " + receiver + ") " +
            ":content " + content +
            " )";
            return temp;
        }
    }

    private bool CheckPerformative(string candidPerform)
    {
       
        string[] performatives = { "request constraints", "send constraints", "send route", "send packages" };
        int i = 0;
        bool found = false;

        if (candidPerform == null)
            i = performatives.Length + 1;
        candidPerform = candidPerform.ToLower();
        while (i < performatives.Length && !found)
        {
            if (candidPerform.CompareTo(performatives[i]) == 0)
                found = true;
            i++;
        }
        return found;
    }
}

class InvalidPerformativeException : System.Exception
{
    public InvalidPerformativeException(string message) :
    base(message)
    {

    }
}
