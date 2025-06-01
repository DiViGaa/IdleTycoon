using LocalizationTool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogsManager.Dialogs
{
    public class WelcomeDialog : Dialog
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button closeButton;

        private const string PlayerPrefsKey = "HasSeenWelcomeDialog";

        protected override void Awake()
        {
            base.Awake();
            messageText.text = LocalizationSystem.GetFormattedLocalizedString("welcomeMessage");
            closeButton.onClick.AddListener(Hide);
        }

        public static bool ShouldShow()
        {
            return !PlayerPrefs.HasKey(PlayerPrefsKey);
        }

        public override void Dispose()
        {
            PlayerPrefs.SetInt(PlayerPrefsKey, 1);
            PlayerPrefs.Save();
        }
    }
}