using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CannonFightBase
{
    public class PlayerItem : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _image;

        private Player _player;

        public Player Player => _player;

        public void Initialize(Player player)
        {
            _player = player;
            _nameText.text = player.NickName;
        }

        public void Initialize(string name,Sprite profilePicture)
        {
            _player = null;
            _nameText.text = name;
            _image.sprite = profilePicture;

            _image.SetNativeSize();
            _image.color = Color.white;
        }
    }
}
