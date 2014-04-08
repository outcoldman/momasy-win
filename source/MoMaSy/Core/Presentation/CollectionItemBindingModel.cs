using System;

namespace outcold.MoMaSy.Core.Presentation
{
    internal abstract class CollectionItemBindingModel<TBindingModel, TItem>
        : BindingModel<TBindingModel>
         where TBindingModel : CollectionItemBindingModel<TBindingModel, TItem>
    {
        private bool _isEditMode;
        private bool _isSelected;

        protected CollectionItemBindingModel(TItem item)
        {
            SetItem(item);
        }

        protected TItem Item { get; private set; }

        public void SetItem(TItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            Item = item;

            LoadData();
            RaisePropertyChanged(() => HasChanges);
        }

        public TItem GetItem()
        {
            return GetItem(dirty: false);
        }

        public TItem GetItem(bool dirty)
        {
            if (!dirty)
                SaveData();
            return Item;
        }

        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    RaisePropertyChanged(() => IsEditMode);
                }
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    RaisePropertyChanged(() => IsSelected);
                }
                if (!_isSelected)
                {
                    IsEditMode = false;
                }
            }
        }

        protected override void RaisePropertyChanged(System.Linq.Expressions.Expression<Func<object>> expression)
        {
            base.RaisePropertyChanged(expression);
            RaisePropertyChanged(PropertyNameExtractor.GetPropertyName(() => HasChanges));
        }

        public abstract bool HasChanges
        {
            get;
        }

        protected abstract void LoadData();

        protected abstract void SaveData();
    }
}
