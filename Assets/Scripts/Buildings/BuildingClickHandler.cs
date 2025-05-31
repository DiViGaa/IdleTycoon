using System;
using EnterPoints;
using Interface;
using UnityEngine;

namespace Buildings
{
    public class BuildingClickHandler: IService, IDisposable
    {
        private UnityEngine.Camera _mainCamera;

        public void Initialize()
        {
            MainGameEnterPoint.OnUpdate += OnUpdate;
            _mainCamera = UnityEngine.Camera.main;
        }

        void OnUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider.TryGetComponent(out IInteractable interactable))
                    {
                        interactable.OnInteract();
                    }
                }
            }
        }

        public void Dispose()
        {
            MainGameEnterPoint.OnUpdate -= OnUpdate;
            _mainCamera = null;
        }
    }
}