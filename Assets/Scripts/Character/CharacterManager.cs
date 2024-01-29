using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public abstract class CharacterManager : MonoBehaviour
    {
        protected string _nickName;

        protected Sprite _picture;

        private int _killCount;

        private bool _isWinner;

        public string NickName => _nickName;

        public Sprite Picture => _picture;

        public int KillCount => _killCount;

        public bool IsWinner => _isWinner;

        public void SetNameAndPicture(string name, Sprite sprite)
        {
            _nickName = name;
            _picture = sprite;
        }

        public virtual void GetKill()
        {
            _killCount++;
        }

        public virtual void SetAsWinner()
        {
            _isWinner = true;
            Invoke(nameof(LeftRoom), 2);
        }

        public virtual void SetAsLoser()
        {
            Invoke(nameof(LeftRoom), 2);
        }

        private void LeftRoom()
        {
            PhotonNetwork.Disconnect();
        }
    }
}
