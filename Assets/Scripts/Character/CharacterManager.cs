using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public abstract class CharacterManager : MonoBehaviour
    {
        protected string _nickName;

        protected Sprite _picture;

        public string NickName => _nickName;

        public Sprite Picture => _picture;

        public void SetNameAndPicture(string name, Sprite sprite)
        {
            _nickName = name;
            _picture = sprite;
        }

    }
}
