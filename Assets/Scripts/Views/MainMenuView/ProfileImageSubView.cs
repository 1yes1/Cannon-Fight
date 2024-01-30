using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CannonFightUI
{
    public class ProfileImageSubView : UISubView
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _nickname;

        private Texture2D _imageTexture;

        public override void Initialize()
        {
            
        }

        public override void SetParameters(params object[] objects)
        {
            _imageTexture = (Texture2D)objects[0];
        }

        public void SetProfileImage()
        {
            if(_imageTexture != null)
            {
                _image.sprite = Sprite.Create(_imageTexture, new Rect(0.0f, 0.0f, _imageTexture.width, _imageTexture.height), new Vector2(0.5f, 0.5f), 100.0f); ;
                _image.transform.localScale = Vector3.one;
            }
        }

        public void SetNickname(string nickname)
        {
            _nickname.text = nickname;
        }
    }
}
