using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FileSearchApp.ViewModel
{
    abstract class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        // Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }


        public bool HasErrors
        {
            get
            {
                if (_errors.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public bool PropertyHasErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Clear all errors against a property
        /// </summary>
        /// <param name="propertyName"></param>
        public void ClearErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
            }
        }

        /// <summary>
        /// Add an error against a property
        /// </summary>
        public void AddError(string propertyName, string errormsg)
        {

            if (_errors.ContainsKey(propertyName))
            {   // error already present. Add to existing error list
                _errors[propertyName].Add(errormsg);
            }
            else
            {   // no error present. add new error list to dict
                _errors.Add(propertyName, new List<string>() { errormsg });
            }
        }

        /// <summary>
        /// Get errors for provided property
        /// </summary>
        public IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];
            else
                return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Get first error for a property
        /// </summary>
        public string GetFirstError(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName][0];
            else
                return string.Empty;
        }

        /// <summary>
        /// Add and Raise error event
        /// </summary>
        public void RaiseError(string propertyName, string errorMsg)
        {
            ClearErrors(propertyName);
            AddError(propertyName, errorMsg);
            OnErrorsChanged(propertyName);
        }

    }
}
