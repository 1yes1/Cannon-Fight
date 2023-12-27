using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannonFightBase
{
    [Serializable]
    public struct AgentTraits
    {
        public MovementSettings MovementSettings;
        public FireSettings FireSettings;
        public HealthSettings HealthSettings;
    }

    [Serializable]
    public struct FireSettings
    {
        public int Damage;
        public float Rate;
        public float Range;
    }

    [Serializable]
    public struct MovementSettings
    {
        public float Speed;
    }

    [Serializable]
    public struct HealthSettings
    {
        public int Health;
    }

}
