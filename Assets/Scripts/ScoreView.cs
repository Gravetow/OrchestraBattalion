using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class ScoreView : MonoBehaviour
{
    [Inject]
    SignalBus signalBus;

    private TextMeshProUGUI scoreText;
    private int score = 0;

    private void Awake()
    {
        signalBus.Subscribe<WobbleDestroyedSignal>(UpdateScore);
    }

    private void UpdateScore()
    {
        score++;
        scoreText.SetText("" + score);
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        scoreText.SetText("" + score);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
