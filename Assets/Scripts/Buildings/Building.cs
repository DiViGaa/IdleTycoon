using System.Collections.Generic;
using UnityEngine;
using DialogsManager;
using DialogsManager.Dialogs;
using Upgrade;
using ServicesLocator;
using SoundManager;

namespace Buildings
{
    public class Building : MonoBehaviour, IInteractable
    {
        [SerializeField] private string buildingId;
        [SerializeField] private LayerMask allowedBuildLayers;
        [SerializeField] private Vector2Int size = Vector2Int.one;

        [SerializeField] private LayerMask removeOnStartLayers;

        public Vector2Int Size => size;
        public LayerMask AllowedBuildLayers => allowedBuildLayers;
        public string BuildingId => buildingId;

        protected BuildingUpgradeState upgradeState;
        protected UpgradeManager upgradeManager;

        private List<Renderer> _renderers = new List<Renderer>();
        private MaterialPropertyBlock _propertyBlock;

        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorProp = Shader.PropertyToID("_Color");

        private void Awake()
        {
            _renderers.AddRange(GetComponentsInChildren<Renderer>());
            _propertyBlock = new MaterialPropertyBlock();
        }

        public virtual void Start()
        {
            RemoveIntersectingObjects();

            upgradeManager = ServiceLocator.Current.Get<UpgradeManager>();
            upgradeState = upgradeManager.GetUpgrade(buildingId);
        }

        private void RemoveIntersectingObjects()
        {
            float height = 3f;

            Vector3 center = transform.position + new Vector3(size.x / 2f, height / 2f, size.y / 2f);
            Vector3 halfExtents = new Vector3(size.x / 2f, height / 2f, size.y / 2f);

            Collider[] overlappingColliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, removeOnStartLayers);

            foreach (var col in overlappingColliders)
            {
                if (col.gameObject == this.gameObject)
                    continue;

                Debug.Log($"Удаляем пересекающийся объект {col.gameObject.name} на слое {LayerMask.LayerToName(col.gameObject.layer)}");
                Destroy(col.gameObject);
            }
        }

        public int UpgradeLevel => upgradeState.Level;
        public int UpgradeCost => upgradeState.UpgradeCost;

        public bool TryUpgrade(int playerCoins)
        {
            if (upgradeManager.TryUpgrade(buildingId, playerCoins))
            {
                upgradeState = upgradeManager.GetUpgrade(buildingId);
                OnUpgraded();
                return true;
            }
            return false;
        }

        protected virtual void OnUpgraded()
        {
            Debug.Log($"{buildingId} upgraded to level {upgradeState.Level}");
        }

        public virtual string GetUpgradeInfo()
        {
            return $"Level: {UpgradeLevel}";
        }

        public void SetTransparent(bool available)
        {
            Color color = available ? Color.green : Color.red;
            ApplyColor(color);
        }

        public void SetNormal()
        {
            ApplyColor(Color.white);
        }

        private void ApplyColor(Color color)
        {
            foreach (var renderer in _renderers)
            {
                renderer.GetPropertyBlock(_propertyBlock);

                if (renderer.sharedMaterial.HasProperty(BaseColor))
                    _propertyBlock.SetColor(BaseColor, color);
                else if (renderer.sharedMaterial.HasProperty(ColorProp))
                    _propertyBlock.SetColor(ColorProp, color);

                renderer.SetPropertyBlock(_propertyBlock);
            }
        }

        private void OnDrawGizmos()
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Gizmos.color = (x + y) % 2 == 0
                        ? new Color(0.88f, 0f, 1f, 0.3f)
                        : new Color(1f, 0.68f, 0f, 0.3f);

                    Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, .1f, 1));
                }
            }
        }

        public virtual void OnInteract()
        {
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            var dialog = DialogManager.ShowDialog<BuildingUpgradeDialog>();
            dialog.Initialize(this);
        }
    }
}
