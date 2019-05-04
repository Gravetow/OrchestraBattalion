using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Image>().color = new Color(183, 202, 208, 255);
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().DOFade(0, 1f);       
    }

}
