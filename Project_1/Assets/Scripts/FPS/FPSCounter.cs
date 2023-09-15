using System;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
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
        _frameTimeArray = new float[MaxFPSCount];
    }

    private void Update()
    {
        if (!_isCalculate) return;

        CalculateFPS();
        var isIntervalReached = Time.unscaledTime - _lastPercentileUpdateTime >= PercentilesUpdateInterval;
        
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
        Array.Clear(_frameTimeArray, 0, _frameTimeArray.Length);
        CurrentFPS = AverageFPS = Worst5PercentFPS = Worst1PercentFPS = 0;
    }

    private void CalculateFPS()
    {
        _frameCount++;
        CurrentFPS = (int)(1f / CountFrameTime());
        _frameTimeArray[CurrentFPS] += CountFrameTime();
        _totalFrameTime += CountFrameTime();
        
        AverageFPS = (int)(_frameCount / _totalFrameTime);
    }

    private void CalculatePercentiles()
    {
        Worst5PercentFPS = GetPercentile(FifthPercentile);
        Worst1PercentFPS = GetPercentile(FirstPercentile);
    }
    
    private static float CountFrameTime()
    {
        var frameTime = Time.unscaledDeltaTime;
        return frameTime;
    }

    private int GetPercentile(float percentile)
    {
        var threshold = _totalFrameTime * percentile;
        float framesCount = 0;

        for (var i = 0; i < _frameTimeArray.Length; i++)
        {
            framesCount += _frameTimeArray[i];
            if (framesCount >= threshold)
                return i;
        }
        
        return 0;
    }
}