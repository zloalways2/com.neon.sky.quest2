using System;
using UnityEngine;

namespace NSQ
{
    public enum ENSQColor
    {
        Pink = 0,
        Blue = 1,
        Orange = 2,
        Red = 3,
        Purple = 4,
        Green = 5,
    }
    
    public class NSQTile : MonoBehaviour
    {
        public Vector2Int NSQPosition { get; set; }
        public bool NSQIsActive { get; set; } = true;
        
        public event Action<NSQTile> NSQOnTileClicked;
        
        [SerializeField] private ParticleSystem _NSQParticles;
        [SerializeField] private Material _NSQFxMat;
        
        [field: SerializeField] public ENSQColor NSQColor { get; private set; }

        private void OnMouseUpAsButton()
        {
            if (!NSQIsActive)
                return;
            
            if (NSQStaticUtil.NSQStopUpdate)
                return;
            
            NSQOnTileClicked?.Invoke(this);
        }

        public void NSQDestroy()
        {
            if (NSQStaticUtil.NSQStopUpdate)
                return;
            
            var position = transform.position;
            position.z = -1f;
            var particle = Instantiate(_NSQParticles, position, Quaternion.identity);
            var renderer = particle.GetComponent<ParticleSystemRenderer>();
            renderer.material = _NSQFxMat;
        }
    }
}