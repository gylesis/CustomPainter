using UnityEngine;

namespace Project
{
    public class Entry : MonoBehaviour
    {
        [SerializeField] private Painter _painter;
        [SerializeField] private ToolsController _toolsController;
        [SerializeField] private ColorPalette _colorPalette;
        [SerializeField] private Camera _camera;
        private void Awake()
        {
            _painter.Init(_toolsController, _colorPalette, _camera);
        }
    }
}