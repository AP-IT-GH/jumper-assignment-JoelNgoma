using UnityEngine;
public class ObstacleDetector : MonoBehaviour
{
    public bool ObstacleDetected { get; private set; }
    public bool CheckTimeFinished { get; private set; }

    private float checkDuration = 0.8f;
    private float timer = 0f;
    private bool checking = false;

    public void StartChecking()
    {
        ObstacleDetected = false;
        CheckTimeFinished = false;
        timer = 0f;
        checking = true;
    }

    public void ResetCheck()
    {
        checking = false;
        ObstacleDetected = false;
        CheckTimeFinished = false;
    }

    void Update()
    {
    Debug.Log(ObstacleDetected);

        if (checking)
        {
            timer += Time.deltaTime;
            if (timer >= checkDuration)
            {
                CheckTimeFinished = true;
                checking = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log(other);
            ObstacleDetected = true;
        }
    }
}
