using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using CommonLibrary.Tools;
using MaCompta.Commands;

namespace MaCompta.ViewModels
{
    public abstract class ViewModelBase<T> : IDynamicMetaObjectProvider, INotifyPropertyChanged, IDisposable
        where T : ViewModelBase<T>
    {
        public delegate void NoArgDelegate();

        protected ViewModelBase()
        {
        }
        
        protected ViewModelBase(CommonLibrary.IOC.IContainer container)
        {
            Container = container;
        }

        /// <summary>
        /// Vue sur la liste et l'item sélectionné.
        /// </summary>
        public ICollectionView CollectionView
        {
            get;
            protected set;
        }

        #region IDynamicMetaObjectProvider Members
        public DynamicMetaObject GetMetaObject(System.Linq.Expressions.Expression parameter)
        {
            return new BaseViewModelMetaObject(parameter,
               BindingRestrictions.GetInstanceRestriction(parameter, this), this);
        }
        #endregion

        protected CommonLibrary.IOC.IContainer Container { get; private set; }

        ///// <summary>
        ///// Conteneur des view models et services
        ///// </summary>
        //public IContainer Container
        //{
        //    get { return SimpleIocFactory.Instance.Container; }
        //}
        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; RaisePropertyChanged(vm=>vm.IsBusy); }
        }


        private bool _isModified;
        public virtual bool IsModified
        {
            get { return _isModified; }
            set
            {
                _isModified = value;
                RaisePropertyChanged(vm => vm.IsModified);
            }
        }

        #region Methods
        /// <summary>
        /// Affichage d'un message de log dans la vue principale
        /// 
        /// </summary>
        /// <param name="format">format de la chaine à afficher</param>
        public void LogMessage(string format)
        {
            WpfIocFactory.Instance.LogMessage(format);
        }

        /// <summary>
        /// Affichage d'un message de log dans la vue principale
        /// 
        /// </summary>
        /// <param name="format">format de la chaine à afficher</param>
        /// <param name="args">paramètres à afficher</param>
        public void LogMessage(string format, params object[] args)
        {
            WpfIocFactory.Instance.LogMessage(format, args);
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <param name="propertyExpression">The expression of the property that changed.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        protected virtual void RaisePropertyChangedWithModification(Expression<Func<T, Object>> propertyExpression)
        {
            if (PropertyChanged != null)
            {
                IsModified = true;
                PropertyChanged(this, new PropertyChangedEventArgs(propertyExpression.GetPropertyName()));
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed sans modification.
        /// </summary>
        /// <param name="propertyExpression">The expression of the property that changed.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        protected virtual void RaisePropertyChanged(Expression<Func<T, Object>> propertyExpression)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyExpression.GetPropertyName()));
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Déclenché lorsque la valeur d'une propriété change.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Commands
        //private ICommand m_ActionAjouterCommand;
        ///// <summary>
        ///// Commande d'ajout
        ///// </summary>
        //public ICommand ActionAjouterCommand
        //{
        //    get
        //    {
        //        if (m_ActionAjouterCommand == null)
        //        {
        //            m_ActionAjouterCommand = new RelayCommand(param => ActionAjouter());
        //        }
        //        return m_ActionAjouterCommand;
        //    }
        //}

        /// <summary>
        /// Ajout d'un sous-élément
        /// </summary>
        [BaseCommand("ActionAjouterCommand")]
        public virtual void ActionAjouter()
        {
            throw new NotImplementedException("ViewModelBase => ActionAjouter");
        }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: supprimer l'état managé (objets managés).
                }

                // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                // TODO: définir les champs de grande taille avec la valeur Null.

                disposedValue = true;
            }
        }

        // TODO: remplacer un finaliseur seulement si la fonction Dispose(bool disposing) ci-dessus a du code pour libérer les ressources non managées.
        // ~ViewModelBase() {
        //   // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
        //   Dispose(false);
        // }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(true);
            // TODO: supprimer les marques de commentaire pour la ligne suivante si le finaliseur est remplacé ci-dessus.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #endregion

    }

    public class BaseViewModelMetaObject : DynamicMetaObject
    {
        private readonly Type _objType;
        private readonly Dictionary<string, MethodInfo> _commands;


        public BaseViewModelMetaObject(System.Linq.Expressions.Expression expression,
                         BindingRestrictions restrictions, object value)
            : base(expression, restrictions, value)
        {
            _objType = value.GetType();
            _commands = new Dictionary<string, MethodInfo>();
            foreach (MethodInfo method in _objType.GetMethods())
            {
                var attrs = (BaseCommandAttribute[])
                  method.GetCustomAttributes(typeof(BaseCommandAttribute), true);
                if (attrs.Length > 0)
                    _commands.Add(attrs[0].CommandName, method);
            }
        }

        //public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        //{
        //    MemberInfo member = System.Linq.Expressions.Expression.PropertyOrField(
        //        System.Linq.Expressions.Expression.Convert(Expression, m_ObjType),
        //        binder.Name).Member;
        //    if (member.GetCustomAttributes(
        //      typeof(NotifyPropertyAttribute), true).Length > 0)
        //        return new DynamicMetaObject(
        //           ExpressionTreesHelper.BuilNotifyPropertyChangedAspect(m_ObjType,
        //               Expression, binder.Name, value.Expression),
        //               BindingRestrictions.GetTypeRestriction(Expression, m_ObjType));
        //    return base.BindSetMember(binder, value);
        //}

        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            if (_commands.ContainsKey(binder.Name))
            {
                LambdaExpression callExpression = System.Linq.Expressions.Expression.Lambda(
                    System.Linq.Expressions.Expression.Call(
                    System.Linq.Expressions.Expression.Convert(Expression, _objType),
                    _commands[binder.Name]));
                System.Linq.Expressions.Expression expression =
                    System.Linq.Expressions.Expression.New(
                    typeof(BaseCommand).GetConstructor(new Type[1] { typeof(Action) }),
                    callExpression);
                return new DynamicMetaObject(expression,
                  BindingRestrictions.GetTypeRestriction(Expression, _objType));
            }
            return base.BindGetMember(binder);
        }
    }

    public class MvvmMessageBoxEventArgs : EventArgs
    {
        public MvvmMessageBoxEventArgs(Action<MessageBoxResult> resultAction, string messageBoxText, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = MessageBoxOptions.None)
        {
            this._resultAction = resultAction;
            this._messageBoxText = messageBoxText;
            this._caption = caption;
            this._button = button;
            this._icon = icon;
            this._defaultResult = defaultResult;
            this._options = options;
        }

        Action<MessageBoxResult> _resultAction;
        string _messageBoxText;
        string _caption;
        MessageBoxButton _button;
        MessageBoxImage _icon;
        MessageBoxResult _defaultResult;
        MessageBoxOptions _options;

        public void Show(Window owner)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(owner, _messageBoxText, _caption, _button, _icon, _defaultResult, _options);
            if (_resultAction != null) _resultAction(messageBoxResult);
        }

        public void Show()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(_messageBoxText, _caption, _button, _icon, _defaultResult, _options);
            if (_resultAction != null) _resultAction(messageBoxResult);
        }
    }
}
