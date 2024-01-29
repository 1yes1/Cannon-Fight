using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class UserManager : IInitializable,IDisposable
    {
        private static UserManager _instance;

        private string _nickname;

        public const string NICKNAME_PREFS = "Nickname";

        public string Nickname => CloudSaveManager.GetValue<string>(NICKNAME_PREFS);

        public static UserManager Instance => _instance;

        public void Initialize()
        {
            if (_instance == null)
                _instance = this;
        }

        public void Dispose()
        {
        }

        public void SetNickname(string nickname)
        {
            _nickname = nickname;
            CloudSaveManager.SetValue<string>(NICKNAME_PREFS, nickname);
        }

    }
}
