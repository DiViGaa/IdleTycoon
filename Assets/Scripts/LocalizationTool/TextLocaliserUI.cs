using TMPro;
using UnityEngine;

namespace LocalizationTool
{
    public class TextLocaliserUI : MonoBehaviour
    {
        [SerializeField] private string key;
        private TextMeshProUGUI _textMeshProUGUI;

        private void Start()
        {
            _textMeshProUGUI = gameObject.GetComponent<TextMeshProUGUI>();
            var value = LocalizationSystem.GetLocalizedString(key);
            _textMeshProUGUI.text = value;
        }

        public void SetKey(string key)
        {
            this.key = key;
        }
    }
}