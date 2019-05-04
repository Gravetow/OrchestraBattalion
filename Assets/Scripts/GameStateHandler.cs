using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameStateHandler : MonoBehaviour
{
    [Inject]
    private SignalBus signalBus;

    // Start is called before the first frame update
    private void Start()
    {
        signalBus.Fire<StartGameSignal>();
    }
}