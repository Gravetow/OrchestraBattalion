using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LifeController : MonoBehaviour
{
    [Inject]
    SignalBus signalBus;
    // Start is called before the first frame update
    private void Awake()
    {
        //signalBus.Subscribe<LoseLifeSignal>();
    }

    private void OnDestroy()
    {
        //signalBus.Subscribe<LoseLifeSignal>();
    }

    private void LoseLife()
    {
        //sigan
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
