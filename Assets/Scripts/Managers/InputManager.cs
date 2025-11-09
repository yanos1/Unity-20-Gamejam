using System;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public bool InputEnabled { get; set; } = true;

        public void EnableInput()
        {
            InputEnabled = true;
        }
        
        public void DisableInput()
        {
            InputEnabled = false;
        }
            
            
        
        

        
    }
}