using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class ColorPalette : MonoBehaviour
    {
        [SerializeField] private Slider _rSlider;
        [SerializeField] private Slider _gSlider;
        [SerializeField] private Slider _bSlider;

        [SerializeField] private Image _palette;

        private Camera _camera;
        private Color _currentColor = Color.black;

        public Color Color => _currentColor;
        
        private void Awake()
        {
            _rSlider.onValueChanged.AddListener((RedValueChanged));
            _gSlider.onValueChanged.AddListener((GreenValueChanged));
            _bSlider.onValueChanged.AddListener((BlueValueChanged));
        }

        private void RedValueChanged(float value)
        {
            UpdateColor(0, value);
        }

        private void GreenValueChanged(float value)
        {
            UpdateColor(1, value);
        }

        private void BlueValueChanged(float value)
        {
            UpdateColor(2, value);
        }

        private void UpdateColor(int rgbIndex, float value)
        {
            Color paletteColor = _currentColor;

            paletteColor[rgbIndex] = value;

            _currentColor = paletteColor;

            _palette.color = _currentColor;
        }

        private void OnDestroy()
        {
            _rSlider.onValueChanged.RemoveListener((RedValueChanged));
            _gSlider.onValueChanged.RemoveListener((GreenValueChanged));
            _bSlider.onValueChanged.RemoveListener((BlueValueChanged));
        }
    }
}