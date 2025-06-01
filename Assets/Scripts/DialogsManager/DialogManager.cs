using System;
using System.Collections.Generic;
using DialogsManager.Dialogs;
using ServicesLocator;
using UnityEngine;

namespace DialogsManager
{
    public class DialogManager
    {
        private const string PrefabsFilePath = "Prefabs/UI/";
        
        private static readonly Dictionary<Type, string> PrefabsDictionary = new Dictionary<Type, string>()
        {
            {typeof(MainMenuSettingDialog), "MainMenuSettingDialog"},
            {typeof(BuildingUpgradeDialog), "BuildingUpgradeDialog"},
            {typeof(MainGameShopDialog), "MainGameShopDialog"},
            {typeof(GameSettingDialog), "GameSettingDialog"},
            {typeof(GamePauseDialog), "GamePauseDialog"},
            {typeof(MainMenuDialog), "MainMenuDialog"},
            {typeof(GameUIDialog), "GameUIDialog"},
            {typeof(WelcomeDialog), "WelcomeDialog"},
        };

        public static T ShowDialog<T>() where T : Dialog
        {
            var go = GetPrefabByType<T>();
            if (go == null)
            {
                Debug.LogError("Show window - object not found");
                return null;
            }
            
            return GameObject.Instantiate(go, GuiHolder);
        }

        private static T GetPrefabByType<T>() where T : Dialog
        {
            var prefabName =  PrefabsDictionary[typeof(T)];
            if (string.IsNullOrEmpty(prefabName))
            {
                Debug.LogError("cant find prefab type of " + typeof(T) + "Do you added it in PrefabsDictionary?");
            }

            var path = PrefabsFilePath + PrefabsDictionary[typeof(T)];
            var dialog = Resources.Load<T>(path);
            if (dialog == null)
            {
                Debug.LogError("Cant find prefab at path " + path);
            }

            return dialog;
        }
        
        public static Transform GuiHolder
        {
            get { return ServiceLocator.Current.Get<GUIHolder>().transform; }
        }
    }
}