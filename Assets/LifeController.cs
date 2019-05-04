using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LifeController : MonoBehaviour
{
    [Inject]
    SignalBus signalBus;

    public Image Life1;
    public Image Life2;
    public Image Life3;

    public Sprite LifeLost1;
    public Sprite LifeLost2;
    public Sprite LifeLost3;

    public int life = 3;

    // Start is called before the first frame update
    private void Awake()
    {
        signalBus.Subscribe<LoseLifeSignal>(LoseLife);
    }

    private void OnDestroy()
    {
        signalBus.Unsubscribe<LoseLifeSignal>(LoseLife);
    }

    private void LoseLife()
    {
        if(life == 3)
        {
            Life3.sprite = LifeLost3;
        } else if(life == 2)
        {
            Life2.sprite = LifeLost2;
        }
        else if(life == 1)
        {
            Life1.sprite = LifeLost1;
            signalBus.Fire(new EndGameSignal());
        }
        life--;
    }


}
