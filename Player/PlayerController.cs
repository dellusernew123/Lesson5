using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Mover _mover;

    private CharacterController _characterController;

    private float _deadZone = 0.1f;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Vertical"), 0, -Input.GetAxisRaw("Horizontal"));

        if (input.magnitude <= _deadZone)
            return;
            
        _mover.Move(input.normalized, _speed);
        _mover.ProcessRotateTo(input.normalized, _rotationSpeed, gameObject);
    }
}
