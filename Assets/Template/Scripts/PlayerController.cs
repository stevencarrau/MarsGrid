using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public int gridsize = 8;
    private int grid_offset;
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        grid_offset = gridsize*5;
    }
    
    
    void OnMove(InputValue movementValue)
    {
    	Vector2 movementVector = movementValue.Get<Vector2>();
        if(movementVector.y!=0){
            movementY = movementVector.y;
            movementX = 0.0f;
        }
        else{
            movementX = movementVector.x;
            movementY = 0.0f;
        }
    }


    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement*speed);
        // Debug.Log(GridCell());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Deadzone"))
        {
        speed = 0;
        rb.velocity =  rb.velocity * 0.05f ;
        }
    }

    Vector2 GridCell()
    {
        int x = (int)(rb.position.x+grid_offset)/10;
        int y = (int)(-rb.position.z+grid_offset)/10;
        return new Vector2(x,y);        
    } 

}
