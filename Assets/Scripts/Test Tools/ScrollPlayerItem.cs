using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CannonFightBase
{
    public class ScrollPlayerItem : MonoBehaviour
    {
        private Player _player;

        [SerializeField] private TextMeshProUGUI _nameText;

        [SerializeField] private TextMeshProUGUI _killText;

        
        public void Initialize(Player player)
        {
            _player = player;
            _nameText.text = player.NickName;
            _killText.text = (_player.CustomProperties.ContainsKey("killCount")) ? _player.CustomProperties["killCount"].ToString() : "0";
        }

        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        public string NickName
        {
            get => Player.NickName;
        }

        public void UpdateKillCount()
        {
            _killText.text = (_player.CustomProperties.ContainsKey("killCount")) ? _player.CustomProperties["killCount"].ToString() : "0";
        }

    }
}
