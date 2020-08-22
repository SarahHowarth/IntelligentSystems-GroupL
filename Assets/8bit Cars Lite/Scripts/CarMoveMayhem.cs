using UnityEngine;

public class CarMoveMayhem : MonoBehaviour {

    Vector2 target;
    Quaternion rot;
    
	void Start () {
        target = transform.position;
        rot = transform.rotation;

        if (rot.z == 0f || rot.z == 1f) 
        {
            // car is moving on x axis
            target.x = -target.x;
        }
        else if (rot.z <= -0.6f || rot.z >= 0.6f)
        {
            // car is moving on y axis
            target.y = -target.y;
        }
    }
	
	void FixedUpdate ()
    {
        HasArrived();
        transform.rotation = rot;
        transform.position = Vector2.MoveTowards(transform.position, target, .075f);
    }

    private void HasArrived()
    {
        // destroy gameobject when it arrives at location
        if (rot.z == 0f || rot.z == 1f)
        {
            // car is moving on x axis
            if (transform.position.x == target.x)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // car is moving on y axis
            if (transform.position.y == target.y)
            {
                Destroy(gameObject);
            }
        }
    }
}
