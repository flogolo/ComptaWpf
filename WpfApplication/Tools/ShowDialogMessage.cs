using MaCompta.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaCompta.Tools
{
    internal enum ShowDialogMessageEnum
    {
        OperationPredefinie,
        OperationsFiltrees,
        OperationChangeCompte,
        OperationFiltre,
        MessageAttente,
        FinAttente
    }

   internal class ShowDialogMessage
    {
        internal ShowDialogMessageEnum MessageType { get; set; }

        internal CompteViewModel CompteVM { get; set; }

        internal OperationViewModel SelectedOperation { get; set; }

        internal ShowDialogMessage(ShowDialogMessageEnum messageType)
        {
            MessageType = messageType;
        }
    }
}
