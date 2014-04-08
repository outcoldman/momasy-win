using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace outcold.MoMaSy.Core.Presentation
{
    internal abstract class HierarchicalItemBindingModel<TBindingModel, TItem>
        : CollectionItemBindingModel<TBindingModel, TItem>
         where TBindingModel : HierarchicalItemBindingModel<TBindingModel, TItem>
    {
        private ObservableCollection<TBindingModel> _children = new ObservableCollection<TBindingModel>();
        private bool _isExpanded;

        protected HierarchicalItemBindingModel(TBindingModel parent, TItem item)
            : base(item)
        {
            Parent = parent;
        }

        public TBindingModel Parent
        {
            get;
            private set;
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;

                    if (Children.Count == 0)
                        _isExpanded = false;

                    RaisePropertyChanged(() => IsExpanded);
                }
            }
        }

        public ObservableCollection<TBindingModel> Children
        {
            get
            {
                return _children;
            }
        }

        internal void SetChildren(IEnumerable<TBindingModel> children)
        {
            if (_children.Count > 0)
                throw new NotSupportedException("Children already initialized.");

            if (children != null)
            {
                foreach (var child in children)
                {
                    _children.Add(child);
                }
            }

            _children.CollectionChanged += (sender, eventArgs) =>
            {
                if (_children.Count == 0)
                    IsExpanded = false;
            };
        }
    }
}
