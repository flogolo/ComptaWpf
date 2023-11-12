using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using MaCompta.ViewModels;
using MaCompta.Tools;
using MaCompta.Dialogs;
using System.Windows.Input;
using CommonLibrary.Tools;
using System;
using MaCompta.Controls;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.Messaging;

namespace MaCompta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        //WpfIocFactory m_Pf = new WpfIocFactory();
        private readonly MainViewModel _mainVm;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public delegate void NoArgDelegate();

        public MainWindow()
        {
            Log.Debug("Wpf initialize");
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                _mainVm = WpfIocFactory.Instance.MainVm;
                Loaded += MainWindowLoaded;
                if (_mainVm.TestDatabase())
                {
                    _mainVm.DisplayMessage("Chargement des données...");
                    //chargement des rubriques directement car utilisées par les virements
                    WpfIocFactory.Instance.RubriquesVm.LoadRubriques();
                    //Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                      //  new NoArgDelegate(WpfIocFactory.Instance.RubriquesVm.LoadRubriques));
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new NoArgDelegate(_mainVm.LoadMain));
                    LoadVirements();
                }
                AddHandler(CloseableTabItem.CloseTabEvent, new RoutedEventHandler(CloseTab));
                _mainVm.PropertyChanged += _mainVm_PropertyChanged;
            }
        }

        private void _mainVm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("_mainVm_PropertyChanged" + e.PropertyName);
            if(e.PropertyName == "MessageService")
            {
                messagesLB.ScrollIntoView(messagesLB.Items[0]);
            }
        }

        private void LoadVirements()
        {
            var dc = WpfIocFactory.Instance.Container.Resolve<VirementsViewModel>();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new NoArgDelegate(dc.LoadVirements));
        }

        private void CloseTab(object source, RoutedEventArgs args)
        {
            TabItem tabItem = args.Source as TabItem;
            if (tabItem != null)
            {
                MesTabs.Items.Remove(tabItem);
                var compteView = tabItem.Content as ComptesView;
                _mainVm.OpenedComptes.Remove(compteView.DataContext as CompteViewModel);
            }
        }

        void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            Log.Debug("Wpf loaded");
            DataContext = _mainVm;
            MesComptas.DataContext = _mainVm;

            WpfIocFactory.Instance.MainVm.SelectedCompteChanged += OnMainVmSelectedCompteChanged;
            WpfIocFactory.Instance.MainVm.CompteDeleted += MainVm_CompteDeleted;

            MesRubriques.DataContext = WpfIocFactory.Instance.RubriquesVm;
            //MesComptes.DataContext = WpfIocFactory.Instance.ComptesVm;

            var dc = WpfIocFactory.Instance.Container.Resolve<VirementsViewModel>();
            MesVirements.DataContext = dc;

            ResizeTabItems();

            _mainVm.MessageBoxRequest += MainVmMessageBoxRequest;
            WeakReferenceMessenger.Default.Register<ShowDialogMessage>(this, (r, m) => HandleShowDialog(m));
        }

        private void MainVm_CompteDeleted(object sender, EventArgs<CompteViewModel> e)
        {
            var itemExist = false;
            CloseableTabItem tabItem = null;
            //onglet ouvert?
            foreach (var item in MesTabs.Items)
            {
                if (item is CloseableTabItem)
                {
                    tabItem = item as CloseableTabItem;
                    var compteView = tabItem.Content as ComptesView;
                    if (compteView != null && tabItem.Header.Equals(e.Data.CompteHeader))
                    {
                        itemExist = true;
                        break;
                    }
                }
            }
            if(itemExist)
            {
                tabItem.RaiseEvent(new RoutedEventArgs(CloseableTabItem.CloseTabEvent, this));
            }
            //recharger les virements
            LoadVirements();
        }

        private void OnMainVmSelectedCompteChanged(object sender, EventArgs<CompteViewModel> e)
        {
            var itemExist = false;
            CloseableTabItem tabItem = null;
            //onglet déjà ouvert?
            foreach (var item in MesTabs.Items)
            {
                if(item is CloseableTabItem)
                {
                    tabItem = item as CloseableTabItem;
                    var compteView = tabItem.Content as ComptesView;
                    if( compteView!=null && tabItem.Header.Equals(e.Data.CompteHeader))
                    {
                        itemExist = true;
                        break;
                    }
                }
            }
            //itemExist = _mainVm.OpenedComptes.Contains(e.Data);
            if (!itemExist)
            {
                tabItem = new CloseableTabItem();
                tabItem.Content = new ComptesView();
                tabItem.DataContext = e.Data;
                tabItem.Header = e.Data.CompteHeader;
                e.Data.Load();
                MesTabs.Items.Add(tabItem);
                _mainVm.AddOpenedCompte(e.Data);
                ResizeTabItems();
            }
            Dispatcher.BeginInvoke((Action)(() => MesTabs.SelectedItem = tabItem));
        }

        private void HandleShowDialog(ShowDialogMessage message)
        {
            switch (message.MessageType)
            {
                case ShowDialogMessageEnum.OperationsFiltrees:
                    var dlg = new DialogOperationsFiltrees
                    {
                        DataContext = new OperationsFiltreesViewModel(message.CompteVM.LastCarteOperation())
                    };
                    dlg.ShowDialog();
                    break;
                case ShowDialogMessageEnum.OperationPredefinie:
                    var dlg2 = new DialogOperationPredefinie();
                    dlg2.ShowDialog();
                    break;
                case ShowDialogMessageEnum.OperationChangeCompte:
                    {
                        var dc = new DialogOperationChangeCompteViewModel()
                        {
                            SelectedOperation = message.SelectedOperation,
                            IsInverse = true
                        };
                        var dlg3 = new DialogOperationChangeCompte() { DataContext = dc };
                        var result = dlg3.ShowDialog();
                        if (result.HasValue && result.Value)
                        {
                            dc.SelectedOperation.CompteId = dc.SelectedCompte.Id;
                            dc.SelectedOperation.IsModified = true;
                            foreach (var detail in dc.SelectedOperation.DetailsList)
                            {
                                if (dc.IsInverse)
                                {
                                    detail.Montant = -detail.Montant;
                                }
                                else
                                {
                                    detail.Montant = detail.Montant;
                                }
                            }

                            dc.SelectedOperation.ActionSauvegarder();
                            //suppression de l'opération de la liste des opérations du compte sélectionné
                            message.CompteVM.RemoveSelectedOperation();
                            //recherche si le compte cible est ouvert
                            var compteVmDst = _mainVm.OpenedComptes.FirstOrDefault(c => c.Id == dc.SelectedCompte.Id);
                            if( compteVmDst != null)
                            {
                                //s'il est ouvert -> ajout de l'opération
                                compteVmDst.AddOperationViewModel(dc.SelectedOperation);
                            }
                        }
                    }
                    break;
                case ShowDialogMessageEnum.OperationFiltre:
                    {
                        var dc = WpfIocFactory.Instance.Container.Resolve<DialogOperationFiltreViewModel>();
                        var dlg4 = new DialogOperationFiltre() { DataContext = dc };
                        var result = dlg4.ShowDialog();
                        if (result.HasValue && result.Value)
                        {
                            message.CompteVM.Filtrer(dc.FiltreType, dc.FiltreDate1, dc.FiltreDate2);
                        }
                    }
                    break;
                case ShowDialogMessageEnum.MessageAttente:
                    //MessageBox.Show("Chargement en cours");
                    Cursor = Cursors.Wait;
                    break;
                case ShowDialogMessageEnum.FinAttente:
                    Cursor = null;
                    break;
            }
        }

        static void MainVmMessageBoxRequest(object sender, MvvmMessageBoxEventArgs e)
        {
            e.Show();
        }

        private void ResizeTabItem(TabItem tab)
        {
            if (tab.Content != null)
            {
                var content = tab.Content as UserControl;

                if (content != null)
                {
                    content.Height = MesTabs.ActualHeight - tab.Height - content.Margin.Top - content.Margin.Bottom - MesTabs.Padding.Top - MesTabs.Padding.Bottom;
                    content.Width = MesTabs.ActualWidth - content.Margin.Left - content.Margin.Right - MesTabs.Padding.Left - MesTabs.Padding.Right;
                }
            }
        }

        private void ResizeTabItems()
        {
            foreach (var item in MesTabs.Items)
            {
                ResizeTabItem((TabItem)item);
            }
            //ResizeTabItem(MesComptas);
            //ResizeTabItem(MesComptes);
            //ResizeTabItem(MesRubriques);
            //ResizeTabItem(MesVirements);
            //ResizeTabItem(MesStats);
        }

        private void UserControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("tab main new size {0} {1}" , e.PreviousSize,e.NewSize);
            ResizeTabItems();
        }
    }
}
