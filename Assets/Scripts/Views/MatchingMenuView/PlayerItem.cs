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

        public void Initialize(Player player,Sprite sprite = null)
        {
            _player = player;
            _nameText.text = player.NickName;

            if(sprite != null)
            {
                _image.sprite = sprite;
                _image.color = Color.white;
                _image.transform.localScale = Vector3.one;
            }
        }

        public void Initialize(string name,Sprite profilePicture)
        {
            _player = null;
            _nameText.text = name;

            if(profilePicture != null)
            {
                _image.sprite = profilePicture;
                _image.color = Color.white;
                _image.transform.localScale = Vector3.one;
            }
        }
    }
}
