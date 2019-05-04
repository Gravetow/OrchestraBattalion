using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class City : MonoBehaviour
{
    [Inject]
    SignalBus signalBus;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        signalBus.Fire(new LoseLifeSignal());
    }
}
