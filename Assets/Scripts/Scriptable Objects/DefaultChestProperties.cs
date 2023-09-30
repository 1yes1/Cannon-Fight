using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "DefaultChestProperties", menuName = "CannonFight/DefaultChestProperties", order = 1)]
    public class DefaultChestProperties: ScriptableObjectInstaller
    {
        [Header("Chest")]

        public float Health = 50;

        [Header("Chest Potion")]

        public float PotionFlightTime = 0.8f;

        [Header("Fill Chests")]

        public float StartFillTime = 3;

        public float StartFillFrequency = 2;

        public float RefillTime = 5;

        public override void InstallBindings()
        {
            Container.Bind<DefaultChestProperties>().FromScriptableObjectResource("ScriptableObjects/DefaultChestProperties").AsSingle();
        }

    }
}
