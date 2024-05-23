using PauseCommander.Logic;
using System.Collections.Generic;
using VoiceCommander.Data;
using VoiceCommander.Interfaces;
using Zenject;

namespace PauseCommander.Command
{
    public class PauseCommand : IVoiceCommandHandler
    {
#pragma warning disable CS0649
        [Inject] private readonly PauseSongMgr pauseMgr;
#pragma warning restore

        public List<VoiceCommand> lsVoicecommand { get; } = new List<VoiceCommand>();

        public PauseCommand()
        {
            lsVoicecommand.Add(new VoiceCommand("PauseCommander.PauseCommand.Immediate", "Immediate pause", 0.9f, () => pauseMgr.ImmediatePause()));
            lsVoicecommand.Add(new VoiceCommand("PauseCommander.PauseCommand.Activate", "Activate pause", 0.9f, () => pauseMgr.ActivatePause()));
            lsVoicecommand.Add(new VoiceCommand("PauseCommander.PauseCommand.Disable", "Disable pause", 0.9f, () => pauseMgr.DisablePause()));
        }        
    }
}
