using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Services;
using outcold.MoMaSy.Core.Presentation;

namespace outcold.MoMaSy.Model.Dictionaries
{
    internal abstract class DomainDictionaryBase<TInterface, TService>
        where TService : IDictionaryEntityService<TInterface>
    {
        private ObservableCollection<TInterface> _collection;

        public ObservableCollection<TInterface> Collection
        {
            get
            {
                if (_collection == null)
                {
                    _collection = new ObservableCollection<TInterface>();
                    LoadCollection();
                }
                return _collection;
            }
        }

        private void LoadCollection()
        {
            IEnumerable<TInterface> types = null;
            var service = IoC.Instance.Resolve<TService>();

            Task.Factory.StartNew(() =>
            {
                service.CollectionChanged -= LoadCollection;
                types = PrepareCollection(service.GetAll());
            }).ContinueWith((t) =>
            {
                Debug.Assert(types != null, "types != null");
                Application.Current.Dispatcher.Do(() =>
                    {
                        _collection.Clear();
                        foreach (var type in types)
                        {
                            _collection.Add(type);
                        }
                        service.CollectionChanged += LoadCollection;
                    });
            });
        }

        protected virtual IEnumerable<TInterface> PrepareCollection(IEnumerable<TInterface> collection)
        {
            return collection;
        }
    }
}
