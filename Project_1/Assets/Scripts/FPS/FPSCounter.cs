using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private CalculationMethod _calculationMethod;
    
    //NumberOfFrames
    private int[] _fpsBuffer;
    private int _fpsBufferIndex;
    
    //FrameTime
    private float[] _frameTimeBuffer;
    private int _frameTimeIndex;
    
    private int _bufferSize = 100;
    private bool _isCalculate;

    private const float FifthPercentile = 5f;
    private const float FirstPercentile = 1f;

    public int CurrentFPS { get; private set; }
    public int AverageFPS { get; private set; }
    public int Worst5PercentFPS { get; private set; }
    public int Worst1PercentFPS { get; private set; }

    private void Start()
    {
        _fpsBuffer = new int[_bufferSize];
        _frameTimeBuffer = new float[_bufferSize];
        if (_bufferSize <= 0) _bufferSize = 1;
    }

    private void Update()
    {
        if (!_isCalculate) return;
        UpdateBuffers();
        
        switch (_calculationMethod)
        {
            case CalculationMethod.NumberOfFrames:
                CalculateFPS();
                break;
            case CalculationMethod.FrameTime:
                CalculateFPSFrameTimeMethod();
                break;
        }
    }

    public void StopFPSCalculation()
    {
        _isCalculate = false;

        Array.Clear(_fpsBuffer, 0, _fpsBuffer.Length);
        Array.Clear(_frameTimeBuffer, 0, _frameTimeBuffer.Length);
        _fpsBufferIndex = _frameTimeIndex = 0;
        CurrentFPS = AverageFPS = Worst5PercentFPS = Worst1PercentFPS = 0;
    }

    public void ResetFPSCalculation()
    {
        _isCalculate = true;
    }
    
    private void UpdateBuffers()
    {
        UpdateFpsBuffer(); 
        UpdateFrameTimeBuffer();

        if (_fpsBufferIndex < _bufferSize) return;
        
        _fpsBufferIndex = 0;
        _frameTimeIndex = 0;
    }

    private void UpdateFpsBuffer()
    {
        CurrentFPS = (int)(1f / Time.unscaledDeltaTime);
        _fpsBuffer[_fpsBufferIndex++] = CurrentFPS;
    }

    private void UpdateFrameTimeBuffer()
    {
        float frameTime = Time.unscaledDeltaTime;
        _frameTimeBuffer[_frameTimeIndex++] = frameTime;
    }

    #region NumberOfFrames Calculation

    private void CalculateFPS()
    {
        var sum = 0;
        for (var i = 0; i < _bufferSize; i++) sum += _fpsBuffer[i];
        
        AverageFPS = sum / _bufferSize;
        Worst5PercentFPS = CalculatePercentiles(FifthPercentile);
        Worst1PercentFPS = CalculatePercentiles(FirstPercentile);
    }

    private int CalculatePercentiles(float percentile)
    {
        int[] sortedFpsBuffer = _fpsBuffer.OrderBy(x => x).ToArray();
        int sequenceNumPercentile = Mathf.RoundToInt((percentile * _bufferSize) / 100);

        return sortedFpsBuffer[sequenceNumPercentile];
    }

    #endregion

    #region FrameTime Calculation

    private void CalculateFPSFrameTimeMethod()
    {
        float sumOfFrameTime = 0f;

        for (int i = 0; i < _bufferSize; i++) sumOfFrameTime += _frameTimeBuffer[i];
        var averageFrameTime = sumOfFrameTime / _bufferSize;

        AverageFPS = (int)(1f / averageFrameTime);
        Worst5PercentFPS = (int)(1f/CalculatePercentilesFrameTimeMethod(FifthPercentile));
        Worst1PercentFPS = (int)(1 / CalculatePercentilesFrameTimeMethod(FirstPercentile));

    }

    private float CalculatePercentilesFrameTimeMethod(float percentile)
    {
        float[] sortedFrameTimeBuffer = _frameTimeBuffer.OrderBy(x => x).ToArray();
        int sequenceNumPercentile = Mathf.RoundToInt((100f - percentile * _bufferSize / 100));

        return sortedFrameTimeBuffer[sequenceNumPercentile];
    }

    #endregion
}