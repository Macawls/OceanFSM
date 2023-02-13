using UnityEngine;

namespace OceanFSM.PlayerExample
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] public Transform targetTransform;
        [SerializeField] private float distance = 4;
        [SerializeField] private float height = 1.5f;
        [SerializeField] private float smoothTime = 0.25f;
       
        private Vector3 _mCurrentVelocity;

        private void LateUpdate()
        {
            var direction = targetTransform.transform.forward * -1;
            var target = targetTransform.position + new Vector3(0, height, 0) + direction * distance;
            
            transform.position = Vector3.SmoothDamp(transform.position, target, ref _mCurrentVelocity, smoothTime);
            transform.LookAt(targetTransform);
        }
    }
}