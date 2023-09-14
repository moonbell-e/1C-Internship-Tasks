using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSShower : MonoBehaviour
{
    [SerializeField] private float _fpsTextUpdateInterval;
    [SerializeField] private TextMeshProUGUI _currentFPS;
    [SerializeField] private TextMeshProUGUI _averageFPS;
    [SerializeField] private TextMeshProUGUI _worst5FPS;
    [SerializeField] private TextMeshProUGUI _worst1FPS;

    [SerializeField] private Button _resetFPSButton;
    [SerializeField] private Button _startFPSButton;
    [SerializeField] private FPSCounter _fpsCounter;

    private float _lastFPSTextUpdateTime;
    private bool _isShowFPS;

    private void Start()
    {
        _resetFPSButton.onClick.AddListener(ResetFPS);
        _startFPSButton.onClick.AddListener(ShowFPS);
    }

    private void Update()
    {
        if (!_isShowFPS || !(Time.unscaledTime - _lastFPSTextUpdateTime >= _fpsTextUpdateInterval)) return;
        
        UpdateText();
        _lastFPSTextUpdateTime = Time.unscaledTime;
    }

    private void UpdateText()
    {
        _currentFPS.text = _fpsCounter.CurrentFPS.ToString();
        _averageFPS.text = _fpsCounter.AverageFPS.ToString();
        _worst5FPS.text = _fpsCounter.Worst5PercentFPS >= 1 ? _fpsCounter.Worst5PercentFPS.ToString() : "N/A";
        _worst1FPS.text = _fpsCounter.Worst1PercentFPS >= 1 ? _fpsCounter.Worst1PercentFPS.ToString() : "N/A";
    }
    
    private void ShowFPS()
    {
        _isShowFPS = true;
        _fpsCounter.ResetFPSCalculation();
    }

    private void ResetFPS()
    {
        _isShowFPS = false;
        _fpsCounter.StopFPSCalculation();

        _currentFPS.text = "0";
        _averageFPS.text = "0";
        _worst5FPS.text = "0";
        _worst1FPS.text = "0";
    }
}