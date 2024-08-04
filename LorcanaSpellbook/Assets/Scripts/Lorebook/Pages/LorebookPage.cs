using System.Collections.Generic;
using UnityEngine;

namespace LorcanaLorebook.UI
{
    public abstract class LorebookPage : MonoBehaviour 
    {

        public LorebookState LorebookState;

        protected LorebookManager _manager;

        public virtual void Initialize(LorebookManager manager)
        {
            gameObject.SetActive(true);
            _manager = manager;
        }

        public virtual void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}