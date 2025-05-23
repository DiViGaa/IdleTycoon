using System;
using EnterPoints;
using UnityEngine;

namespace Camera
{
    public class CameraMovement : MonoBehaviour, IDisposable
    {
        [SerializeField] private float dragSpeed = 2f;
        [SerializeField] private Vector2 xBounds = new Vector2(-60f, 60f);
        [SerializeField] private Vector2 zBounds = new Vector2(-100f, 65f);
        
        private Vector3 _dragOrigin;
        
        public void Initialize()
        {
            MainGameEnterPoint.OnUpdate += OnUpdate;
        }

        private void OnUpdate()
        {
            if (Input.GetMouseButtonDown(1))
            {
                _dragOrigin = Input.mousePosition;
                return;
            }

            if (!Input.GetMouseButton(1)) return;

            Vector3 difference = Input.mousePosition - _dragOrigin;
            Vector3 move = new Vector3(-difference.x, 0, -difference.y) * dragSpeed * Time.deltaTime;

            Vector3 newPosition = transform.position + move;

            newPosition.x = Mathf.Clamp(newPosition.x, xBounds.x, xBounds.y);
            newPosition.z = Mathf.Clamp(newPosition.z, zBounds.x, zBounds.y);

            transform.position = newPosition;

            _dragOrigin = Input.mousePosition;
        }

        public void Dispose()
        {
            MainGameEnterPoint.OnUpdate -= OnUpdate;
        }
    }
}