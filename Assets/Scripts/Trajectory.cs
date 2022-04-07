using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class Trajectory : MonoBehaviour
{
    private Rigidbody rb;
    public ObstacleAgent uav; 
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Vector3 current_vel = rb.velocity; 
        if (uav.moveX==0){rb.velocity = new Vector3(0.0f,rb.velocity.y,rb.velocity.z);}
        if (uav.moveZ==0){rb.velocity = new Vector3(rb.velocity.x,rb.velocity.y,0.0f);}
        rb.AddForce(new Vector3(uav.moveX,0,uav.moveZ)*Time.deltaTime*uav.moveSpeed);
    }
}
