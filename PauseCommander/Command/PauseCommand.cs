using PauseCommander.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceCommander.Data;
using VoiceCommander.Interfaces;
using Zenject;

namespace PauseCommander.Command
{
    public class PauseCommand : IVoiceCommandHandler
    {
        [Inject] private readonly PauseSongMgr pauseMgr;

        public List<VoiceCommand> lsVoicecommand { get; } = new List<VoiceCommand>();

        public PauseCommand()
        {
            Plugin.Log.Error("PauseCommand added");
            lsVoicecommand.Add(new VoiceCommand("PauseCommander.PauseCommand.Immediate", "Immediate pause", UnityEngine.Windows.Speech.ConfidenceLevel.High, () => pauseMgr.ImmediatePause()));
            lsVoicecommand.Add(new VoiceCommand("PauseCommander.PauseCommand.Activate", "Activate pause", UnityEngine.Windows.Speech.ConfidenceLevel.High, () => pauseMgr.ActivatePause()));
            lsVoicecommand.Add(new VoiceCommand("PauseCommander.PauseCommand.Disable", "Disable pause", UnityEngine.Windows.Speech.ConfidenceLevel.High, () => pauseMgr.DisablePause()));
        }        
    }
}
