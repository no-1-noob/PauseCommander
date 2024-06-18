using PauseCommander.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;
using UnityEngine.XR;

namespace PauseCommander.Logic
{
    internal class PauseSongMgr : IInitializable, IDisposable, ITickable
    {
#pragma warning disable CS0649
        [Inject] private readonly AudioTimeSyncController audioTimeSyncController;
        [Inject] private readonly BeatmapObjectManager beatmapObjectManager;
        [Inject] private readonly GameplayCoreSceneSetupData setupData;
        [Inject] private readonly PauseController pauseController;
#pragma warning restore

        public event EventHandler<float?> OnNextPauseUpdated;
        public event EventHandler<float?> OnPauseActivated;
        public event EventHandler OnPauseDisabled;
        public event EventHandler OnPauseActivatedFailed;

        List<Pause> lsPause = new List<Pause>();
        int noteDone = 0;
        Pause nextPause = null;

        float pauseDelay = 0.5f; //Delay for after swing

        public void Initialize()
        {
            beatmapObjectManager.noteWasCutEvent += BeatmapObjectManager_noteWasCutEvent;
            beatmapObjectManager.noteWasMissedEvent += BeatmapObjectManager_noteWasMissedEvent;
            pauseController.didPauseEvent += PauseController_didPauseEvent;
            _ = GetPausesFromSong();
            InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;
        }

        public void Dispose()
        {
            ClearPauses();
            beatmapObjectManager.noteWasCutEvent -= BeatmapObjectManager_noteWasCutEvent;
            beatmapObjectManager.noteWasMissedEvent -= BeatmapObjectManager_noteWasMissedEvent;
            pauseController.didPauseEvent -= PauseController_didPauseEvent;
            InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected;
        }        

        #region events
        internal void ImmediatePause()
        {
            pauseController?.Pause();
        }

        internal void ActivatePause()
        {
            Pause nextPause = lsPause.Find(x => x.Start >= audioTimeSyncController.songTime);
            if (nextPause != null)
            {
                this.nextPause = nextPause;
                OnPauseActivated?.Invoke(this, nextPause.Start + pauseDelay - audioTimeSyncController.songTime);
            }
            else
            {
                OnPauseActivatedFailed?.Invoke(this, null);
            }
        }

        internal void DisablePause()
        {
            nextPause = null;
            OnPauseDisabled?.Invoke(this, null);
        }
        #endregion

        #region Pause setup
        internal async Task GetPausesFromSong()
        {
            DateTime dtStart = DateTime.Now;
            List<NoteData> lsNoteData = GetNoteData(setupData.transformedBeatmapData);
            lsPause = GetPauses(lsNoteData, Configuration.PluginConfig.Instance.MinPauseLength, pauseDelay);
        }

        internal void ClearPauses()
        {
            lsPause = new List<Pause>();
        }
        #endregion

        private static List<NoteData> GetNoteData(IReadonlyBeatmapData beatmapData)
        {
            List<NoteData> lsNotes = beatmapData.GetBeatmapDataItems<NoteData>(0).ToList();
            lsNotes = lsNotes.Where(x => x.colorType != ColorType.None).ToList();
            return lsNotes;
        }

        private static List<Pause> GetPauses(List<NoteData> lsNotes, float pauseLength, float pauseDelay)
        {
            lsNotes = lsNotes.OrderBy(x => x.time).ToList();
            List<Pause> lsPauses = new List<Pause>();
            float lastNoteTime = 0;
            int count = 0;
            foreach (NoteData note in lsNotes)
            {
                float possPauseLength = note.time - lastNoteTime;
                if ((possPauseLength) >= (pauseLength + pauseDelay))
                {
                    lsPauses.Add(new Pause(lastNoteTime, possPauseLength, count));
                }
                lastNoteTime = note.time;
                count++;
            }
            return lsPauses;
        }

        public void Tick()
        {
            if (nextPause != null && nextPause.Start + pauseDelay <= audioTimeSyncController?.songTime && noteDone >= nextPause.NoteCount)
            {
                pauseController?.Pause();
                nextPause = null;
                OnPauseDisabled?.Invoke(this, null);
            }
            OnNextPauseUpdated?.Invoke(this, nextPause != null ? nextPause.Start + pauseDelay - audioTimeSyncController?.songTime : null);
        }

        #region events
        private void PauseController_didPauseEvent()
        {
            DisablePause();
        }
        private void InputDevices_deviceDisconnected(InputDevice inputDevice)
        {
            if (Configuration.PluginConfig.Instance.IsControllerDisconnectPauseActive && inputDevice.characteristics.HasFlag(InputDeviceCharacteristics.Controller))
            {
                ImmediatePause();
            }
        }
        private void PauseCoreMgr_OnActivatePause(object sender, EventArgs e)
        {
            ActivatePause();
        }

        private void PauseCoreMgr_OnDisablePause(object sender, EventArgs e)
        {
            DisablePause();
        }

        private void PauseCoreMgr_OnImmediatePause(object sender, EventArgs e)
        {
            ImmediatePause();
        }

        private void BeatmapObjectManager_noteWasMissedEvent(NoteController noteController)
        {
            CheckNotesLeft(noteController);
        }

        private void BeatmapObjectManager_noteWasCutEvent(NoteController noteController, in NoteCutInfo noteCutInfo)
        {
            CheckNotesLeft(noteController);
        }

        private void CheckNotesLeft(NoteController noteController)
        {
            if(noteController.noteData.colorType != ColorType.None)
            {
                noteDone++;
            }
        }
        #endregion
    }
}
