using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {

        transform.parent.GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() =>
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMoveY(100, 5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);           
        transform.DOLocalMoveX(10, 2.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);           
    }
}
