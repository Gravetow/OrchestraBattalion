using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WobbleSpawner : MonoBehaviour
{
    [Inject]
    private SignalBus signalBus;

    [SerializeField]
    private List<GameObject> wobblePrefab = new List<GameObject>();

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
            int groupNumber = Random.Range(0, 3);
            GameObject wobble = Instantiate(wobblePrefab[groupNumber]);
            wobble.transform.position = transform.position + new Vector3(Random.Range(-5, 5), Random.Range(0, 10), 0);
            wobble.GetComponent<Wobble>().signalBus = signalBus;
            wobble.GetComponent<Wobble>().city = cityPosition;
            wobble.GetComponent<Wobble>().group = groupNumber;
            yield return new WaitForSeconds(Random.Range(0.25f, 0.75f));
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}