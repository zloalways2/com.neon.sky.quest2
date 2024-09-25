using UnityEngine;
using UnityEngine.UI;

namespace NSQ
{
    public class NSQMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _NSQMenu;
        [SerializeField] private CanvasGroup _NSQLevels;
        [SerializeField] private CanvasGroup _NSQExit;
        
        [Space]
        [SerializeField] private Button _NSQLevelsBtn;
        [SerializeField] private Button _NSQExitBtn;
        
        [Space]
        [SerializeField] private Button[] _NSQBacksBtn;
        
        [Space]
        [SerializeField] private Button _NSQQuitBtn;

        private void Awake()
        {
            _NSQMenu.NSQEnableCanvas(true);
            _NSQLevels.NSQEnableCanvas(false);
            _NSQExit.NSQEnableCanvas(false);
            
            _NSQLevelsBtn.onClick.AddListener(() =>
            {
                _NSQMenu.NSQEnableCanvas(false);
                _NSQLevels.NSQEnableCanvas(true);
            });
            
            _NSQExitBtn.onClick.AddListener(() =>
            {
                _NSQMenu.NSQEnableCanvas(false);
                _NSQExit.NSQEnableCanvas(true);
            });

            foreach (var nsqBtn in _NSQBacksBtn)
            {
                nsqBtn.onClick.AddListener(() =>
                {
                    _NSQMenu.NSQEnableCanvas(true);
                    _NSQLevels.NSQEnableCanvas(false);
                    _NSQExit.NSQEnableCanvas(false);
                });
            }
            
            _NSQQuitBtn.onClick.AddListener(Application.Quit);
        }
    }
}