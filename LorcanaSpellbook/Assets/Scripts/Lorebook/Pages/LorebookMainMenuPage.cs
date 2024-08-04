namespace LorcanaLorebook.UI
{
    /// <summary>
    /// Page that loads cards and allows user to select begin.
    /// </summary>
    public class LorebookMainMenuPage : LorebookPage 
    {
        public LorebookButton BinderButton;

        public override void Initialize(LorebookManager manager)
        {
            base.Initialize(manager);      
            BinderButton.Button.onClick.AddListener(OnBinderSortClicked);
        }

        public override void Disable()
        {
            base.Disable();
            BinderButton.Button.onClick.RemoveAllListeners();
        }

        private void OnBinderSortClicked()
        {
            _manager.ChangeState(LorebookState.BinderSort);
        }
    }
}