using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Zenject;

public class ReplayButton : MonoBehaviour, IPointerClickHandler
{
    [Inject]
    SignalBus signalBus;

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.parent.GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() =>
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    private void Awake()
    {
        signalBus.Subscribe<EndGameSignal>(Activate);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        signalBus.Unsubscribe<EndGameSignal>(Activate);

    }

    private void Activate()
    {
        gameObject.SetActive(true);
    }
}
