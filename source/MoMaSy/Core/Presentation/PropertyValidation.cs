using System;

namespace outcold.MoMaSy.Core.Presentation
{
	internal class PropertyValidation<TBindingModel>
		where TBindingModel : BindingModel<TBindingModel>
	{
		private Func<TBindingModel, bool> _validationCriteria;
		private readonly string _propertyName;
        private Func<string> _funcErrorMessage;

		public PropertyValidation(string propertyName)
		{
			_propertyName = propertyName;
		}

		public PropertyValidation<TBindingModel> When(Func<TBindingModel, bool> validationCriteria)
		{
			if (_validationCriteria != null)
				throw new InvalidOperationException("You can only set the validation criteria once.");

			_validationCriteria = validationCriteria;
			return this;
		}

        public PropertyValidation<TBindingModel> Show(string errorMessage)
        {
            if (errorMessage == null)
                throw new ArgumentNullException("errorMessage");

            return Show(() => errorMessage);
        }

		public PropertyValidation<TBindingModel> Show(Func<string> funcErrorMessage)
		{
            if (funcErrorMessage == null)
                throw new ArgumentNullException("errorMessage");

            if (_funcErrorMessage != null)
				throw new InvalidOperationException("You can only set the message once.");

            _funcErrorMessage = funcErrorMessage;
			return this;
		}

		public bool IsInvalid(TBindingModel presentationModel)
		{
			if (_validationCriteria == null)
				throw new InvalidOperationException(
					"No criteria have been provided for this validation. (Use the 'When(..)' method.)");

			return _validationCriteria(presentationModel);
		}

		public string GetErrorMessage()
		{
            if (_funcErrorMessage == null)
				throw new InvalidOperationException(
					"No error message has been set for this validation. (Use the 'Show(..)' method.)");

            return _funcErrorMessage();
		}

		public string PropertyName
		{
			get { return _propertyName; }
		}
	}
}
