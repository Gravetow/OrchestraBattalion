using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryClick : MonoBehaviour, IPointerClickHandler
{

    public List<Sprite> sprites = new List<Sprite>();
    public Image image;

    int currentSprite = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentSprite < sprites.Count)
        {
            image.sprite = sprites[currentSprite];
            currentSprite++;
        } else
        {
            transform.parent.GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() =>
             SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
