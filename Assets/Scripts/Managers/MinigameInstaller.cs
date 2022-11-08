using Zenject;

public class MinigameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MinigameUi>().FromComponentInHierarchy().AsSingle();
    }
}