using JSON;
using Player;
using ServicesLocator;
using SoundManager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DialogsManager.Dialogs
{
    public class GameUIDialog : Dialog
    {
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button shopButton;

        [SerializeField] private TextMeshProUGUI cenoxiumTextMeshProUGUI;
        [SerializeField] private TextMeshProUGUI crysalorTextMeshProUGUI;
        [SerializeField] private TextMeshProUGUI thaloriteTextMeshProUGUI;
        [SerializeField] private TextMeshProUGUI velorithTextMeshProUGUI;
        [SerializeField] private TextMeshProUGUI zenthiteTextMeshProUGUI;
        [SerializeField] private TextMeshProUGUI coinTextMeshProUGUI;

        public static UnityAction UpdateUI;


        public void Initialize()
        {

            pauseButton.onClick.AddListener(ShowPauseDialog);
            shopButton.onClick.AddListener(ShowShopDialog);

            UpdateUI += UpdateResourceUI;
            UpdateResourceUI();
        }

        private void ShowShopDialog()
        {
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            var dialog = DialogManager.ShowDialog<MainGameShopDialog>();
            dialog.Initialize();
            Hide();
        }

        private void ShowPauseDialog()
        {
            ServiceLocator.Current.Get<AudioManager>().PlaySound("ui", "UI");
            var dialog = DialogManager.ShowDialog<GamePauseDialog>();
            dialog.Initialize();
            Hide();
        }

        private void UpdateResourceUI()
        {
            cenoxiumTextMeshProUGUI.text = ServiceLocator.Current.Get<Player.Player>().GetResource(ResourceType.Cenoxium).ToString("F0");
            crysalorTextMeshProUGUI.text = ServiceLocator.Current.Get<Player.Player>().GetResource(ResourceType.Crysalor).ToString("F0");
            thaloriteTextMeshProUGUI.text = ServiceLocator.Current.Get<Player.Player>().GetResource(ResourceType.Thalorite).ToString("F0");
            velorithTextMeshProUGUI.text = ServiceLocator.Current.Get<Player.Player>().GetResource(ResourceType.Velorith).ToString("F0");
            zenthiteTextMeshProUGUI.text = ServiceLocator.Current.Get<Player.Player>().GetResource(ResourceType.Zenthite).ToString("F0");
            coinTextMeshProUGUI.text = ServiceLocator.Current.Get<Player.Player>().GetResource(ResourceType.Coins).ToString("F0");
        }

        public override void Dispose()
        {
            pauseButton.onClick.RemoveAllListeners();
            shopButton.onClick.RemoveAllListeners();
            UpdateUI -= UpdateResourceUI;
            base.Dispose();
        }
    }
}
