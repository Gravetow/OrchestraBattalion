using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AudioInterpreter : ITickable, IInitializable
{
    [Inject] private SignalBus signalBus;

    private int[][] spectrumRanges;
    private Dictionary<int, SpectrumData> spectrumDataList = new Dictionary<int, SpectrumData>();

    public void Initialize()
    {
        spectrumRanges = new int[4][]
        {
            new int[2] {0, 260},
            new int[2] {261, 500},
            new int[2] {501, 900},
            new int[2] {901, 2250},
        };

        for (int i = 0; i < spectrumRanges.Length; i++)
        {
            var group = new SpectrumData
            {
                index = i
            };
            spectrumDataList.Add(i, group);
        }
    }

    public void Tick()
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
        bool[] wobbleGroups = new bool[4];
        foreach (var item in spectrumDataList)
        {
            item.Value.dataNormalized = Mathf.Clamp((item.Value.data - min) / (max - min), 0.01f, 1f);

            var group = item.Value;
            if (group.dataNormalized > 0.15f && group.index == 0)
                wobbleGroups[0] = true;
            if (group.dataNormalized > 0.25f && group.index == 1)
                wobbleGroups[1] = true;
            if (group.dataNormalized > 0.7f && group.index == 2)
            {
                wobbleGroups[2] = true;
            }
            if (group.dataNormalized > 0.7f && group.index == 3)
            {
                wobbleGroups[3] = true;
            }
        }
        signalBus.Fire(new MoveWobblesSignal() { wobbleGroups = wobbleGroups });
    }
}