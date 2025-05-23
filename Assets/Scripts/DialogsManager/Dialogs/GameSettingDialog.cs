namespace DialogsManager.Dialogs
{
    public class GameSettingDialog : SettingDialog
    {
        protected override void ReloadSettings()
        {
            var dialog = DialogManager.ShowDialog<GameSettingDialog>();
            dialog.Initialize();
            base.ReloadSettings();
        }

        protected override void BackToMenu()
        {
            var dialog = DialogManager.ShowDialog<GamePauseDialog>();
            dialog.Initialize();
            base.BackToMenu();
        }
        
    }
}
