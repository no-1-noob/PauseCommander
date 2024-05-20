using System.Collections.Generic;
using VoiceCommander.Data;
using VoiceCommander.Interfaces;

namespace PauseCommander.Command
{
    public class PauseCommandMainMenu : IVoiceCommandHandler
    {

        public List<VoiceCommand> lsVoicecommand { get; } = new List<VoiceCommand>();

        public PauseCommandMainMenu()
        {
            lsVoicecommand.Add(new VoiceCommand("PauseCommander.PauseCommandMainMenu.Test", "Test 1", 0.9f, () => Plugin.Log.Error("PauseCommandMainMenu Test1")));
            lsVoicecommand.Add(new VoiceCommand("PauseCommander.PauseCommandMainMenu.Test", "Test 2", 0.9f, () => Plugin.Log.Error("PauseCommandMainMenu Test1")));
            lsVoicecommand.Add(new VoiceCommand("PauseCommander.PauseCommandMainMenu.Test2", "Test 2", 0.9f, () => Plugin.Log.Error("PauseCommandMainMenu Test1")));
        }

        
    }
}
