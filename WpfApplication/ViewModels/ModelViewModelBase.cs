using System;
using System.Windows;
using CommonLibrary.IOC;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using CommonLibrary.Tools;
using MaCompta.Commands;
using MaCompta.Common;
using MaCompta.Controls;

namespace MaCompta.ViewModels
{
    public abstract class ModelViewModelBase<T, TM> : ViewModelBase<T>, IViewModel
        where T : ModelViewModelBase<T, TM>
        where TM: ModelBase
    {

        #region Members
// ReSharper disable StaticFieldInGenericType
        private static int _newId = -1;
// ReSharper restore StaticFieldInGenericType
        #endregion

        #region Constructor
        protected ModelViewModelBase()
        {
        } 

        /// <summary>
        /// constructeur par défaut
        /// </summary>
        protected ModelViewModelBase(IContainer container)
            : base(container)
        {
            Id = _newId--;
        } 
        /// <summary>
        /// constructeur par défaut
        /// </summary>
        protected ModelViewModelBase(IContainer container, IServiceBase<TM> serviceBase)
            : base(container)
        {
            Id = _newId--;
            ModelServiceBase = serviceBase;
        }         
        #endregion

        #region Properties
        /// <summary>
        /// Service de base pour le modèle associé
        /// </summary>
        protected IServiceBase<TM> ModelServiceBase { get; set; }

        protected string ModelName { get; set; }

        private long _id;

        /// <summary>
        /// Identifiant de l'objet associé
        /// </summary>
        public long Id
        {
            get { return _id; }
            set { _id = value;
            RaisePropertyChanged(vm => vm.Id);
            }
        }

        private bool _isNew;

        /// <summary>
        /// Indique que l'objet est en cours de création
        /// </summary>
        public bool IsNew
        {
            get { return _isNew; }
            set { _isNew = value;
            RaisePropertyChanged(vm => vm.IsNew);
            RaisePropertyChanged(vm => vm.IsSaved);
            }
        }

        /// <summary>
        /// indique que l'objet a été sauvegardé (il n'est plus en cours de création)
        /// </summary>
        public bool IsSaved
        {
            get { return !IsNew; }
        }

        /// <summary>
        /// Modèle associé
        /// </summary>
        public TM Model { get; set; }

        private String _libelle;
        public string Libelle
        {
            get
            {
                return _libelle;
            }
            set
            {
                _libelle = value;
                RaisePropertyChangedWithModification(vm => vm.Libelle);
            }
        }

        private bool _isSelected;
        

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(vm => vm.IsSelected);
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Evènement pour indiquer une suppression
        /// </summary>
        public event EventHandler<EventArgs<TM>> ViewModelDeleted;

        protected void RaiseDeletedEvent()
        {
            //ViewModelDeleted.Raise(this, EventArgs.Empty);
            if (ViewModelDeleted != null)
                ViewModelDeleted(this, new EventArgs<TM> {Data=Model });
        }

        /// <summary>
        /// Evènement pour indiquer une sauvegarde
        /// </summary>
        public event EventHandler<EventArgs<TM>> ViewModelSaved;

        protected void RaiseSavedEvent()
        {
            //ViewModelSaved.Raise(this, new EventArgs<TM> { Data = Model });
            if (ViewModelSaved !=null)
                ViewModelSaved(this, new EventArgs<TM> { Data = Model });
        }

        /// <summary>
        /// Evènement pour indiquer une duplication
        /// </summary>
        public event EventHandler ViewModelDuplicated;

        protected void RaiseDuplicatedEvent(T vm)
        {
            ViewModelDuplicated.Raise(vm, EventArgs.Empty);
            //if (ViewModelDuplicated != null)
            //    ViewModelDuplicated(arg, EventArgs.Empty);
        }

        /// <summary>
        /// Evènement pour indiquer une mise à jour du montant
        /// </summary>
        public event EventHandler MontantUpdated;

        /// <summary>
        /// déclenche l'évènement de changement de montant
        /// </summary>
        public void RaiseMontantUpdated()
        {
            MontantUpdated.Raise(this, EventArgs.Empty);
        }
        #endregion

        #region Commands
        //private ICommand m_AnnulerCommand;
        ///// <summary>
        ///// Commande de sauvegarde
        ///// </summary>
        //public ICommand AnnulerCommand
        //{
        //    get
        //    {
        //        if (m_AnnulerCommand == null)
        //        {
        //            m_AnnulerCommand = new RelayCommand(param => Annuler());
        //        }
        //        return m_AnnulerCommand;
        //    }
        //}


        //private ICommand m_ActionSauvegarderCommand;
        ///// <summary>
        ///// Commande de sauvegarde
        ///// </summary>
        //public ICommand ActionSauvegarderCommand
        //{
        //    get
        //    {
        //        if (m_ActionSauvegarderCommand == null)
        //        {
        //            m_ActionSauvegarderCommand = new RelayCommand(param => ActionSauvegarder());
        //        }
        //        return m_ActionSauvegarderCommand;
        //    }
        //}

        //private ICommand m_ActionSupprimerCommand;
        ///// <summary>
        ///// Commande de suppression
        ///// </summary>
        //public ICommand ActionSupprimerCommand
        //{
        //    get
        //    {
        //        if (m_ActionSupprimerCommand == null)
        //        {
        //            m_ActionSupprimerCommand = new RelayCommand(param => ActionSupprimer());
        //        }
        //        return m_ActionSupprimerCommand;
        //    }
        //}

        //private void DemandeSupprimer()
        //{
        //    if (MessageBox.Show("Etes-vous sûr de vouloir supprimer cet élément?", "Suppression", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
        //        ActionSupprimer();
        //}

        //private ICommand m_ActionDupliquerCommand;
        ///// <summary>
        ///// Commande de duplication
        ///// </summary>
        //public ICommand ActionDupliquerCommand
        //{
        //    get
        //    {
        //        if (m_ActionDupliquerCommand == null)
        //        {
        //            m_ActionDupliquerCommand = new RelayCommand(param => ActionDupliquer());
        //        }
        //        return m_ActionDupliquerCommand;
        //    }
        //}
        #endregion     

        #region Methods


        /// <summary>
        /// Duplication d'un élément
        /// </summary>
        [BaseCommand("ActionDupliquerCommand")]
        public virtual void ActionDupliquer()
        {
            var vm = DuplicateViewModel();
            RaiseDuplicatedEvent(vm);
        }

        /// <summary>
        /// Suppression de l'élément
        /// </summary>
        [BaseCommand("ActionSupprimerCommand")]
        public void ActionSupprimer()
        {
            WpfIocFactory.Instance.MainVm.MessageBoxShow(Supprimer, String.Format("Etes-vous sûr de vouloir supprimer ce(tte) {0}?", ModelName), "Alerte", MessageBoxButton.YesNo);
        }

        public virtual void Supprimer(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (!IsNew)
                {
                    //on ne supprime de la base que s'il a déjà été sauvegardé
                    ModelServiceBase.DeleteItem(Id, false);
                }
                LogMessage("{0} supprimé(e) : {1}", ModelName, ToString());
                RaiseDeletedEvent();
            }
        }

        /// <summary>
        /// Sauvegarde de l'élément
        /// </summary>
        [BaseCommand("ActionSauvegarderCommand")]
        public virtual void ActionSauvegarder()
        {
            //sauvegarde de la rubrique
            //WpfIocFactory.Instance.LogMessage(String.Format("{0} en cours de sauvegarde...", ModelName ));
            if (IsNew)
            {
                SaveToModel();
                ModelServiceBase.CreateItem(Model);
                Id = Model.Id;
                //Model = saved;
                IsNew = false;
                IsModified = false;
                LogMessage("{0} créé(e) : {1}", ModelName, ToString());
            }
            else if (IsModified)
            {
                SaveToModel();
                ModelServiceBase.UpdateItem(Model);
                IsModified = false;
                LogMessage("{0} mis(e) à jour : {1}", ModelName, ToString());
            }
            RaiseSavedEvent();
        }

        [BaseCommand("AnnulerCommand")]
        public virtual void Annuler()
        {
            //CancelModel(Model);
            if (Model != null)
            {
                InitFromModel(Model);
                IsModified = false;
            }
        }

        [BaseCommand("CopierCommand")]
        public virtual void Copier()
        {
        }

        [BaseCommand("CollerCommand")]
        public virtual void ActionColler()
        {
        }

        ///// <summary>
        ///// Initialisation du view model à partir d'un objet model
        ///// </summary>
        ///// <param name="model"></param>
        //public abstract void CancelModel(TM model);

        /// <summary>
        /// Initialisation du view model à partir d'un objet model
        /// </summary>
        /// <param name="model"></param>
        public abstract T InitFromModel(TM model);

        public virtual void ReinitFromModel()
        {}

        /// <summary>
        /// renvoie un objet model initialisé à partir du view model
        /// </summary>
        /// <returns></returns>
        public abstract void SaveToModel();

        /// <summary>
        /// Duplication du view model hors Id
        /// </summary>
        /// <returns></returns>
        public abstract T DuplicateViewModel();

        public override string ToString()
        {
            return Libelle;
        }

        #endregion

    }
}
