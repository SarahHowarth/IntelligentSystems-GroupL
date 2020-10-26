using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACLMessage : MonoBehaviour
{
 
    private string sender;
    private string receiver;
    private string performative;
    private string content;
    private List<GameObject> gameObjectContent;
    private string aclMessage;

    public ACLMessage()
    {
        aclMessage = "";
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

    //just for temporary until we test, then will use Content to parse route ID's as a string
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
       
        string[] performatives = { "request constraints", "send constraints", "request route", "send route" };
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
