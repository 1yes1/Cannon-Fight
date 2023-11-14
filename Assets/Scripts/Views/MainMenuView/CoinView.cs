using CannonFightBase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CannonFightUI
{
    public class CoinView : UIView
    {
        [SerializeField] private TextMeshProUGUI _coinText;

        public override void Initialize()
        {
        }

        public void UpdateCoin(int coin)
        {
            _coinText.text = coin.ToString();
        }

    }
}
