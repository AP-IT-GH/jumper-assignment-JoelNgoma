using UnityEngine;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
public class ObstacleManager : MonoBehaviour
{

    public static ObstacleManager Instance { get; private set; }
    public List<Transform> obstacles = new List<Transform>();
    private List<Rigidbody> obstacleRigidbodies = new List<Rigidbody>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Transform obstacle in obstacles)
        {
            Rigidbody rb = obstacle.GetComponent<Rigidbody>();
            if (rb != null)
            {
                obstacleRigidbodies.Add(rb);
            }
        }
    }

    public void ResetObstacles()
    {
        foreach (Transform obstacle in obstacles)
        {
            obstacle.localPosition = new Vector3(Random.Range(5f, 10f), 0.5f, 0f);
        }

        foreach (Rigidbody rb in obstacleRigidbodies)
        {
            rb.linearVelocity = Vector3.left * Random.Range(2f, 6f);
        }
    }

    public Vector3 GetClosestObstaclePosition()
    {
        Transform closest = null;
        float closestDistance = float.MaxValue;

        foreach (Transform obstacle in obstacles)
        {
            float distance = Vector3.Distance(obstacle.position, JumpAgentPosition());
            if (distance < closestDistance)
            {
                closest = obstacle;
                closestDistance = distance;
            }
        }

        return closest != null ? closest.position : Vector3.zero;
    }

    public float GetClosestObstacleSpeed()
    {
        Transform closest = null;
        float closestDistance = float.MaxValue;

        foreach (Transform obstacle in obstacles)
        {
            float distance = Vector3.Distance(obstacle.position, JumpAgentPosition());
            if (distance < closestDistance)
            {
                closest = obstacle;
                closestDistance = distance;
            }
        }

        if (closest != null)
        {
            Rigidbody rb = closest.GetComponent<Rigidbody>();
            if (rb != null)
            {
                return rb.linearVelocity.magnitude;

            }
        }

        return 0f;
    }

    private Vector3 JumpAgentPosition()
    {
        return FindObjectOfType<JumpAgent>().transform.position;
    }


}
