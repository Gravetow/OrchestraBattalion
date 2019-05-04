using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<StartGameSignal>();
        Container.DeclareSignal<EndGameSignal>();
        Container.DeclareSignal<MoveWobblesSignal>();
        Container.DeclareSignal<WobbleDestroyedSignal>();
        Container.DeclareSignal<LoseLifeSignal>();

        Container.BindInterfacesTo<AudioInterpreter>().AsSingle();
    }
}