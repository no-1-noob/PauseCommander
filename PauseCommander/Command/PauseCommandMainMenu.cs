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
    public class PauseCommandMainMenu : IVoiceCommandHandler
    {

        public List<VoiceCommand> lsVoicecommand { get; } = new List<VoiceCommand>();

        public PauseCommandMainMenu()
        {
            Plugin.Log.Error("PauseCommandMainMenu added");
            lsVoicecommand.Add(new VoiceCommand("PauseCommander.PauseCommandMainMenu.Test", "Test 1", UnityEngine.Windows.Speech.ConfidenceLevel.High, () => Plugin.Log.Error("PauseCommandMainMenu Test1")));
        }

        
    }
}
