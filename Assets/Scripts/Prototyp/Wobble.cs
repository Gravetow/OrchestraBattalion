using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;
using UnityEngine.EventSystems;

public class Wobble : MonoBehaviour
{
    public SignalBus signalBus;

    public Transform city;
    public float speed = 20;

    public float speedmult = 1f;

    public int group = 0;

    public Animator animator;

    private bool happy;

    private SpriteRenderer spriteRenderer;

    private void OnDestroy()
    {
        signalBus.Unsubscribe<MoveWobblesSignal>(Move);
        signalBus.Unsubscribe<LoseLifeSignal>(DieThroughCity);

    }

    private void Move(MoveWobblesSignal moveWobblesSignal)
    {
        if (!happy)
        {
            if (moveWobblesSignal.wobbleGroups[group])
            {
                if (animator.GetBool("happy") == true)
                {
                    animator.SetBool("happy", false);
                }
                speed = 3;
            }
            else
            {
                if (animator.GetBool("happy") != true)
                {
                    animator.SetBool("happy", true);
                }
                happy = true;
                speed = 0;
            }
        }
    }

    private void OnFinishDancing()
    {
        happy = false;
    }

    // Start is called before the first frame update
    private void Start()
    {
        signalBus.Subscribe<MoveWobblesSignal>(Move);
        signalBus.Subscribe<LoseLifeSignal>(DieThroughCity);

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        float step = speed * Time.deltaTime * speedmult; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, city.position, step);

        CheckForTouch();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "OuterZone")
        {
            speedmult = 0.5f;
            animator.SetInteger("evil", 1);
        }
        else if (other.gameObject.name == "InnerZone")
        {
            speedmult = 0.25f;
            animator.SetInteger("evil", 2);
        }
    }

    private void OnClicked()
    {
        if (happy)
            animator.SetBool("dying", true);
    }

    private void OnMouseDown()
    {
        OnClicked();
    }

    private bool CheckForTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            var wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            var touchPosition = new Vector2(wp.x, wp.y);

            if (GetComponent<BoxCollider2D>() == Physics2D.OverlapPoint(touchPosition))
            {
                OnClicked();
            }
        }

        return false;
    }

    public void DieThroughCity()
    {
        happy = true;
        animator.SetBool("happy", true);
        animator.SetBool("dying", true);
        speed = 0;
    }

    public void OnDead()
    {
        signalBus.Fire(new WobbleDestroyedSignal());
        Destroy(gameObject);
    }
}