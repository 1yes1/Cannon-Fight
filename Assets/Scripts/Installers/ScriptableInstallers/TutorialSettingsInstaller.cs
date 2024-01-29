using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    [CreateAssetMenu(fileName = "TutorialSettingsInstaller", menuName = "CannonFight/TutorialSettingsInstaller")]
    public class TutorialSettingsInstaller : ScriptableObjectInstaller<TutorialSettingsInstaller>
    {
        public GameTutorialManager.GameTutorialSettings GameTutorialSettings;
        public override void InstallBindings()
        {
            Container.BindInstance(GameTutorialSettings);
        }
    }
}