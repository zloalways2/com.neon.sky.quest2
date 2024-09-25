using UnityEngine;
using UnityEngine.UI;

namespace NSQ
{
    public class NSQSettings : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _NSQSettings;
        [SerializeField] private Button _NSQOpen;
        [SerializeField] private Button _NSQClose;
        
        [SerializeField] private Slider _NSQMusicBtn;
        [SerializeField] private Slider _NSQSoundBtn;

        [SerializeField] private AudioSource _NSQMusic;
        [SerializeField] private AudioSource _NSQSound;
        
        [SerializeField] private AudioClip _NSQClickClip;
        

        public void NSQCLICK() => _NSQSound.NSQPlayOneShot(_NSQClickClip);
        
        private void Awake()
        {
            Application.targetFrameRate = 120;
            
            NSQMusicControl();
            NSQSoundControl();
            
            NSQStaticUtil.NSQIsPause = false;
            
            _NSQSettings.NSQEnableCanvas(false);
            
            _NSQOpen.onClick.AddListener(() =>
            {
                NSQStaticUtil.NSQIsPause = true;
                _NSQSettings.NSQEnableCanvas(true);
            });
            _NSQClose.onClick.AddListener(() =>
            {
                NSQStaticUtil.NSQIsPause = false;
                _NSQSettings.NSQEnableCanvas(false);
            });
        }

        private void NSQMusicControl()
        {
            var music = PlayerPrefs.GetFloat("NSQMusic", 1);
            _NSQMusic.volume = music;
            _NSQMusicBtn.value = music;
            
            _NSQMusicBtn.onValueChanged.AddListener(NSQOnChange);

            return;

            void NSQOnChange(float nsqVal)
            {
                PlayerPrefs.SetFloat("NSQMusic", nsqVal);
                _NSQMusic.volume = nsqVal;
            }
        }
        
        private void NSQSoundControl()
        {
            var music = PlayerPrefs.GetFloat("NSQSound", 1);
            _NSQSound.volume = music;
            _NSQSoundBtn.value = music;
            
            _NSQSoundBtn.onValueChanged.AddListener(NSQOnChange);

            return;

            void NSQOnChange(float nsqVal)
            {
                PlayerPrefs.SetFloat("NSQSound", nsqVal);
                _NSQSound.volume = nsqVal;
            }
        }
    }
}