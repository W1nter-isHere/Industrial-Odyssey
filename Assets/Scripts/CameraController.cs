using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float movementTime = 5;
    [SerializeField] private float zoomSpeed = 20;

    private Vector3 _newPosition;
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
        _newPosition = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _newPosition += Vector3.forward * movementSpeed;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _newPosition -= Vector3.forward * movementSpeed;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _newPosition -= Vector3.right * movementSpeed;
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _newPosition += Vector3.right * movementSpeed;
        }

        _newPosition += Vector3.down * (Input.mouseScrollDelta.y * zoomSpeed);
        _transform.Translate(Vector3.Lerp(_transform.position, _newPosition, Time.unscaledDeltaTime * movementTime));
    }
}