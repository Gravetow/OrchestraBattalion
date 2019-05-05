using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FinalScreen : MonoBehaviour
{
    [Inject]
    private SignalBus signalBus;

    // Start is called before the first frame update
    private void Awake()
    {
        signalBus.Subscribe<EndGameSignal>(Show);
        GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }

    private void OnDestroy()
    {
        signalBus.Unsubscribe<EndGameSignal>(Show);
    }

    private void Show()
    {
        GetComponent<Image>().DOFade(1, 0.5f);
    }

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}