
using System.Collections.ObjectModel;
namespace outcold.MoMaSy.Core.Presentation
{
    internal abstract class CollectionBindingModel<T, TBindingModel> : BindingModel
        where T : class
        where TBindingModel : CollectionItemBindingModel<TBindingModel, T>
    {
        private ObservableCollection<TBindingModel> _items;

        public ObservableCollection<TBindingModel> Items
        {
            get
            {
                return _items;
            }
            protected set
            {
                if (_items != value)
                {
                    _items = value;
                    RaisePropertyChanged(() => Items);
                }
            }
        }

        public TBindingModel SelectedBindingModel
        {
            get
            {
                if (_items != null)
                {
                    foreach (TBindingModel item in _items)
                    {
                        if (item.IsSelected)
                        {
                            return item;
                        }
                    }
                }

                return null;
            }
            set
            {
                var previous = SelectedBindingModel;
                if (previous != value)
                {
                    if (previous != null)
                    {
                        previous.IsSelected = false;
                    }
                    if (value != null)
                        value.IsSelected = true;
                    RaisePropertyChanged(() => SelectedBindingModel);
                }
            }
        }
    }
}
