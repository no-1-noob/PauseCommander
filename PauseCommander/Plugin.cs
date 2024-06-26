﻿using IPA;
using IPALogger = IPA.Logging.Logger;
using SiraUtil.Zenject;
using PauseCommander.Installers;
using IPA.Config.Stores;
using BeatSaberMarkupLanguage;
using HMUI;
using PauseCommander.UI;
using BeatSaberMarkupLanguage.MenuButtons;
using SimpleUpdateChecker.Plugin;

namespace PauseCommander
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    internal class Plugin : SimpleUpdatePlugin
    {
        internal static Plugin Instance { get; private set; }
        internal static FlowCoordinator ParentFlow { get; private set; }
        private static PauseCommanderFlowCoordinator _flow;

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger, Zenjector zenjector, IPA.Config.Config conf)
        {
            base.CreateSimpleUpdateChecker(logger, zenjector, "https://mods.no1noob.net/api/PauseCommander_1_29", "https://github.com/no-1-noob/PauseCommander/releases");
            Instance = this;
            Log.Info("PauseCommander initialized.");
            if(conf != null)
            {
                Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            }
            zenjector.Install<PauseSongInstaller>(Location.StandardPlayer | Location.CampaignPlayer);
            MenuButtons.instance.RegisterButton(new MenuButton("Pause Commander", "Pause the game with your voice", ShowSettingsFlow, true));
        }

        [OnStart]
        public void OnApplicationStart()
        {
        }

        [OnExit]
        new public void OnApplicationQuit()
        {
        }

        private static void ShowSettingsFlow()
        {
            if (_flow == null)
                _flow = BeatSaberUI.CreateFlowCoordinator<PauseCommanderFlowCoordinator>();

            ParentFlow = BeatSaberUI.MainFlowCoordinator.YoungestChildFlowCoordinatorOrSelf();

            BeatSaberUI.PresentFlowCoordinator(ParentFlow, _flow, immediately: true);
        }
    }
}
