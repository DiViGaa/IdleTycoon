using Interface;
using Player;
using UnityEngine;
using System;

namespace NPC
{
    public class LogisticsNPC : MonoBehaviour
    {
        private ResourceType _resource;
        private float _amount;

        private Vector3 _startPos;
        private Vector3 _targetPos;

        private Action _onDelivered;

        private NPCMover _mover;

        public bool IsReturned { get; private set; }

        public void Setup(ResourceType resource, float amount, Vector3 start, Vector3 target, Action onDelivered)
        {
            _resource = resource;
            _amount = amount;
            _startPos = start;
            _targetPos = target;
            _onDelivered = onDelivered;

            transform.position = _startPos;
            IsReturned = false;

            _mover = GetComponent<NPCMover>();
            _mover.MoveTo(_targetPos, OnArrivedAtTarget);
        }

        private void OnArrivedAtTarget()
        {
            _onDelivered?.Invoke();

            _mover.MoveTo(_startPos, OnReturnedToBase);
        }

        private void OnReturnedToBase()
        {
            IsReturned = true;
            Destroy(gameObject);
        }
    }
}