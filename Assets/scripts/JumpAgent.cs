using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class JumpAgent : Agent
{
    private Rigidbody rb;
    public float jumpForce = 10f;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }


    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0, 0.5f, 0);
        rb.angularVelocity = Vector3.zero;
        ObstacleManager.Instance.ResetObstacles();
        
    }
    public override void CollectObservations(VectorSensor sensor) 
    {

        sensor.AddObservation(Vector3.Distance(transform.position, ObstacleManager.Instance.GetClosestObstaclePosition()));
        sensor.AddObservation(ObstacleManager.Instance.GetClosestObstacleSpeed());

    }
    public override void OnActionReceived(ActionBuffers actionBuffers) 
    {
        int action = actionBuffers.DiscreteActions[0];
        if(action == 1 && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle"))
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("spatie ingedrukt");
            discreteActions[0] = 1; // 1 = springen
        }
        else
        {
            discreteActions[0] = 0; // 0 = niets doen
        }
    }

}
