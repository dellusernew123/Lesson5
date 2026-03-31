using UnityEngine;

public class Mover : MonoBehaviour
{
    private CharacterController _characterController;

    private void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
    }

    public void Move(Vector3 direction, float speed)
    {
        _characterController.Move(direction * speed * Time.deltaTime);
    }

    public void ProcessRotateTo(Vector3 direction, float rotationSpeed, GameObject gameObject)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        float step = rotationSpeed * Time.deltaTime;

        gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, lookRotation, step);
    }
}
