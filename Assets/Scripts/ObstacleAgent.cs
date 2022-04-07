using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.SideChannels;

public class ObstacleAgent : Agent {
    
    [SerializeField] private Transform targetTransform;
    public float moveSpeed;
    public int gridsize = 8;
    private int grid_offset;
    private Vector3[] init_positions;
    private Rigidbody rb;
    private int init_idx;
    public int moveX;
    public int moveZ;
    CustomSideChannel sideChannel;
    // public bool maskActions = true;
    // private ActMask = new List<bool>(4);
    // ActMask.Add(true).Add(true).Add(true).Add(true);

    const int k_Right = 0;
    const int k_Left = 1;
    const int k_Up = 2;
    const int k_Down = 3;
    

    void Start(){
        rb = GetComponent<Rigidbody>();
        grid_offset = gridsize*5;
        // this.FixedUpdate();
        Vector3 spawn1 = new Vector3((float) grid_offset - 25, 10.0f,(float) -grid_offset+15);
        Vector3 spawn2 = new Vector3((float) -grid_offset + 15, 10.0f,(float) grid_offset - 15);
        Vector3 spawn3 = new Vector3((float) -grid_offset + 25 ,10.0f,(float)  grid_offset - 15);
        Vector3 spawn4 = new Vector3((float) -grid_offset + 15 ,10.0f,(float) -grid_offset+35);
        init_positions = new[] {spawn1,spawn2,spawn3,spawn4};
        int moveX = 0;
        int moveY = 0;
        sideChannel = new CustomSideChannel();
        SideChannelManager.RegisterSideChannel(sideChannel);
        sideChannel.MessageToPass += OnMessageReceived;
    }

    public void OnMessageReceived(object sendor,MessageEventArgs msg){
        string message = msg.message;
        Debug.Log(message);
    }

    public override void OnEpisodeBegin(){
        int init_idx = UnityEngine.Random.Range(0,4);
        Debug.Log(init_idx);
        transform.position = init_positions[init_idx];
    }


    // public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    // {
    //     // Mask the necessary actions if selected by the user.
    //     if (maskActions)
    //     {
    //         // Prevents the agent from picking an action that would make it collide with a wall
    //         var positionX = (int)transform.localPosition.x;
    //         var positionZ = (int)transform.localPosition.z;
    //         var maxPosition = (int)m_ResetParams.GetWithDefault("gridSize", 5f) - 1;

    //         if (positionX == 0)
    //         {
    //             actionMask.SetActionEnabled(0, k_Left, false);
    //         }

    //         if (positionX == maxPosition)
    //         {
    //             actionMask.SetActionEnabled(0, k_Right, false);
    //         }

    //         if (positionZ == 0)
    //         {
    //             actionMask.SetActionEnabled(0, k_Down, false);
    //         }

    //         if (positionZ == maxPosition)
    //         {
    //             actionMask.SetActionEnabled(0, k_Up, false);
    //         }
    //     }
    // }

    public override void OnActionReceived(ActionBuffers actions){
        int action = actions.DiscreteActions[0];
        sideChannel.SendStringToPython(action.ToString());
        if (action==k_Right){
            moveX = 1;
            moveZ = 0;
        }
        else if (action==k_Left){
            moveX=-1;
            moveZ = 0;
        }
        else if (action==k_Up){
            moveX = 0;
            moveZ=1;
        }
        else if (action==k_Down) {
            moveX = 0;
            moveZ=-1;
        }
        else {
            moveX = 0;
            moveZ = 0;
        }
        
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.position);
        sensor.AddObservation(targetTransform.position);
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Deadzone")){
        // moveSpeed = 0;
        // rb.velocity =  rb.velocity * 0.05f ;
                SetReward(-1.0f);
        EndEpisode();
        }
        if (other.gameObject.CompareTag("Goal")){
        SetReward(1.0f);
        EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        float xaxis = Input.GetAxisRaw("Horizontal");
        float zaxis = Input.GetAxisRaw("Vertical");
        if (xaxis>0){
            discreteActions[0] = 0;
        }
        else if (xaxis<0){
            discreteActions[0] = 1;
        }
        else if (zaxis > 0){
            discreteActions[0] = 2;
        }
        else if (zaxis <0) {
            discreteActions[0] = 3;
        }
        else{
            discreteActions[0] = UnityEngine.Random.Range(0,4);
        }
    }
}