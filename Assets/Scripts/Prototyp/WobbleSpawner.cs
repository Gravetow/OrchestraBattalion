using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WobbleSpawner : MonoBehaviour
{
    public GameObject city;
    public GameObject wobblePrefab;

    Dictionary<int, SpectrumData> spectrumDataList = new Dictionary<int, SpectrumData>();
    int[][] spectrumRanges;

    Dictionary<int, GameObject> firstcubes = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> secondcubes = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> thirdcubes = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> fourthcubes = new Dictionary<int, GameObject>();

    void Start()
    {
        // define which aduio hz ranges we want to use
        spectrumRanges = new int[26][]
        {
            /*
            new int[2] {121, 260},
            new int[2] {261, 500},
            //türkis (meistens viel von)
            new int[2] {501, 750},
            //violett (kaum)
            new int[2] {751, 2250}
          */
            /// range 1  :  0-6
            new int[2] {121, 140},
            new int[2] {141, 160},
            new int[2] {161, 180},
            new int[2] {181, 200},
            new int[2] {201, 220},
            new int[2] {221, 240},
            new int[2] {241, 260},

            // range 2 :  7-12
            new int[2] {261, 280},
            new int[2] {281, 300},
            new int[2] {301, 350},
            new int[2] {351, 400},
            new int[2] {401, 450},
            new int[2] {451, 500},

            // range 3  :  13- 18
            new int[2] {501, 550},
            new int[2] {551, 600},
            new int[2] {601, 650},
            new int[2] {651, 700},
            new int[2] {701, 800},
            new int[2] {801, 900},

            // range 4   :19- 25
            new int[2] {901, 1000},
            new int[2] {1001, 1200},
            new int[2] {1201, 1400},
            new int[2] {1401, 1600},
            new int[2] {1601, 1800},
            new int[2] {1801, 2000},
            new int[2] {2001, 2250}

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

            if (group.index < 7)
            {
                firstcubes.Add(group.index, cube);
                //var mat = cube.GetComponent<Image>().color = Color.blue;
            }
            else if (group.index < 13)
            {
                secondcubes.Add(group.index, cube);
                //var mat = cube.GetComponent<Image>().color = Color.red;
            }
            else if (group.index < 19)
            {
                thirdcubes.Add(group.index, cube);
                //var mat = cube.GetComponent<Image>().color = Color.yellow;

            }
            else
            {
                fourthcubes.Add(group.index, cube);
                //var mat = cube.GetComponent<Image>().color = Color.green;

            }

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

        bool firstGroup = false;
        bool secondGroup = false;
        bool thirdGroup = false;
        bool fourthGroup = false;

        // normalize data between 0 and 1
        foreach (var item in spectrumDataList)
        {
            item.Value.dataNormalized = Mathf.Clamp((item.Value.data - min) / (max - min), 0.01f, 1f);

            var group = item.Value;
            if (group.dataNormalized > 0.15f && group.index < 7)
                firstGroup = true;
            if (group.dataNormalized > 0.25f && group.index >= 7 && group.index < 13)
                secondGroup = true;
            if (group.dataNormalized > 0.4f && group.index >= 13 && group.index < 19)
            {
                thirdGroup = true;
            }
            if (group.dataNormalized > 0.7f && group.index >= 19 && group.index < 26)
            {
                fourthGroup = true;
            }
        }

        foreach (GameObject cube in firstcubes.Values)
        {
            if (firstGroup)
            {
                cube.GetComponent<Wobble>().speed = 10;
            }
            else
            {
                cube.GetComponent<Wobble>().speed = 25;
            }
        }
        foreach (GameObject cube in secondcubes.Values)
        {
            if (secondGroup)
            {
                cube.GetComponent<Wobble>().speed = 10;

            }
            else
            {
                cube.GetComponent<Wobble>().speed = 25;
            }
        }
        foreach (GameObject cube in thirdcubes.Values)
        {
            if (thirdGroup)
            {
                cube.GetComponent<Wobble>().speed = 10;

            }
            else
            {
                cube.GetComponent<Wobble>().speed = 25;
            }
        }
        foreach (GameObject cube in fourthcubes.Values)
        {
            if (fourthGroup)
            {
                cube.GetComponent<Wobble>().speed = 10;

            }
            else
            {
                cube.GetComponent<Wobble>().speed = 25;
            }
        }

    }
}
