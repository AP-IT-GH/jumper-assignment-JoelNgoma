using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class AgentJump : Agent
{
    Rigidbody rb;
    public float jumpForce = 8f;
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

        Debug.Log($"[CHECK] isGrounded: {isGrounded}, ObstacleDetected: {detector.ObstacleDetected}, isAgentStill: {isAgentStill}");

        if (!isGrounded && detector.ObstacleDetected)
        {
            AddReward(1.0f);
            Debug.Log("Goede sprong over obstakel +1");
        }

        if (isGrounded && detector.ObstacleDetected)
        {
            AddReward(-1.0f); 
            Debug.Log("Niet gesprongen bij obstakel -1");
            EndEpisode();
        }
        if (!isGrounded && !detector.ObstacleDetected)
        {
            AddReward(-0.2f); 
            Debug.Log("Onnodige sprong -0.2");
        }

        detector.ResetCheck();
    }
}





    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Collision Enter - Grounded: True");
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-1f);
            Debug.Log("Obstacle geraakt");
            EndEpisode();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("Collision Exit - Grounded: False");
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
            Debug.Log("Jump Action Received and Executed!");
            detector.StartChecking();
        }
    }

        public override void OnEpisodeBegin()
    {
        rb.linearVelocity = Vector3.zero;
        detector.ResetCheck();
    }


}
