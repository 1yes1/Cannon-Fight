using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    public interface IPotionCollector
    {
        public bool CanCollectPotion(SkillType skill);
        public void Collect(Potion potion);
    }
}
