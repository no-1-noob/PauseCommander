using BeatSaberMarkupLanguage;
using HMUI;
using PauseCommander.UI.ViewController;

namespace PauseCommander.UI
{
    internal class PauseCommanderFlowCoordinator : FlowCoordinator
    {
        private static PauseCommanderFlowCoordinator instance = null;
        private static SettingsMidViewController settingsMidView;
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            instance = this;
            SetTitle("Pause Commander");
            showBackButton = true;
            settingsMidView = BeatSaberUI.CreateViewController<SettingsMidViewController>();
            ProvideInitialViewControllers(mainViewController: settingsMidView);
        }

        protected override void BackButtonWasPressed(HMUI.ViewController topViewController) => Close();

        private void Close()
        {
            Plugin.ParentFlow.DismissFlowCoordinator(instance, () => {
                instance = null;
            }, immediately: true);
        }
    }
}
