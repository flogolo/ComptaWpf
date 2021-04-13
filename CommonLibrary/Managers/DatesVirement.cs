using System;
using System.Collections.Generic;

namespace CommonLibrary.Managers
{
    public class DatesVirement
    {
        public DatesVirement()
        {
            DatesVirementList = new List<DateTime>();
        }

        public DateTime DebutMois { get; set; }
        public DateTime FinMois { get; set; }

        public List<DateTime> DatesVirementList { get; set; }
    }
}
