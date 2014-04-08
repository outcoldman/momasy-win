using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace outcold.MoMaSy.Core.Presentation
{
    internal abstract class BindingModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler onPropertyChanged = PropertyChanged;
            if (onPropertyChanged != null)
                onPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        protected virtual void RaisePropertyChanged(Expression<Func<object>> expression)
        {
            RaisePropertyChanged(PropertyNameExtractor.GetPropertyName(expression));
        }

        protected IoC IoC
        {
            get { return IoC.Instance; }
        }
    }

    internal abstract class BindingModel<TBindingModel> : BindingModel, IDataErrorInfo
        where TBindingModel : BindingModel<TBindingModel>
    {
        private readonly List<PropertyValidation<TBindingModel>> _validations = new List<PropertyValidation<TBindingModel>>();
        private Dictionary<string, List<string>> _errorMessages = new Dictionary<string, List<string>>();

        protected BindingModel()
        {
            AddAllAttributeValidators();
        }

        #region IDataErrorInfo

        public string this[string columnName]
        {
            get
            {
                if (_errorMessages.ContainsKey(columnName))
                {
                    return String.Join(Environment.NewLine, _errorMessages[columnName]);
                }
                return null;
            }
        }

        public string Error
        {
            get
            {
                StringBuilder errMessage = new StringBuilder();
                foreach (var columnErrors in _errorMessages)
                {
                    foreach (var columnError in columnErrors.Value)
                    {
                        if (errMessage.Length > 0)
                            errMessage.AppendLine();
                        errMessage.Append(columnError);
                    }
                }
                return errMessage.ToString();
            }
        }

        #endregion

        public void ValidateAll()
        {
            var propertyNamesWithValidationErrors = _errorMessages.Keys;

            _errorMessages = new Dictionary<string, List<string>>();

            _validations.ForEach(PerformValidation);

            var propertyNamesThatMightHaveChangedValidation =
                _errorMessages.Keys.Union(propertyNamesWithValidationErrors).ToList();

            propertyNamesThatMightHaveChangedValidation.ForEach(RaisePropertyChanged);
        }

        public bool IsValid
        {
            get { return !_errorMessages.Any(); }
        }

        protected override void RaisePropertyChanged(Expression<Func<object>> expression)
        {
            string propertyName = PropertyNameExtractor.GetPropertyName(expression);
            ValidateProperty(propertyName);
            RaisePropertyChanged(propertyName);
        }

        protected PropertyValidation<TBindingModel> AddValidationFor(Expression<Func<object>> expression)
        {
            return AddValidationFor(PropertyNameExtractor.GetPropertyName(expression));
        }

        private PropertyValidation<TBindingModel> AddValidationFor(string propertyName)
        {
            var validation = new PropertyValidation<TBindingModel>(propertyName);
            _validations.Add(validation);
            return validation;
        }

        private void AddAllAttributeValidators()
        {
            PropertyInfo[] propertyInfos = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Attribute[] custom = Attribute.GetCustomAttributes(propertyInfo, typeof(ValidationAttribute), true);
                foreach (var attribute in custom)
                {
                    var property = propertyInfo;
                    var validationAttribute = attribute as ValidationAttribute;

                    if (validationAttribute == null)
                        throw new NotSupportedException("validationAttribute variable should be inherited from ValidationAttribute type");

                    string name = property.Name;

                    var displayAttribute = Attribute.GetCustomAttributes(propertyInfo, typeof(DisplayAttribute)).FirstOrDefault() as DisplayAttribute;
                    if (displayAttribute != null)
                    {
                        name = displayAttribute.GetName();
                    }

                    var message = validationAttribute.FormatErrorMessage(name);

                    AddValidationFor(propertyInfo.Name)
                        .When(x =>
                        {
                            var value = property.GetGetMethod().Invoke(this, new object[] { });
                            var result = validationAttribute.GetValidationResult(value,
                                                                    new ValidationContext(this, null, null) { MemberName = property.Name });
                            return result != null;
                        })
                        .Show(message);

                }
            }
        }

        private void ValidateProperty(string propertyName)
        {
            _errorMessages.Remove(propertyName);

            _validations.Where(v => v.PropertyName == propertyName).ToList().ForEach(PerformValidation);
        }

        private void PerformValidation(PropertyValidation<TBindingModel> validation)
        {
            if (validation.IsInvalid((TBindingModel)this))
            {
                AddErrorMessageForProperty(validation.PropertyName, validation.GetErrorMessage());
            }
        }

        private void AddErrorMessageForProperty(string propertyName, string errorMessage)
        {
            if (_errorMessages.ContainsKey(propertyName))
            {
                _errorMessages[propertyName].Add(errorMessage);
            }
            else
            {
                _errorMessages.Add(propertyName, new List<string> { errorMessage });
            }
        }
    }
}
