using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Wobble : MonoBehaviour
{
    public GameObject city;
    public float speed = 20;

    public float speedmult = 1f;
    public Sprite[] animationSprites;

    int nextSpriteNumber = 1;
    bool next = true;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

        StartCoroutine(Animate());
      
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime  * speedmult; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, city.transform.position, step);

    }

  IEnumerator Animate()
    {
        if (next)
        {
            image.sprite = animationSprites[nextSpriteNumber];
            next = false;
        }
        else
        {
            image.sprite = animationSprites[nextSpriteNumber - 1];
            next = true;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(Animate());

    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "OuterZone")
        {
            nextSpriteNumber += 2;

        } else if(other.gameObject.name == "InnerZone")
        {
            nextSpriteNumber += 2;
        }
    }



}
