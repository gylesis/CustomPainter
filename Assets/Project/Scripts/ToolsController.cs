using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class ToolsController : MonoBehaviour
    {
        [SerializeField] private ToolUI[] _tools;
        [SerializeField] private Slider _brushSize; 
        public ToolType ToolType { get; private set; }
        
        public int BrushSize { get;private set; } 
        
        private void Awake()
        {
            _brushSize.onValueChanged.AddListener((OnSizeChanged));

            int startSize = 35;
            
            _brushSize.value =(float) startSize / 100;

            BrushSize = startSize;
            
            foreach (ToolUI tool in _tools)
            {
                tool.Chosen += ToolChosen;
            }
        }

        private void OnSizeChanged(float value)
        {
            BrushSize = (int) (value * 100);
        }

        private void ToolChosen(ToolType toolType)
        {
            ToolType = toolType;
        }

        private void OnDestroy()
        {
            foreach (ToolUI tool in _tools)
            {
                tool.Chosen -= ToolChosen;
            }
            
            _brushSize.onValueChanged.RemoveListener((OnSizeChanged));
        }
    }
}