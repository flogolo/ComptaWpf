using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.Tools
{
    public class ErrorEventArgs: EventArgs
    {
        public string ErrorMessage { get; set; }

        public ErrorEventArgs( string message)
        {
            ErrorMessage = message;
        }
    }
}
