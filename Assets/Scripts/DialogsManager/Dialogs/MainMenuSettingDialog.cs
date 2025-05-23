namespace DialogsManager.Dialogs
{
    public class MainMenuSettingDialog : SettingDialog
    {
        protected override void ReloadSettings()
        {
            var dialog = DialogManager.ShowDialog<MainMenuSettingDialog>();
            dialog.Initialize();
            base.ReloadSettings();
        }

        protected override void BackToMenu()
        {
            var dialog = DialogManager.ShowDialog<MainMenuDialog>();
            dialog.Initialize();
            base.BackToMenu();
        }
    }
}
