using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

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
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            detector.StartChecking();
        }
    }

        public override void OnEpisodeBegin()
    {
        rb.linearVelocity = Vector3.zero;
        detector.ResetCheck();
    }


}
