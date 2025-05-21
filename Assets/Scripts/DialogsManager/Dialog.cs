using System;
using UnityEngine;
using UnityEngine.UI;

namespace DialogsManager
{
    public class Dialog : MonoBehaviour, IDisposable
    {
        [SerializeField] private Button _outsideClickArea;

        protected virtual void Awake()
        {
            if (_outsideClickArea != null)
            {
                _outsideClickArea.onClick.AddListener(Hide);
            }
        }

        protected void Hide()
        {
            Dispose();
            Destroy(gameObject);
        }

        protected void OnDestroy()
        {
            if (_outsideClickArea != null)
            {
                _outsideClickArea.onClick.RemoveAllListeners();
            }
        }

        public virtual void Dispose()
        {
        
        }
    }
}
