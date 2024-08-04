using System.Collections.Generic;
using UnityEngine;

namespace LorcanaLorebook.UI
{
    /// <summary>
    /// Page that loads cards and allows user to select begin.
    /// </summary>
    public class LorebookBinderSortPage : LorebookPage
    {
        [SerializeField]
        private List<BinderSortSubpage> _subPages;

        private Dictionary<BinderSortState, BinderSortSubpage> _subPageStates = new Dictionary<BinderSortState, BinderSortSubpage>();

        private BinderSortState _state;
        private BinderSortSubpage _activeSubpage;

        public int BinderRows { get; set; } = 3;
        public int BinderCols { get; set; } = 4;

        public override void Initialize(LorebookManager manager)
        {
            base.Initialize(manager);

            foreach (var subpage in _subPages)
            {
                _subPageStates.Add(subpage.State, subpage);
            }

            SetState(BinderSortState.BinderSize);
        }

        public override void Disable()
        {
            base.Disable();
            _activeSubpage.Disable();
            _state = BinderSortState.BinderSize;
        }

        public void SetState(BinderSortState newState)
        {
            if (newState == _state)
            {
                Debug.LogWarning("Setting Binder Sort state to the same state: " + newState);
                return;
            }

            _activeSubpage?.DisableSubpage();
            _activeSubpage = _subPageStates[newState];
            _activeSubpage.InitializeSubpage(this);
            _state = newState;
        }

        public void OnInputChanged(BinderSortInputTag tag, string value)
        {
            switch (tag)
            {
                case BinderSortInputTag.Width:
                    BinderRows = int.Parse(value);
                    break;
                case BinderSortInputTag.Height:
                    BinderCols = int.Parse(value);
                    break;
            }
        }
    }
}