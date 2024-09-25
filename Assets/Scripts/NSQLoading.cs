using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace NSQ
{
    public class NSQLoading : MonoBehaviour
    {
        private void Start() => Invoke(nameof(NSQLoadMenu), Random.Range(0.25f, 1f));
        private void NSQLoadMenu() => SceneManager.LoadScene("Menu");
    }
}