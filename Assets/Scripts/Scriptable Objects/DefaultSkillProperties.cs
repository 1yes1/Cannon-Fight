﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "DefaultSkillProperties", menuName = "CannonFight/DefaultSkillProperties", order = 1)]
    public class DefaultSkillProperties: ScriptableObjectInstaller<DefaultSkillProperties>
    {
        [Header("MultiBall")]

        public int MultiBallCount = 3;

        public float MultiBallFrequency = 0.1f;

        public int MultiBallSkillTime = 10;

        [Header("Health")]

        public int MaxHealth = 100;


        [Header("Damage")]

        public int DamageMultiplier = 2;

        public int DamageSkillTime = 15;

        public override void InstallBindings()
        {
            Container.Bind<DefaultSkillProperties>().FromScriptableObjectResource("ScriptableObjects/DefaultSkillProperties").AsSingle();
        }
    }
}