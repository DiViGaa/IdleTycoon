using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace NPC
{
    public class NPCMover : MonoBehaviour
    {
        [SerializeField] private float speed = 3f;
        [SerializeField] private float rayDistance = 1.5f;
        [SerializeField] private float sideOffset = 0.75f;
        [SerializeField] private float arrivalThreshold = 0.5f;
        [SerializeField] private float npcAvoidanceRadius = 1.2f;
        [SerializeField] private float npcAvoidanceStrength = 1.0f;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private LayerMask npcLayer;
        [SerializeField] private Animator animator;

        private Vector3 _targetPosition;
        private bool _isMoving = false;
        private Action _onArrived;

        private const float MaxMoveTime = 10f;
        
        public void MoveTo(Vector3 targetPosition, Action onArrived)
        {
            _targetPosition = targetPosition;
            _onArrived = onArrived;

            if (!_isMoving)
                StartCoroutine(MoveRoutine());
        }

        private IEnumerator MoveRoutine()
        {
            _isMoving = true;
            float elapsed = 0f;

            if (animator != null)
                animator.SetBool("isMoving", true);

            while (!IsAtTarget() && elapsed < MaxMoveTime)
            {
                Vector3 direction = (_targetPosition - transform.position).normalized;
                Vector3 moveDirection = direction;

                bool centerBlocked = Physics.Raycast(transform.position, direction, rayDistance, obstacleMask);

                Vector3 leftOrigin = transform.position - transform.right * sideOffset;
                bool leftBlocked = Physics.Raycast(leftOrigin, direction, rayDistance, obstacleMask);

                Vector3 rightOrigin = transform.position + transform.right * sideOffset;
                bool rightBlocked = Physics.Raycast(rightOrigin, direction, rayDistance, obstacleMask);

                if (centerBlocked)
                {
                    if (!leftBlocked)
                        moveDirection = Quaternion.Euler(0, -45, 0) * direction;
                    else if (!rightBlocked)
                        moveDirection = Quaternion.Euler(0, 45, 0) * direction;
                    else
                        moveDirection = -direction;
                }

                Collider[] nearbyNPCs = Physics.OverlapSphere(transform.position, npcAvoidanceRadius, npcLayer);
                foreach (var npc in nearbyNPCs)
                {
                    if (npc.gameObject == gameObject) continue;

                    Vector3 away = (transform.position - npc.transform.position).normalized;
                    moveDirection += away * npcAvoidanceStrength;
                }

                moveDirection = moveDirection.normalized;

                if (moveDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                }

                transform.position += moveDirection * speed * Time.deltaTime;

                elapsed += Time.deltaTime;
                yield return null;
            }

            if (animator != null)
                animator.SetBool("isMoving", false);

            if (elapsed >= MaxMoveTime)
                Debug.LogWarning($"{name}: NPCMover.MoveRoutine — таймаут движения достигнут.");

            _isMoving = false;
            _onArrived?.Invoke();
        }

        private bool IsAtTarget()
        {
            Vector2 currentXZ = new Vector2(transform.position.x, transform.position.z);
            Vector2 targetXZ = new Vector2(_targetPosition.x, _targetPosition.z);
            return Vector2.Distance(currentXZ, targetXZ) <= arrivalThreshold;
        }

        public bool IsMoving => _isMoving;
    }
}
