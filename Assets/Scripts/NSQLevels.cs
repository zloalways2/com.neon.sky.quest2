using UnityEngine;
using UnityEngine.SceneManagement;

namespace NSQ
{
    public class NSQLevels : MonoBehaviour
    {
        [SerializeField] private NSQLevelBtn[] _NSQLevels;

        private void Start()
        {
            var nsqLevelPass = PlayerPrefs.GetInt("NSQLevelPass", 1);
            
            for (var i = 0; i < _NSQLevels.Length; i++)
            {
                var index = i;

                _NSQLevels[index]._NSQLevel.text = $"{index + 1}";
                var nsqLevelColor = _NSQLevels[index]._NSQLevel.color;
                nsqLevelColor.a = index < nsqLevelPass ? 1f : 0.5f;
                _NSQLevels[index]._NSQLevel.color = nsqLevelColor;
                _NSQLevels[index].interactable = index < nsqLevelPass;
                
                _NSQLevels[index].onClick.AddListener(() =>
                {
                    PlayerPrefs.SetInt("NSQLevel", index + 1);
                    SceneManager.LoadScene("Game");
                });
            }
        }
    }
}