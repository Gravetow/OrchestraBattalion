﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WobbleSpawner : MonoBehaviour
{
    public GameObject city;
    public GameObject wobblePrefab;

    Dictionary<int, SpectrumData> spectrumDataList = new Dictionary<int, SpectrumData>();
    int[][] spectrumRanges;

    Dictionary<int, GameObject> cubes = new Dictionary<int, GameObject>();

    void Start()
    {
        // define which aduio hz ranges we want to use
        spectrumRanges = new int[4][]
        {

            new int[2] {121, 260},
            new int[2] {261, 500},
            new int[2] {501, 900},
            new int[2] {901, 2250}
          
            /*// range 1
            new int[2] {121, 140},
            new int[2] {141, 160},
            new int[2] {161, 180},
            new int[2] {181, 200},
            new int[2] {201, 220},
            new int[2] {221, 240},
            new int[2] {241, 260},

            // range 2
            new int[2] {261, 280},
            new int[2] {281, 300},
            new int[2] {301, 350},
            new int[2] {351, 400},
            new int[2] {401, 450},
            new int[2] {451, 500},

            // range 3
            new int[2] {501, 550},
            new int[2] {551, 600},
            new int[2] {601, 650},
            new int[2] {651, 700},
            new int[2] {701, 800},
            new int[2] {801, 900},

            // range 4  
            new int[2] {901, 1000},
            new int[2] {1001, 1200},
            new int[2] {1201, 1400},
            new int[2] {1401, 1600},
            new int[2] {1601, 1800},
            new int[2] {1801, 2000},
            new int[2] {2001, 2250}
            */
        };
        // create data objects for each range
        for (int i = 0; i < spectrumRanges.Length; i++)
        {
            var group = new SpectrumData
            {
                index = i
            };
            spectrumDataList.Add(i, group);
        }
        // initialize the spectrum data list
        var count = 0;

        // create the cubes for each data range
        foreach (var item in spectrumDataList)
        {
            var group = item.Value;
            var cube = Instantiate(wobblePrefab);
            cube.GetComponent<Wobble>().city = city;
            cube.name = "" + spectrumRanges[item.Key].ToString();
            cube.transform.SetParent(transform);
            cube.name = group.index.ToString();
            var rotate = 360f / spectrumDataList.Count * count;
            cube.transform.position = transform.position + new Vector3(Random.Range(-450, 450), 0, 0);
            cube.SetActive(true);

            // set cube color
            var mat = cube  .GetComponent<Image>().color =  Color.HSVToRGB(1f / spectrumDataList.Count * count, 1, 1);
          
            cubes.Add(group.index, cube);

            count++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // how much hz does each spectrum data sample represent
        var hzPerSample = (AudioSettings.outputSampleRate / 8192f);

        // get min/max data range to be able to normalize it later
        var min = Mathf.Infinity;
        var max = -Mathf.Infinity;
        var spectrumData = new float[8192];

        // get spectrum data from the audio listener with the best quality possible
        AudioListener.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        for (int i = 0; i < spectrumRanges.Length - 1; i++)
        {
            var range = spectrumRanges[i];
            var group = spectrumDataList[i];
            group.data = 0;

            // get the minimum and maxium data index that as valid in this range chunk
            var minIndex = Mathf.FloorToInt(range[0] / hzPerSample);
            var maxIndex = Mathf.CeilToInt(range[1] / hzPerSample);

            // add data to the range group
            for (var si = minIndex; si <= maxIndex; si++)
            {
                group.AddData(spectrumData[si]);
            }

            // get min/max data for later normalize process
            if (min > group.data)
            {
                min = group.data;
            }
            if (max < group.data)
            {
                max = group.data;
            }
        }
        // normalize data between 0 and 1
        foreach (var item in spectrumDataList)
        {
            item.Value.dataNormalized = Mathf.Clamp((item.Value.data - min) / (max - min), 0.01f, 1f);
        }
    }
}