using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    private float moveSpeed;
    private Vector3 moveDirection = Vector3.forward;
    private Vector3 endPosition;
    private bool isInitialized = false;

    public void Initialize(Vector3 direction, Vector3 targetEndPosition)
    {
        moveDirection = direction.normalized;
        moveSpeed = Random.Range(1f, 5f);
        endPosition = targetEndPosition;
        isInitialized = true;
    }

    void Update()
    {
        if (!isInitialized) return;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        float distanceToEnd = Vector3.Distance(transform.position, endPosition);

    if (distanceToEnd <= 0.11f)
    {
        Destroy(gameObject);
    }

    }
}