namespace LorcanaLorebook.UI
{
    public abstract class BinderSortSubpage : LorebookPage 
    {
        protected LorebookBinderSortPage _owner;
        public abstract BinderSortState State { get; }

        public virtual void InitializeSubpage(LorebookBinderSortPage owner)
        {
            base.Initialize(LorebookManager.Instance);
            enabled = true;
            _owner = owner;
        }

        public virtual void DisableSubpage()
        {
            base.Disable();
            enabled = false;
        }
    }
}