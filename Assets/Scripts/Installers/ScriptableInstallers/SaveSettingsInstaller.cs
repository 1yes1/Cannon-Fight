using CannonFightBase;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SaveSettingsInstaller", menuName = "CannonFight/SaveSettingsInstaller")]
public class SaveSettingsInstaller : ScriptableObjectInstaller<SaveSettingsInstaller>
{
    public CoinManager.SaveSettings CoinManagerSaveSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(CoinManagerSaveSettings);
    }
}