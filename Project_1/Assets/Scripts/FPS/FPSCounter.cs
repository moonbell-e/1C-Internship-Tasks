using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private int _bufferSize;
    private int[] _fpsBuffer;
    private int _fpsBufferIndex;

    public int CurrentFPS { get; private set; }
    public int AverageFPS { get; private set; }
    public int Worst5PercentFPS { get; }
    public int Worst1PercentFPS { get; }

    private void Start()
    {
        _bufferSize = 100;
        _fpsBuffer = new int[_bufferSize];
    }

    private void Update()
    {
        UpdateBuffer();
        CalculateFPS();
    }

    private void OnValidate()
    {
        if (_bufferSize <= 0) _bufferSize = 1;
    }

    private void UpdateBuffer()
    {
        CurrentFPS = (int)(1f / Time.unscaledDeltaTime);
        _fpsBuffer[_fpsBufferIndex++] = CurrentFPS;

        if (_fpsBufferIndex >= _bufferSize) _fpsBufferIndex = 0;
    }

    private void CalculateFPS()
    {
        var sum = 0;

        for (var i = 0; i < _bufferSize; i++) sum += _fpsBuffer[i];

        AverageFPS = sum / _bufferSize;
    }

    private void CalculatePercentiles()
    {
    }
}