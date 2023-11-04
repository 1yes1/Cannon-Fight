using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class SaveManager : MonoBehaviour
    {

        private CoinManager.SaveSettings coinManagerSave;

        [Inject]
        public void Construct(CoinManager.SaveSettings saveSettings)
        {
            coinManagerSave = saveSettings;
        }

    }
}
