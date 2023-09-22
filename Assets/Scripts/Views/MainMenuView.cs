using CannonFightUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CannonFightBase
{
    public class MainMenuView : UIView
    {
        [SerializeField] private Button btnPlay;

        public override void Initialize()
        {
            btnPlay.onClick.AddListener(Launcher.Instance.JoinRoom);
        }

    }
}
