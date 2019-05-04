using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InnerBarrier : MonoBehaviour
{
    [Inject]
    SignalBus signalBus;
    // Start is called before the first frame update
    int counter = 0;
    public int checkCounter;
    private void Awake()
    {
        signalBus.Subscribe<LoseLifeSignal>(DestroyBarrier);
    }

    void OnDestroy()
    {
        signalBus.Unsubscribe<LoseLifeSignal>(DestroyBarrier);
    }



    // Update is called once per frame
    private void DestroyBarrier()
    {
        if(counter == checkCounter)
        {
            Destroy(gameObject);
        }
        counter++;
    }

}
