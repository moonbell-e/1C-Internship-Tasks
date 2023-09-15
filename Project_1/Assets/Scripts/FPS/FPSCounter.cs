using System;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private CalculationMethod _calculationMethod;

    private int[] _fpsArray;
    private float[] _frameTimeArray;

    private int _frameCount;
    private float _totalFrameTime;
    private float _lastPercentileUpdateTime;
    private bool _isCalculate;

    private const float FifthPercentile = 0.05f;
    private const float FirstPercentile = 0.01f;
    private const float PercentilesUpdateInterval = 5f;
    private const int MaxFPSCount = 1000;


    public int CurrentFPS { get; private set; }
    public int AverageFPS { get; private set; }
    public int Worst5PercentFPS { get; private set; }
    public int Worst1PercentFPS { get; private set; }

    private void Start()
    {
        _fpsArray = new int[MaxFPSCount];
        _frameTimeArray = new float[MaxFPSCount];
    }

    private void Update()
    {
        if (!_isCalculate) return;

        CalculateFPS();
        bool isIntervalReached = Time.unscaledTime - _lastPercentileUpdateTime >= PercentilesUpdateInterval;
        if (isIntervalReached)
        {
            CalculatePercentiles();
            _lastPercentileUpdateTime = Time.unscaledTime;
        }
    }

    public void StopFPSCalculation()
    {
        _isCalculate = false;
        ResetCounters();
    }

    public void ResetFPSCalculation()
    {
        _isCalculate = true;
    }

    private void ResetCounters()
    {
        _totalFrameTime = 0f;
        _frameCount = 0;
        _lastPercentileUpdateTime = 0;
        Array.Clear(_fpsArray, 0, _fpsArray.Length);
        Array.Clear(_frameTimeArray, 0, _frameTimeArray.Length);
        CurrentFPS = AverageFPS = Worst5PercentFPS = Worst1PercentFPS = 0;
    }

    private void CalculateFPS()
    {
        _frameCount++;
        CurrentFPS = (int)(1f / Time.unscaledDeltaTime);
        UpdateArrays();
        
        _totalFrameTime += CountFrameTime();
        AverageFPS = (int)(_frameCount / _totalFrameTime);
    }

    private void UpdateArrays()
    {
        _fpsArray[CurrentFPS]++;
        _frameTimeArray[CurrentFPS] += CountFrameTime();
    }

    private static float CountFrameTime()
    {
        var frameTime = Time.unscaledDeltaTime;
        
        return frameTime;
    }

    private void CalculatePercentiles()
    {
        switch (_calculationMethod)
        {
            case CalculationMethod.NumberOfFrames:
                Worst5PercentFPS = (int)GetPercentileOfFPS(FifthPercentile);
                Worst1PercentFPS = (int)GetPercentileOfFPS(FirstPercentile);
                break;
            case CalculationMethod.FrameTime:
                Worst5PercentFPS = (int)GetPercentileOfFrameTime(FifthPercentile);
                Worst1PercentFPS = (int)GetPercentileOfFrameTime(FirstPercentile);
                break;
        }
    }

    private float GetPercentileOfFPS(float percentile)
    {
        var thresholdPercent = Mathf.RoundToInt(_frameCount * percentile);
        if (thresholdPercent == 0) return 0;
        var framesCount = 0;
        float result = 0;

        for (var i = 0; framesCount < thresholdPercent; i++)
        {
            framesCount += _fpsArray[i];
            if (framesCount > thresholdPercent)
            {
                var difference = framesCount - thresholdPercent;
                result += i * (_fpsArray[i] - difference);
            }
            else
            {
                result += i * _fpsArray[i];
            }
        }
        return result / thresholdPercent;
    }

    private float GetPercentileOfFrameTime(float percentile)
    {
        var thresholdPercent = _totalFrameTime * percentile;
        if (thresholdPercent == 0) return 0;
        float framesCount = 0;
        float result = 0;

        for (var i = 0; framesCount < thresholdPercent; i++)
        {
            framesCount += _frameTimeArray[i];
            if (framesCount > thresholdPercent)
            {
                var difference = framesCount - thresholdPercent;
                result += i * (_frameTimeArray[i] - difference);
            }
            else
            {
                result += i * _frameTimeArray[i];
            }
        }
        
        return result / thresholdPercent;
    }
}