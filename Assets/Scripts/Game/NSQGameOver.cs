using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NSQ
{
    public class NSQGameOver : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _NSQWinBG;
        [SerializeField] private CanvasGroup _NSQGame;
        [SerializeField] private CanvasGroup _NSQWin;
        [SerializeField] private GameObject _NSQParticle;
        [SerializeField] private List<Button> _NSQHome;
        [SerializeField] private Button[] _NSQRestart;
        [SerializeField] private TMP_Text[] _NSQLevel;
        [SerializeField] private TMP_Text _NSQTimer;
        [SerializeField] private CanvasGroup _NSQLose;
        [SerializeField] private TMP_Text _NSQScore;
        
        [Space]
        public AudioClip _NSQWinClip;
        public AudioClip _NSQLoseClip;
        public AudioSource _NSQSfx;
        public AudioSource _NSQMusic;

        private float _nsqTimer = 60f;
        
        private int _nsqLevel;

        private void Start()
        {
            _NSQTimer.text = _nsqTimer.NSQToTimeString();

            foreach (var nsqLvl in _NSQLevel)
            {
                _nsqLevel = PlayerPrefs.GetInt("NSQLevel", 1);
                nsqLvl.text = $"Level {_nsqLevel}";
            }
            
            _NSQGame.NSQEnableCanvas(true);
            _NSQWin.NSQEnableCanvas(false);
            _NSQLose.NSQEnableCanvas(false);

            foreach (var nsqButton in _NSQRestart)
                nsqButton.onClick.AddListener(()=> SceneManager.LoadScene("Game"));
            
            foreach (var nsqButton in _NSQHome)
                nsqButton.onClick.AddListener(() =>
                {
                    NSQStaticUtil.NSQIsGameOver = true;

                    SceneManager.LoadScene("Menu");
                });
            
            NSQStaticUtil.NSQIsGameOver = false;
        }

        private void Update()
        {
            if (NSQStaticUtil.NSQStopUpdate)
                return;
            
            _nsqTimer -= Time.deltaTime;

            _NSQTimer.text = $"Time: {_nsqTimer.NSQToTimeString()}";

            if (_nsqTimer > 0) 
                return;
            
            _NSQTimer.text = $"Time: {_nsqTimer.NSQToTimeString()}";
            
            NSQLose();
        }

        public void NSQWin(int score)
        {
            _NSQLevel[_NSQLevel.Length-1].text = $"Level {_nsqLevel}\nComplete";
            
            var lastScore = PlayerPrefs.GetInt($"NSQScore{_nsqLevel}", 0);
            
            if (lastScore < score)
            {
                _NSQScore.text = $"New Record on level: {score}!\nCongratulations!";
                PlayerPrefs.SetInt($"NSQScore{_nsqLevel}", score);
            }
            else
                _NSQScore.text = $"Score on level: {score}\nBest score: {lastScore}";
            
            NSQStaticUtil.NSQIsGameOver = true;
            
            _NSQGame.DOFade(0f, 0.2f).SetEase(Ease.Flash);

            if (_NSQMusic.volume > 0.4f)
                _NSQMusic.DOFade(0.1f, 0.1f).SetEase(Ease.Flash);
            
            _NSQSfx.NSQPlayOneShot(_NSQWinClip);
            
            _NSQGame.interactable = false;

            _NSQWinBG.DOFade(1f, 0.1f).SetEase(Ease.Flash);
            _NSQWin.DOFade(1f, 0.2f).SetEase(Ease.Flash).OnComplete(() =>
            {
                _NSQParticle.SetActive(true);
                _NSQWin.NSQEnableCanvas(true);
            });

            _nsqLevel++;
            
            var lvlsPass = PlayerPrefs.GetInt("NSQLevelPass", 1);
            if (lvlsPass < _nsqLevel)
                PlayerPrefs.SetInt("NSQLevelPass", _nsqLevel);
            
            PlayerPrefs.SetInt("NSQLevel", Mathf.Clamp(_nsqLevel, 1, 99));

        }
        
        private void NSQLose()
        {
            NSQStaticUtil.NSQIsGameOver = true;
            
            _NSQMusic.DOFade(0.1f, 0.1f).SetEase(Ease.Flash);
            _NSQSfx.NSQPlayOneShot(_NSQLoseClip);
            
            _NSQGame.interactable = false;
            
            _NSQLose.DOFade(1f, 0.2f).SetEase(Ease.Flash).OnComplete(() =>
            {
                _NSQLose.NSQEnableCanvas(true);
            });
        }
    }
}