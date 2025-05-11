using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Collections.Generic;
// using System.Diagnostics;

public class AgentJump : Agent
{
    Rigidbody rb;
    public float jumpForce = 10f;
    bool isGrounded;
    public ObstacleDetector detector;
    bool waitingForDetection = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = true; 
    }
private List<GameObject> observedObstacles = new List<GameObject>();

public override void CollectObservations(VectorSensor sensor)
{
    sensor.AddObservation(isGrounded ? 1f : 0f);
    sensor.AddObservation(detector.ObstacleDetected ? 1f : 0f);

    for (int i = 0; i < observedObstacles.Count; i++)
    {
        if (observedObstacles[i] != null)
        {
            sensor.AddObservation(observedObstacles[i].transform.position);
        }
    }
}




    

void FixedUpdate()
{
    if (detector.CheckTimeFinished)
    {

        
        if (!isGrounded && detector.ObstacleDetected)
        {
            AddReward(1.0f);
            Debug.Log("Goede sprong over obstakel +1");
        }

        if (!isGrounded && !detector.ObstacleDetected)
        {
            AddReward(-0.2f); 
            Debug.Log("Onnodige sprong -0.2");
            
        }

        detector.ResetCheck();
    }
}


    void OnTriggerEnter(Collider other){
    if (other.CompareTag("Obstacle"))
    {
        AddReward(-0.1f); 
        Debug.Log("Niet gesprongen bij een obstacle -1");
        EndEpisode();
    }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Obstacle") && !isGrounded)
        {

            EndEpisode();     
        }   
  
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int jumpAction = actions.DiscreteActions[0];
            if (jumpAction == 1 && isGrounded)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f))
        {
            if (hit.collider.CompareTag("Ground")) 
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                detector.StartChecking();
            }
            else if (hit.collider.CompareTag("Obstacle"))
            {
                rb.AddForce(Vector3.up * (jumpForce * 0.5f), ForceMode.Impulse);
                detector.StartChecking();
            }
        }
    }
    }

        public override void OnEpisodeBegin()
    {
        rb.linearVelocity = Vector3.zero;
        detector.ResetCheck();
        observedObstacles.Clear();
        isGrounded = true;
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (var obstacle in obstacles)
        {
        observedObstacles.Add(obstacle);
        }
    }


}
