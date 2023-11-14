using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CannonFightBase
{
    public class PlayerItem : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI _nameText;

        private Player _player;

        public Player Player => _player;

        public void Initialize(Player player)
        {
            _player = player;
            _nameText.text = player.NickName;
        }

    }
}
