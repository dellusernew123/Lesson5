using UnityEngine;

public class Mover : MonoBehaviour
{
    private CharacterController _characterController;
    public void Move(GameObject gameObject, Vector3 direction, float speed, float rotationSpeed)
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        _characterController.Move(direction * speed * Time.deltaTime);
        ProcessRotateTo(direction, rotationSpeed, gameObject);
    }

    private void ProcessRotateTo(Vector3 direction, float rotationSpeed, GameObject gameObject)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        float step = rotationSpeed * Time.deltaTime;

        gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, lookRotation, step);
    }
}
