using System;

namespace MaCompta.Controls
{
    public interface IViewModel
    {
        String Libelle {get; }
        bool IsSelected { get; set; }
    }
}
