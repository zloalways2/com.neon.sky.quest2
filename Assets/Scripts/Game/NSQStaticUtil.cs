using DG.Tweening;
using UnityEngine;

namespace NSQ
{
    public static class NSQStaticUtil
    {
        public static bool NSQIsGameOver;
        public static bool NSQIsPause;
        
        public static void NSQPlayOneShot(this AudioSource source, AudioClip clip, bool nsqpitch = false)
        {
            source.pitch = nsqpitch ? Random.Range(0.8f, 1.2f) : 1f;
            
            source.PlayOneShot(clip);
        }
        
        public static void NSQEnableCanvas(this CanvasGroup cg, bool val)
        {
            cg.alpha = val ? 1f : 0f;
            cg.blocksRaycasts = val;
            
            if (val)
                cg.interactable = val;
            else
                DOTween.Sequence().AppendInterval(0.1f).OnComplete(() => cg.interactable = val);
        }
        
        public static bool NSQStopUpdate => NSQIsGameOver || NSQIsPause;

        public static string NSQToTimeString(this float val)
        {
            var minutes = Mathf.FloorToInt(val / 60);
            var seconds = Mathf.FloorToInt(val % 60);
        
            return $"{minutes:00}:{seconds:00}";
        }
    }
}