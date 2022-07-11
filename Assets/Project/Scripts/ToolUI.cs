using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class ToolUI : MonoBehaviour
    {
        [SerializeField] private ToolType _toolType;
        [SerializeField] private Toggle _toggle;
        [SerializeField] private TMP_Text _text;
        
        public event Action<ToolType> Chosen;
    
        private void Awake()
        {
            _toggle.onValueChanged.AddListener((OnValueChanged));
            _text.text = _toolType.ToString();
        }

        private void OnValueChanged(bool value)
        {
            if (value) 
                Chosen?.Invoke(_toolType);
        }

        private void OnDestroy()
        {
            _toggle.onValueChanged.RemoveAllListeners();
        }
    }
}