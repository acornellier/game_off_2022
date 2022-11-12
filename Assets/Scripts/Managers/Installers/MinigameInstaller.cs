using UnityEngine;
using Zenject;

public class MinigameInstaller : MonoInstaller
{
    [SerializeField] Stage _stage;

    public override void InstallBindings()
    {
        Container.BindInstance(_stage);

        Container.BindInterfacesAndSelfTo<MinigameManager>().FromComponentInHierarchy().AsSingle();
    }
}