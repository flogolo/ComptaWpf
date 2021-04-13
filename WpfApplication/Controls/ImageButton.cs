using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MaCompta.Controls
{
    public enum BoutonActionType
    {
        None,
        Ajouter,
        Supprimer,
        Copier,
        Valider,
        ValiderPartiel,
        Coller,
        Sauvegarder,
        Dupliquer,
        Annuler
    }

    public class ImageButton : Button
    {
        readonly Image _image;
        //TextBlock _textBlock = null;

        public ImageButton()
        {
            //System.Diagnostics.Debug.WriteLine("ImageButton constructor");
            //StackPanel panel = new StackPanel();
            //panel.Orientation = Orientation.Horizontal;

            //panel.Margin = new System.Windows.Thickness(10);

            _image = new Image();
            Margin = new Thickness(2);

            SetBinding(ToolTipService.ToolTipProperty, new Binding { Source = this, Path = new PropertyPath("TooltipText") });
            //Width = 22;
            //Height = 22;

            //panel.Children.Add(_image);

            //_textBlock = new TextBlock();
            //panel.Children.Add(_textBlock);

            Content = _image;
        }

        //public string Text
        //{
        //    get
        //    {
        //        if (_textBlock != null)
        //            return _textBlock.Text;
        //        else
        //            return String.Empty;
        //    }
        //    set
        //    {
        //        if (_textBlock != null)
        //            _textBlock.Text = value;
        //    }
        //}

        public ImageSource Image
        {
            get
            {
                if (_image != null)
                    return _image.Source;
                return null;
            }
            set
            {
                if (_image != null)
                    _image.Source = value;
            }
        }

        public double ImageWidth
        {
            get
            {
                if (_image != null)
                    return _image.Width;
                return double.NaN;
            }
            set
            {
                if (_image != null)
                    _image.Width = value;
            }
        }

        public double ImageHeight
        {
            get
            {
                if (_image != null)
                    return _image.Height;
                return double.NaN;
            }
            set
            {
                if (_image != null)
                    _image.Height = value;
            }
        }

        //public string TooltipText { get; set; }


        public string TooltipText
        {
            get { return (string)GetValue(TooltipTextProperty); }
            set { SetValue(TooltipTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TooltipText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TooltipTextProperty =
            DependencyProperty.Register("TooltipText", typeof(string), typeof(ImageButton), new UIPropertyMetadata(string.Empty));



        public BoutonActionType BoutonType
        {
            get { return (BoutonActionType)GetValue(ButtonTypeProperty); }
            set {
                SetValue(ButtonTypeProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for ButtonType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.Register("BoutonType", typeof(BoutonActionType), typeof(ImageButton), new UIPropertyMetadata(OnBoutonActionTypeChanged));

        private static void OnBoutonActionTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bouton = d as ImageButton;
            if (bouton != null)
            {
                bouton.SetImage((BoutonActionType)e.NewValue);
            }
        }

        public void SetImage(BoutonActionType value)
        {
            string imageName = null;
            switch (value)
            {
                case BoutonActionType.Ajouter:
                    imageName = "add.png";
                    break;
                case BoutonActionType.Supprimer:
                    imageName = "delete.png";
                    break;
                case BoutonActionType.Copier:
                    imageName = "copy.png";
                    break;
                case BoutonActionType.Valider:
                    imageName = "accept.png";
                    break;
                case BoutonActionType.ValiderPartiel:
                    imageName = "acceptPartiel.png";
                    break;
                case BoutonActionType.Coller:
                    imageName = "paste.png";
                    break;
                case BoutonActionType.Sauvegarder:
                    imageName = "Sauvegarde.png";
                    break;
                case BoutonActionType.Dupliquer:
                    imageName = "Duplication.png";
                    break;
                case BoutonActionType.Annuler:
                    imageName = "cancel.png";
                    break;
            }
            if (!String.IsNullOrEmpty(imageName))
            {
                var logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("../Images/" + imageName, UriKind.Relative);
                //new Uri("pack://application:,,,/WpfApplication;component/Images/" + imageName);
                logo.EndInit();
                _image.Source = logo;
                _image.Height = 16;
                _image.Width = 16;
                //_image.Source  = "../Images/" + imageName;
                //_image = new Image(new Uri("../Images/" + imageName, UriKind.Relative));
            }

        }

        

    }
}
