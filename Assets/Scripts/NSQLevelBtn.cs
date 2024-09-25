using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NSQ
{
    public class NSQLevelBtn : Button
    {
        /// <summary>
        /// NSQ
        /// TEXT
        /// FOR
        /// BUTTON
        /// FILL
        /// ONLY
        /// WITH
        /// DEBUG
        /// MODE
        /// </summary>
        [field: SerializeField] public TMP_Text _NSQLevel { get; private set; }
    }
}