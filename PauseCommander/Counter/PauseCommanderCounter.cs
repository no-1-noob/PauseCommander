using CountersPlus.Utils;
using HMUI;
using PauseCommander.Logic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using CountersPlus.ConfigModels;
using System.Threading.Tasks;

namespace PauseCommander.Counter
{
    internal class PauseCommanderCounter : CountersPlus.Counters.Custom.BasicCustomCounter
    {
#pragma warning disable CS0649
        [InjectOptional] private readonly PauseSongMgr pauseSongMgr;
#pragma warning restore
        private TMP_Text pauseText;
        private TMP_Text noPauseFound;
        private TMP_Text pauseIcon;
        private ImageView imgProgress;

        private float nextPauseTime = 0;

        public override void CounterInit()
        {
            var canvas = CanvasUtility.GetCanvasFromID(Settings.CanvasID);
            if(canvas != null)
            {
                HUDCanvas currentSettings = CanvasUtility.GetCanvasSettingsFromID(Settings.CanvasID);
                Vector2 ringAnchoredPos = (CanvasUtility.GetAnchoredPositionFromConfig(Settings) * currentSettings.PositionScale);
                imgProgress = CreateProgress(canvas);
                imgProgress.rectTransform.anchoredPosition = ringAnchoredPos;
                imgProgress.transform.localScale = Vector3.one * .10f;
                
                pauseText = CanvasUtility.CreateTextFromSettings(Settings);
                pauseText.text = string.Empty;
                pauseText.alignment = TextAlignmentOptions.CenterGeoAligned;

                noPauseFound = CanvasUtility.CreateTextFromSettings(Settings);
                noPauseFound.color = Color.red;
                noPauseFound.transform.localScale = Vector3.one * 2f;
                noPauseFound.transform.localPosition += new Vector3(0, 0.3f, 0);
                noPauseFound.text = "❌";

                pauseIcon = CanvasUtility.CreateTextFromSettings(Settings);
                pauseIcon.alpha = .5f;
                pauseIcon.transform.localScale = Vector3.one * 3f;
                pauseIcon.transform.localPosition += new Vector3(0, 1.5f, 0);
                pauseIcon.text = "⏸";

#if DEBUG
                //UpdateProgress(60, 30);
#endif
                imgProgress.gameObject.SetActive(false);
                pauseText.gameObject.SetActive(false);
                noPauseFound.gameObject.SetActive(false);
                pauseIcon.gameObject.SetActive(false);
            }
            if(pauseSongMgr != null)
            {
                pauseSongMgr.OnPauseActivated += PauseSongMgr_OnPauseActivated;
                pauseSongMgr.OnNextPauseUpdated += PauseSongMgr_OnNextPauseUpdated;
                pauseSongMgr.OnPauseDisabled += PauseSongMgr_OnPauseDisabled;
                pauseSongMgr.OnPauseActivatedFailed += PauseSongMgr_OnPauseActivatedFailed;
            }
        }

        public override void CounterDestroy()
        {
            if (pauseSongMgr != null)
            {
                pauseSongMgr.OnPauseActivated -= PauseSongMgr_OnPauseActivated;
                pauseSongMgr.OnNextPauseUpdated -= PauseSongMgr_OnNextPauseUpdated;
                pauseSongMgr.OnPauseDisabled -= PauseSongMgr_OnPauseDisabled;
                pauseSongMgr.OnPauseActivatedFailed -= PauseSongMgr_OnPauseActivatedFailed;
            }
        }

        private void PauseSongMgr_OnPauseActivated(object sender, float? e)
        {
            if (e.HasValue)
            {
                nextPauseTime = e.Value;
                imgProgress.gameObject.SetActive(true);
                pauseText.gameObject.SetActive(true);
                pauseIcon.gameObject.SetActive(true);
                noPauseFound.gameObject.SetActive(false);
            }
        }

        private void PauseSongMgr_OnNextPauseUpdated(object sender, float? e)
        {
            if (pauseText != null)
                UpdateProgress(nextPauseTime, e.HasValue ? e.Value : 0);
        }

        private void PauseSongMgr_OnPauseDisabled(object sender, System.EventArgs e)
        {
            imgProgress.gameObject.SetActive(false);
            pauseText.gameObject.SetActive(false);
            pauseIcon.gameObject.SetActive(false);
        }

        private void PauseSongMgr_OnPauseActivatedFailed(object sender, System.EventArgs e)
        {
            noPauseFound.gameObject.SetActive(true);
            _ = DisableNoPauseText();
        }

        private async Task DisableNoPauseText()
        {
            await Task.Delay(2000);
            noPauseFound.gameObject.SetActive(false);
        }

        private void UpdateProgress(float totalTimeLeft, float currentTimeLeft)
        {
            if(totalTimeLeft > 0)
            {
                float percent = (totalTimeLeft - currentTimeLeft) / totalTimeLeft;
                imgProgress.fillAmount = percent;
                pauseText.text = currentTimeLeft.ToString("N0");
            }
        }

        private ImageView CreateProgress(Canvas canvas)
        {
            //Circle definition from VisualScoreCounter ;)
            GameObject imageGameObject = new GameObject("Ring Image", typeof(RectTransform));
            imageGameObject.transform.SetParent(canvas.transform, false);
            ImageView newImage = imageGameObject.AddComponent<ImageView>();
            newImage.enabled = false;
            newImage.material = BeatSaberMarkupLanguage.Utilities.ImageResources.NoGlowMat;
            newImage.sprite = Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault(x => x.name == "Circle");
            newImage.type = Image.Type.Filled;
            newImage.fillClockwise = true;
            newImage.fillOrigin = 2;
            newImage.fillAmount = 1;
            newImage.fillMethod = Image.FillMethod.Radial360;
            newImage.enabled = true;
            return newImage;
        }
    }
}
