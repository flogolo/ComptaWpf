using CommonLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace CommonLibrary.Tools
{
    public static class PaiementHelper
    {
        static PaiementHelper()
        {
            PaiementList = new List<PaiementViewModel>
                               {
                                   new PaiementViewModel{EnumType=EnumPaiement.CB, Code="CB", Designation="Carte bancaire"},
                                   new PaiementViewModel{EnumType=EnumPaiement.CHQ, Code="CHQ", Designation="Chèque"},
                                   new PaiementViewModel{EnumType=EnumPaiement.VRT, Code="VRT", Designation="Virement"},
                                   new PaiementViewModel{EnumType=EnumPaiement.CSH, Code="CSH", Designation="Liquide"},
                                   new PaiementViewModel{EnumType=EnumPaiement.PLV, Code="PLV", Designation="Prélèvement"},
                                   new PaiementViewModel{EnumType=EnumPaiement.TIP, Code="TIP", Designation="TIP"},
                               };
        }

        public static List<PaiementViewModel> PaiementList
        {
            get;
            set;
        }

        public static IEnumerable<string> TypesPaiement
        {
            get { return from paiement in PaiementList orderby paiement.Designation select paiement.Designation; }
        }

        ///// <summary>
        ///// types de paiement possibles
        ///// </summary>
        //public static Dictionary<string, string> TypesPaiement
        //{
        //    get
        //    {
        //        return new Dictionary<string, string>
        //            {
        //                //{"", ""},
        //                {"CB", "Carte bancaire"},
        //                {"CHQ", "Chèque"},
        //                {"CSH", "Liquide"},
        //                {"PLV", "Prélèvement"},
        //                {"VRT", "Virement"},
        //                {"TIP", "TIP"},
        //            };
        //    }
        //}

        public static string GetCodePaiement(string paiement)
        {
            var paiementViewModel = PaiementList.FirstOrDefault(p => p.Designation == paiement);
            if (paiementViewModel != null)
                return paiementViewModel.Code;
            return null;
        }

        public static string GetPaiement(string code)
        {
            var paiementViewModel = PaiementList.FirstOrDefault(p => p.Code == code);
            if (paiementViewModel != null)
                return paiementViewModel.Designation;
            return null;
        }

        public static bool IsCheque(string paiement)
        {
            var paiementViewModel = PaiementList.FirstOrDefault(p => p.EnumType == EnumPaiement.CHQ);
            return paiementViewModel != null && paiement.Equals(paiementViewModel.Designation);
        }
        /// <summary>
        /// Recherche la prochaine valeur de chèque correspondant au chiffre saisi
        /// </summary>
        /// <param name="value">début d'un numéro de chèque</param>
        /// <returns></returns>
        public static string FindCheque(string value, List<OperationModel> allOperations)
        {
            var item = allOperations.Where(o => o.NumeroCheque != null
                && o.NumeroCheque.Length >= value.Length
                && o.NumeroCheque.Substring(0, value.Length).Equals(value)).OrderByDescending(o => o.NumeroCheque).FirstOrDefault();
            if (item != null)
            {
                bool withZero = false;
                if (value.StartsWith("0"))
                {
                    withZero = true;
                }
                int numero;
                if (int.TryParse(item.NumeroCheque, out numero))
                {
                    numero++;
                    return withZero ? "0" + numero.ToString() : numero.ToString();
                }
            }
            return value;
        }
    }

    public enum EnumPaiement
    {
        CHQ,
        VRT,
        CB,
        CSH,
        PLV,
        TIP
    }

    public class PaiementViewModel
    {
        public EnumPaiement EnumType { get; set; }
        public string Code { get; set; }
        public string Designation { get; set; }
    }
}
