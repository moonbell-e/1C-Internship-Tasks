using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentFPS;
    [SerializeField] private TextMeshProUGUI _averageFPS;
    [SerializeField] private TextMeshProUGUI _worst5FPS;
    [SerializeField] private TextMeshProUGUI _worst1FPS;

    [SerializeField] private Button _resetFPSButton;
    [SerializeField] private Button _startFPSButton;
    [SerializeField] private FPSCounter _fpsCounter;

    private bool _isShowFPS;

    private void Start()
    {
        _resetFPSButton.onClick.AddListener(ResetFPS);
        _startFPSButton.onClick.AddListener(ShowFPS);
    }

    private void Update()
    {
        if (!_isShowFPS) return;

        _currentFPS.text = _fpsCounter.CurrentFPS.ToString();
        _averageFPS.text = _fpsCounter.AverageFPS.ToString();
        _worst5FPS.text = _fpsCounter.Worst5PercentFPS.ToString();
        _worst1FPS.text = _fpsCounter.Worst1PercentFPS.ToString();
    }

    public void ShowFPS()
    {
        _isShowFPS = true;
    }

    private void ResetFPS()
    {
        _isShowFPS = false;

        _currentFPS.text = "0";
        _averageFPS.text = "0";
        _worst5FPS.text = "0";
        _worst1FPS.text = "0";
    }
}