using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;
    
    private Vector3 _mPosition;
    private Vector3 _mDesiredPosition;
    
    private void FixedUpdate()
    {
       _mDesiredPosition = target.position + offset;
    }

    private void Update()
    {
        GetSmoothedPosition(_mDesiredPosition, out _mPosition);
    }

    private void GetSmoothedPosition(in Vector3 desiredPosition, out Vector3 smoothedPosition)
    {
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }

    private void LateUpdate()
    {
        transform.position = _mPosition;
    }
}
