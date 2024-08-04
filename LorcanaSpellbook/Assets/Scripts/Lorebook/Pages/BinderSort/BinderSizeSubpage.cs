using System;
using TMPro;

namespace LorcanaLorebook.UI
{
    public class BinderSizeSubpage : BinderSortSubpage
    {
        public override BinderSortState State {get => BinderSortState.BinderSize; }
        public TextMeshProUGUI RowsText;
        public TextMeshProUGUI ColumnsText;

        public LorebookButton ContinueButton;

        public override void InitializeSubpage(LorebookBinderSortPage owner)
        {
            base.InitializeSubpage(owner);

            RowsText.text = owner.BinderRows.ToString();
            ColumnsText.text = owner.BinderCols.ToString();

            ContinueButton.Button.onClick.AddListener(OnContinue);
        }

        private void OnContinue()
        {
            _owner.SetState(BinderSortState.Display);
        }
    }
}