using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().DOFade(0, 2f).OnComplete(() => gameObject.SetActive(false));       
    }

}
