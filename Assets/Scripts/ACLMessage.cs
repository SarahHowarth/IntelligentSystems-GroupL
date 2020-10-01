using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACLMessage : MonoBehaviour
{
    private string sender;
    private string reciever;
    private string performative;
    private string content;
    private string aclMessasge; //readonly property

    public ACLMessage()
    {
        aclMessasge = "";
        sender = "";
        reciever = "";
        performative = "";
        content = "";


    }

    public string Sender
    {
        get { return sender; }
        set { sender = value; }
        //if set doesn't work, try 'set {sender = value;}'
    }

    public string Reciever
    {
        get { return reciever; }
        set { reciever = value; }
    }
    public string Performative
    {
        get { return performative; }
        set 
        {
            performative = "";
            bool found = checkPerformative(value.ToString());
            if (!found)
            {
                throw new InvalidPerformativeException("Performative not in the list: " + value);
            }
            else
            {
                performative = value;
            }
        }
    }
    public string Content
    {
        get { return content; }
        set { content = value; }
    }
    public string AclMessage
    {
        get 
        {
            string temp = "";
            temp = "(" + performative.ToLower() + ":sender (agent-identifier :name "
+ reciever + ") " + ":content " + content + ")";
            return temp;
        }
    }

    //validation for performatives 
    private bool checkPerformative(string candidPerform)
    {
        string[] performatives = { "accept proposal", "agree", "request" };
        int i = 0;

        //perfomative isn't in list by default
        bool found = false;

        if(candidPerform == null)
        {
            i = performatives.Length + 1;
            candidPerform = candidPerform.ToLower();
            
            while(i < performative.Length && !found)
            {
                if(candidPerform.CompareTo(performatives[i]) == 0)
                    found = true;

                i++;
            }
            return found;
        }
    }
}

//does this need to be a MonoBehaviour 
class InvalidPerformativeException : System.Exception
{
    public InvalidPerformativeException(string message) :
    base(message)
    {

    }
}