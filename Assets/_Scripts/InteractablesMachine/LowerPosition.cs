using UnityEngine;

public class LowerPosition : MonoBehaviour
{
    [SerializeField] private float lowerAmount = 2f;
    [SerializeField] private float moveSpeed = 3f;

    private Vector3 originalPosition;
    private Vector3 loweredPosition;
    private Vector3 targetPosition;

    private bool isLowered = false;
    private bool isMoving = false;

    void Start()
    {
        originalPosition = transform.position;
        loweredPosition = originalPosition + Vector3.down * lowerAmount;
        targetPosition = originalPosition;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }

    // Flip-flop event
    public void TogglePosition()
    {
        isLowered = !isLowered;
        targetPosition = isLowered ? loweredPosition : originalPosition;
        isMoving = true;
    }

    // Failsafe reset event
    public void ResetPosition()
    {
        isLowered = false;
        targetPosition = originalPosition;
        isMoving = true;
    }
}
