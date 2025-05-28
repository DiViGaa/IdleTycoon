using EnterPoints;
using Interface;
using UnityEngine;

namespace Buildings
{
    public class BuildingClickHandler: IService
    {
        private UnityEngine.Camera mainCamera;

        public void Initialize()
        {
            MainGameEnterPoint.OnUpdate += OnUpdate;
            mainCamera = UnityEngine.Camera.main;
        }

        void OnUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider.TryGetComponent(out IInteractable interactable))
                    {
                        interactable.OnInteract();
                    }
                }
            }
        }
    }
}