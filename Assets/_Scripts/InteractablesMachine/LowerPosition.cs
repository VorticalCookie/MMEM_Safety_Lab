using UnityEngine;


public class LowerPosition : MonoBehaviour
{
    [SerializeField] private float lowerAmount = 2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float lowerXDegrees = 0f; // How much to lower X rotation

    private Vector3 originalPosition;
    private Vector3 loweredPosition;
    private Vector3 targetPosition;

    private Quaternion originalRotation;
    private Quaternion targetRotation;

    private bool isLowered = false;
    private bool isMoving = false;

    void Start()
    {
        originalPosition = transform.position;
        loweredPosition = originalPosition + Vector3.down * lowerAmount;
        targetPosition = originalPosition;

        originalRotation = transform.rotation;
        if (isMoving)
        {
            // Move position
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

          

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f &&
                Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
            {
                transform.position = targetPosition;
                transform.rotation = targetRotation;
                isMoving = false;
            }
        }
        targetRotation = originalRotation;
    }

    void Update()
    {
        if (isMoving)
        {
            // Move position
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            // Lerp rotation
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                moveSpeed * 30f * Time.deltaTime // Adjust rotation speed as needed
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f &&
                Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
            {
                transform.position = targetPosition;
                transform.rotation = targetRotation;
                isMoving = false;
            }
        }
    }

    // Flip-flop event
    public void TogglePosition()
    {
        isLowered = !isLowered;
        targetPosition = isLowered ? loweredPosition : originalPosition;

        if (isLowered)
        {
            // Lower X by lowerXDegrees from the original rotation
            Vector3 loweredEuler = originalRotation.eulerAngles;
            loweredEuler.x -= lowerXDegrees;
            targetRotation = Quaternion.Euler(loweredEuler);
        }
        else
        {
            targetRotation = originalRotation;
        }

        isMoving = true;
    }

    // Failsafe reset event
    public void ResetPosition()
    {
        isLowered = false;
        targetPosition = originalPosition;
        targetRotation = originalRotation;
        isMoving = true;
    }
}
