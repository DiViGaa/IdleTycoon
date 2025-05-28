using Buildings;
using Player;
using UnityEngine;
using UnityEngine.UI;
using ServicesLocator;
using TMPro;

namespace DialogsManager.Dialogs
{
    public class BuildingUpgradeDialog : Dialog
    {
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button closeButton;

        private BuildingBase _building;
        private Player.Player _player;

        public void Initialize(BuildingBase building)
        {
            _building = building;
            _player = ServiceLocator.Current.Get<Player.Player>();

            UpdateUI();

            upgradeButton.onClick.AddListener(OnUpgradeClick);
            closeButton.onClick.AddListener(Close);
        }

        private void UpdateUI()
        {
            infoText.text = _building.GetUpgradeInfo();
            costText.text = $"Upgrade Cost: {_building.UpgradeCost}";
        }

        private void OnUpgradeClick()
        {
            if (_player.HasEnough(ResourceType.Coins, _building.UpgradeCost))
            {
                _player.ChangeResource(ResourceType.Coins, -_building.UpgradeCost);
                _building.TryUpgrade((int)_player.GetResource(ResourceType.Coins));
                UpdateUI();
            }
            else
            {
                Debug.Log("Not enough coins.");
            }
        }

        private void Close()
        {
            Hide();
        }

        public override void Dispose()
        {
            upgradeButton.onClick.RemoveListener(OnUpgradeClick);
            closeButton.onClick.RemoveListener(Close);
            base.Dispose();
        }
    }
}