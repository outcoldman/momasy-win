using System;
using System.Diagnostics;
using outcold.MoMaSy.BindingModels.Settings;
using outcold.MoMaSy.Core;
using outcold.MoMaSy.Core.Presentation;
using outcold.MoMaSy.Model;
using outcold.MoMaSy.Resources.Strings;
using outcold.MoMaSy.Services;
using outcold.MoMaSy.Views.Settings;

namespace outcold.MoMaSy.Presenters.Settings
{
    internal class TransactionTypesViewPresenter : Presenter<ITransactionTypesView, TransactionTypesBindingModel>
    {
        private ITransactionTypesService _transactionTypesService;

        public TransactionTypesViewPresenter(ITransactionTypesView view)
            : base(view)
        {
            if (!view.IsInDesignTool)
            {
                _transactionTypesService = IoC.Resolve<ITransactionTypesService>();
                LoadTransactionTypes();
            }
        }

        public void LoadTransactionTypes()
        {
            DoBackground(() =>
            {
                var transactionTypes = _transactionTypesService.GetAll();
                BindingModel.SetTransactionTypes(transactionTypes);
            });
        }

        internal void EditCurrentTransactionType()
        {
            DoBackground(() =>
            {
                var bm = BindingModel.SelectedTransactionTypeBindingModel;
                Debug.Assert(bm != null, "bm != null");

                bm.IsEditMode = true;
            });
        }

        internal void UndoCurrentTransactionType()
        {
            var bm = BindingModel.SelectedTransactionTypeBindingModel;
            Debug.Assert(bm != null, "bm != null");
            UndoTransactionType(bm);
        }

        internal void UndoTransactionType(TransactionTypeBindingModelBase transactionTypeBindingModel)
        {
            if (transactionTypeBindingModel == null)
                throw new ArgumentNullException("transactionTypeBindingModel");

            DoBackground(() =>
            {
                ITransactionType type = transactionTypeBindingModel.GetItem();
                transactionTypeBindingModel.SetItem(_transactionTypesService.Get(type));
                transactionTypeBindingModel.IsEditMode = false;
            });
        }

        internal void SaveCurrentTransactionType()
        {
            var bm = BindingModel.SelectedTransactionTypeBindingModel;
            Debug.Assert(bm != null);
            SaveTransactionType(bm);
        }

        internal void SaveTransactionType(TransactionTypeBindingModelBase transactionTypeBindingModel)
        {
            if (transactionTypeBindingModel == null)
                throw new ArgumentNullException("transactionTypeBindingModel");

            DoBackground(() =>
            {
                Debug.Assert(!transactionTypeBindingModel.IsSystem, "!transactionTypeBindingModel.IsSystem");

                transactionTypeBindingModel.ValidateAll();
                if (transactionTypeBindingModel.IsValid)
                {
                    if (transactionTypeBindingModel.HasChanges)
                    {
                        ITransactionType type = transactionTypeBindingModel.GetItem();
                        _transactionTypesService.Save(type);
                        transactionTypeBindingModel.SetItem(type);
                    }

                    transactionTypeBindingModel.IsEditMode = false;
                }
                else
                {
                    Debug.Assert(!string.IsNullOrWhiteSpace(transactionTypeBindingModel.Error), "!string.IsNullOrWhiteSpace(transactionTypeBindingModel.Error)");
                    string errMessage = string.Format(StringResources.ErrMsg_ItemUnsaved, transactionTypeBindingModel.Error);

                    UndoTransactionType(transactionTypeBindingModel);

                    NotifyService.ShowError(errMessage);
                }
            });
        }

        internal void AddTransactionType()
        {
            var selectedModel = BindingModel.SelectedTransactionTypeBindingModel;
            Debug.Assert(selectedModel != null, "selectedModel != null");

            TransactionTypeBindingModelBase bm = null;

            DoBackground(() =>
            {
                var parent = selectedModel.IsSystem ? null : selectedModel.GetItem(dirty: true);
                var transactionType = _transactionTypesService.CreateNew(parent);
                bm = new TransactionTypeBindingModel(selectedModel, transactionType);
            }, () =>
            {
                Debug.Assert(bm != null, "bm != null");

                selectedModel.Children.Add(bm);
                selectedModel.IsExpanded = true;
                bm.IsSelected = true;
                bm.IsEditMode = true;
            });
        }

        internal void DeleteTransactionType()
        {
            var bm = BindingModel.SelectedTransactionTypeBindingModel;
            Debug.Assert(bm != null, "bm != null");

            DoBackground(() =>
            {
                ITransactionType transactionType = bm.GetItem();

                Debug.Assert(transactionType != null, "transactionType != null");
                Debug.Assert(!bm.IsSystem, "!transactionType.IsSystem");

                string errorMessage;
                if (!_transactionTypesService.CanBeDeleted(transactionType, out errorMessage))
                {
                    Debug.Assert(!string.IsNullOrWhiteSpace(errorMessage));
                    NotifyService.ShowError(errorMessage);
                    return false;
                }

                _transactionTypesService.Delete(transactionType);
                return true;
            },
            () =>
            {
                var parent = bm.Parent;
                parent.IsSelected = true;
                parent.Children.Remove(bm);
            });
        }
    }
}
