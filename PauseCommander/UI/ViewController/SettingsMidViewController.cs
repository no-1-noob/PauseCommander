using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using System.ComponentModel;

namespace PauseCommander.UI.ViewController
{
    [HotReload(RelativePathToLayout = @"SettingsMidViewController.bsml")]
    [ViewDefinition("PauseCommander.UI.View.SettingsMidView.bsml")]
    internal class SettingsMidViewController : BSMLAutomaticViewController, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler PropertyChanged;

        [UIValue("min-pause-length")]
        public float MinPauseLength
        {
            get => Configuration.PluginConfig.Instance.MinPauseLength;
            set
            {
                Configuration.PluginConfig.Instance.MinPauseLength = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinPauseLength)));
            }
        }

        [UIValue("activate-controller-disconnect")]
        public bool ActivateControllerDisconnect
        {
            get => Configuration.PluginConfig.Instance.IsControllerDisconnectPauseActive;
            set
            {
                Configuration.PluginConfig.Instance.IsControllerDisconnectPauseActive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActivateControllerDisconnect)));
            }
        }
    }
}
