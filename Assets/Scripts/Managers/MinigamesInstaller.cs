using Zenject;

public class MinigamesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MinigameUi>().FromComponentInHierarchy().AsSingle();
    }
}