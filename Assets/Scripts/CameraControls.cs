using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{

    /*
    This code is largely based on FlyCamera.cs, written by Windexglow 11-13-10.   
    Converted to C# 27-02-13 - no credit wanted. Available at <https://gist.github.com/gunderson/d7f096bd07874f31671306318019d996>.

    This code has been largely extended to include zoom functionality and match our envirnoment by constraining view rotation.

    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    shift : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/


    float mainSpeed = 100.0f; //regular speed
    float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 1000.0f; //Maximum speed when holdin gshift
    float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;
    public Camera mainCam;

    void Update()
    {
        //implementing angled viewing based on mouse position, while user holds E. 
        if (Input.GetKey(KeyCode.E))
        {
            lastMouse = Input.mousePosition - lastMouse;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
            transform.eulerAngles = lastMouse;
            lastMouse = Input.mousePosition;
        }


        //Keyboard commands
        float f = 0.0f;
        Vector3 p = GetBaseInput();

        //holding shift speeds up camera movement
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.Space))
        { //If player wants to move on X and Z axis only
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            transform.Translate(p);
        }

    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)//forwards
        {
            //velocity based zoom in
            p_Velocity += new Vector3(0, 0, 3);

            //Zoom in using camera orthographic
            //mainCam.orthographicSize++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) //backwards
        {
            //velocity based zoom out
            p_Velocity += new Vector3(0, 0, -3);

            //Zoom out using camera orthographic
            //mainCam.orthographicSize--;
        }
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}
