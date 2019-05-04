using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WobbleSpawner : MonoBehaviour
{
    [Inject]
    private SignalBus signalBus;

    [SerializeField]
    private GameObject wobblePrefab;

    [SerializeField]
    private Transform cityPosition;

    private bool spawn;

    private void Awake()
    {
        signalBus.Subscribe<StartGameSignal>(StartSpawning);
        signalBus.Subscribe<EndGameSignal>(EndSpawning);
    }

    private void OnDestroy()
    {
        signalBus.Unsubscribe<StartGameSignal>(StartSpawning);
        signalBus.Unsubscribe<EndGameSignal>(EndSpawning);
    }

    private void StartSpawning()
    {
        spawn = true;

        StartCoroutine(Spawn());
    }

    private void EndSpawning()
    {
        spawn = false;
        StopCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (spawn)
        {
            GameObject wobble = Instantiate(wobblePrefab);
            wobble.transform.position = transform.position + new Vector3(Random.Range(-5, 5), Random.Range(0, 10), 0);
            wobble.GetComponent<Wobble>().signalBus = signalBus;
            wobble.GetComponent<Wobble>().city = cityPosition;
            wobble.GetComponent<Wobble>().group = Random.Range(0, 3);
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}