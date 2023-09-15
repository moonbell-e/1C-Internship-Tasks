using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentFPS;
    [SerializeField] private TextMeshProUGUI _averageFPS;
    [SerializeField] private TextMeshProUGUI _worst5FPS;
    [SerializeField] private TextMeshProUGUI _worst1FPS;

    [SerializeField] private Button _resetFPSButton;
    [SerializeField] private Button _startFPSButton;
    [SerializeField] private FPSCounter _fpsCounter;

    private float _lastFPSTextUpdateTime;
    private bool _isFPSCounted;

    private const float FPSTextUpdateInterval = 0.4f;

    private void Start()
    {
        _resetFPSButton.onClick.AddListener(ResetFPS);
        _startFPSButton.onClick.AddListener(ShowFPS);
    }

    private void Update()
    {
        bool isIntervalReached = Time.unscaledTime - _lastFPSTextUpdateTime < FPSTextUpdateInterval;
        if (!_isFPSCounted || isIntervalReached) return;
        _lastFPSTextUpdateTime = Time.unscaledTime;
        
        UpdateText();
    }

    private void UpdateText()
    {
        _currentFPS.text = _fpsCounter.CurrentFPS.ToString();
        _averageFPS.text = _fpsCounter.AverageFPS.ToString();
        _worst5FPS.text = _fpsCounter.Worst5PercentFPS >= 0 ? _fpsCounter.Worst5PercentFPS.ToString() : "N/A";
        _worst1FPS.text = _fpsCounter.Worst1PercentFPS >= 0 ? _fpsCounter.Worst1PercentFPS.ToString() : "N/A";
    }

    private void ShowFPS()
    {
        _isFPSCounted = true;
        _fpsCounter.ResetFPSCalculation();
    }

    private void ResetFPS()
    {
        _isFPSCounted = false;
        _fpsCounter.StopFPSCalculation();
        UpdateText();
    }
}