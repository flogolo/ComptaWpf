using System;
using System.Collections.Generic;
using System.Linq;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using CommonLibrary.Tools;

namespace CommonLibrary.Managers
{
    public class VirementsTools
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IVirementService _virementSrv;
        private readonly IOperationService _operationSrv;

        public event EventHandler<EventArgs<String>> LogMessageRequested;
        public event EventHandler VirementsAdded;

        public void InvokeVirementsAdded(EventArgs e)
        {
            EventHandler handler = VirementsAdded;
            if (handler != null) handler(this, e);
        }

        public VirementsTools(IVirementService virementSrv, IOperationService operationSrv)
        {
            _virementSrv = virementSrv;
            _operationSrv = operationSrv;
        }

        private static DateTime GetCurrentFinMois(DateTime maintenant)
        {
            //regarder la date
            //var maintenant = DateTime.Today;
            //var dateDebutMois = new DateTime(maintenant.Year, maintenant.Month, 1);
            //calculer la date du virement pour le mois en cours
            DateTime dateFinMois;
            if (maintenant.Month == 12)
            {
                dateFinMois = new DateTime(maintenant.Year, maintenant.Month, 31);
            }
            else
            {
                //1er jour du mois suivant        
                dateFinMois = new DateTime(maintenant.Year, maintenant.Month + 1, 1);
                //jour précédent = fin du mois
                dateFinMois = dateFinMois.AddDays(-1);
            }
            return dateFinMois;
        }

        private static DateTime GetDateCorrigee(int annee, int mois, int jour)
        {
            var date = new DateTime();
            try
            {
                date = new DateTime(annee, mois, jour);
            }
            catch (ArgumentOutOfRangeException)
            {
                //pb année bissextile?
                if (mois == 2 && jour > 28)
                {
                    date = new DateTime(annee, mois+1, 1);
                    date = date.AddDays(-1);
                }
            }
            return date;
        }

        public DateTime GetNextDateVirement(DateTime maintenant, int frequence, DateTime dateDernierVirement, int jour)
        {
            //date du virement à effectuer
            //DateTime dateVirement = DateTime.Today;
            DateTime dateVirement = maintenant;
            if (frequence == (int)FrequenceEnum.Mensuel)
            {
                    //le prochan virement se fera le mois prochain
                    int annee = dateDernierVirement.Year;
                    int mois = dateDernierVirement.Month;
                    if( mois == 12)
                    {
                        annee++;
                        mois = 1;
                    }
                    else
                    {
                        mois++;
                    }
                    dateVirement = GetDateCorrigee(annee, mois, jour);
            }
            else if (frequence == (int)FrequenceEnum.Annuel)
            {
                dateVirement = GetDateCorrigee(dateDernierVirement.Year, jour, jour);
            }
            else if (frequence == (int)FrequenceEnum.Hebdomadaire)
            {
                //le jour représente le jour de la semaine: 1=lundi, 7=dimanche
                //premier jour du mois
                dateVirement = new DateTime(dateDernierVirement.Year, dateDernierVirement.Month, 1);
                //recherche de la date correspondant au jour de la semaine
                while ((int)dateVirement.DayOfWeek != jour || dateVirement < dateDernierVirement)
                    dateVirement = dateVirement.AddDays(1);
            }
            return dateVirement;
        }
        /*
        public void EffectuerVirements(DateTime maintenant)
        {
            //var listVirements = new Collection<VirementModel>();
            //var operations = new List<OperationModel>();

            var dateFinMois = GetCurrentFinMois(maintenant);
            var hasModified = false;
            Log.Debug("EffectuerVirements " + maintenant + " -> " + dateFinMois);

            //Effectuer les virements configurés
            foreach (var virement in _VirementSrv.ItemsList)
            {
                Log.Debug("-> virement " + virement.Description);
                bool isModified = false;
                if (virement.Duree != 0)
                {
                    //Récupération de la date du dernier virement
                    //si aucun virement n'a jamais été effectué -> premier du mois
                    DateTime dateDernierVirement = virement.DateDernierVirement == null
                                                       ? new DateTime(maintenant.Year, maintenant.Month, 1)
                                                       : (DateTime)virement.DateDernierVirement;

                    Log.Debug("Date dernier virement =" + dateDernierVirement);

                    //date du virement à effectuer
                    DateTime dateVirement = GetNextDateVirement(maintenant,
                        virement.Frequence, dateDernierVirement, virement.Jour);

                    Log.Debug("Date prochain virement =" + dateVirement);

                    //incrémenter le mois si le virement n'a pas été fait ???
                    if (dateVirement <= dateDernierVirement)
                    {
                        dateVirement = dateVirement.AddMonths(1);
                    }

                    Log.Debug("Date prochain virement =" + dateVirement);

                    //si la date est passée
                    while (dateVirement <= dateFinMois
                           && dateVirement > dateDernierVirement)
                    {
                        //-1 => virement à durée indéfinie
                        //effectuer le virement
                        if (virement.CompteSrcId > 0)
                        {
                            //débit du compte source
                            var op = CreateOperation(dateVirement, virement.CompteSrcId, virement.Ordre,
                                                     virement.Details, true, virement.Montant);
                            if (op != null)
                            {
                                SendLogMessage("Ajout virements -> ajout d'une opération de débit " +
                                               op.DateOperation);
                                _OperationSrv.CreateOperationWithDetails(op);
                                isModified = true;
                            }
                        }
                        if (virement.CompteDstId > 0)
                        {
                            //crédit du compte destination
                            var op = CreateOperation(dateVirement, virement.CompteDstId, virement.Ordre,
                                                     virement.Details, false, virement.Montant);
                            if (op != null)
                            {
                                SendLogMessage("Ajout virements -> ajout d'une opération de crédit " +
                                               op.DateOperation);
                                _OperationSrv.CreateOperationWithDetails(op);
                                isModified = true;
                            }
                        }
                        //mettre à jour la date dernier virement => date virement
                        if (isModified)
                        {
                            virement.DateDernierVirement = dateVirement;
                            //mettre à jour la duree si elle n'est pas indéfinie
                            if (virement.Duree > 0)
                            {
                                virement.Duree = virement.Duree - 1;
                            }
                        }

                        switch (virement.Frequence)
                        {
                            case (int) FrequenceEnum.Mensuel:
                                dateVirement = dateVirement.AddMonths(1);
                                break;
                            case (int) FrequenceEnum.Hebdomadaire:
                                dateVirement = dateVirement.AddDays(7);
                                break;
                            default:
                                dateVirement = dateVirement.AddYears(1);
                                break;
                        }
                        Log.Debug("Date prochain virement =" + dateVirement);
                    }
                }
                //sauvegarde du virement (duree et date dernier ont été modifiés)
                if (isModified)
                {
                    _VirementSrv.UpdateItem(virement);
                    hasModified = true;
                }
            }

            //Sauvegarde des opérations effectuées
            if (!hasModified)
            {
                SendLogMessage("Aucun virement à effectuer");
            }
        }
        */
        private void SendLogMessage(string msg)
        {
            Log.Debug(msg);
            if (LogMessageRequested != null)
                LogMessageRequested(this, new EventArgs<String>(msg));
        }

        /// <summary>
        /// créer une opération de virement
        /// </summary>
        /// <param name="dateVirement"></param>
        /// <param name="compteId"></param>
        /// <param name="ordre"></param>
        /// <param name="detailsList"></param>
        /// <param name="aDebiter"></param>
        /// <param name="virementMontant">Montant à créditer/débiter (si == 0, rechercher le mois associé)</param>
        /// <param name="typePaiement">type de paiement</param>
        /// <returns></returns>
        private static OperationModel CreateOperation(DateTime dateVirement, long compteId, string ordre, IEnumerable<VirementDetailModel> detailsList, bool aDebiter, decimal virementMontant, string typePaiement)
        {
            var operation = new OperationModel
                                {
                                    DateOperation = dateVirement,
                                    CompteId = compteId,
                                    Ordre = ordre,
                                    TypePaiement = typePaiement,
                                    Details = new List<DetailModel>(),
                                    IsVirementAuto = true
                                };
            decimal montantTotal = 0;
            foreach (var vrtdetail in detailsList)
            {
                if (aDebiter && vrtdetail.IsCompteSrcOnly
                    || !aDebiter && vrtdetail.IsCompteDstOnly
                    || (!vrtdetail.IsCompteDstOnly && !vrtdetail.IsCompteSrcOnly))
                {
                    var detail = new DetailModel
                                     {
                                         Commentaire = vrtdetail.Commentaire,
                                         RubriqueId = vrtdetail.RubriqueId,
                                         SousRubriqueId = vrtdetail.SousRubriqueId,
                                         OperationId = 1,
                                     };
                    decimal montant = 0;
                    if (virementMontant != 0)
                        //prendre le montant passé en paramètre
                        montant = virementMontant;
                    else
                    {
                        //chercher le montant du mois concerné
                        var vm = vrtdetail.Montants.FirstOrDefault(m => m.Mois == dateVirement.Month);
                        if (vm != null && vm.Montant != 0)
                            montant = vm.Montant;
                    }

                    if (montant != 0)
                    {
                        if (aDebiter)
                        {
                            detail.Montant = -montant;
                            detail.MontantBudget = -montant;
                        }
                        else
                        {
                            detail.Montant = montant;
                            detail.MontantBudget = montant;
                        }
                        operation.Details.Add(detail);
                    }
                    montantTotal += montant;
                }
            }
            operation.MontantBudget = montantTotal;
            if (operation.Details.Any(d => d.Montant != 0))
                return operation;
            //Log.Warn("CreateOperation non efectué : tous les détails ont un montant null");
            return null;
        }

        public List<DatesVirement> GetAllMonths(DateTime dernierVirement, int duree, DateTime jusqua, int jour, FrequenceEnum frequence)
        {
            //System.Diagnostics.Debug.WriteLine("-> GetAllMonths dernier={0} durée={1} jusqua={2}", dernierVirement, duree, jusqua);
            int mois = dernierVirement.Month;
            int annee = dernierVirement.Year;

            var listeDates = new List<DatesVirement>();

            //premier jour du mois
            var premierJour = new DateTime(annee, mois, 1);
            //premier jour du mois suivant
            var suivant = premierJour.AddMonths(1);
            //dernier jour du mois
            var dernierJour = suivant.AddDays(-1);
            int nb = 0;
            while (( duree ==-1 || nb<duree) && premierJour <= jusqua)
            {
                var moisCourant = new DatesVirement {DebutMois = premierJour, FinMois = dernierJour};
                //recherche des dates de virement
                var dates = GetDatesVirementForMonth(moisCourant, frequence, jour);
                //si on trouve des virements -> ok
                if (dates.Count > 0)
                {
                    listeDates.Add(moisCourant);
                    foreach (var dateTime in dates)
                    {
                        //si on trouve des virements non effectués
                        if (dateTime > dernierVirement)
                        {
                            moisCourant.DatesVirementList.Add(dateTime);
                            nb++;                            
                        }
                    }
                }
                
                //System.Diagnostics.Debug.WriteLine(premierJour+":"+dernierJour);
                premierJour = suivant;
                suivant = premierJour.AddMonths(1);
                dernierJour = suivant.AddDays(-1);
            }
            return listeDates;
        }

        public List<DateTime> GetDatesVirementForMonth(DatesVirement datesMois, FrequenceEnum frequence, int jour)
        {
            //date du virement à effectuer
            //DateTime dateVirement = DateTime.Today;
            var dates = new List<DateTime>();
            switch (frequence)
            {
                case FrequenceEnum.Mensuel:
                    int annee = datesMois.DebutMois.Year;
                    int mois = datesMois.DebutMois.Month;
                    dates.Add(GetDateCorrigee(annee, mois, jour));
                    break;
                case FrequenceEnum.Annuel:
                    //le jour représente le mois du virement
                    //si 0 -> janvier à 11 -> décembre
                    if ((jour+1) == datesMois.DebutMois.Month)
                        dates.Add(GetDateCorrigee(datesMois.DebutMois.Year, jour+1, jour+1));
                    break;
                case FrequenceEnum.Hebdomadaire:
                    //le jour représente le jour de la semaine: 1=lundi, 7=dimanche
                    //premier jour du mois
                    DateTime date = datesMois.DebutMois;
                    //recherche de la date correspondant au jour de la semaine
                    while (date <= datesMois.FinMois)
                    {
                        if ((int) date.DayOfWeek == jour)
                            dates.Add(date);
                        date = date.AddDays(1);
                    }
                    break;
            }
            return dates;
        }

        public void EffectuerVirementsNew(DateTime maintenant)
        {
            //var listVirements = new Collection<VirementModel>();
            //var operations = new List<OperationModel>();
            try
            {
                var dateFinMois = GetCurrentFinMois(maintenant);
                var hasModified = false;
                string addSrcMessage;
                string addDstMessage;
                SendLogMessage("EffectuerVirements " + maintenant + " -> " + dateFinMois);

                //Effectuer les virements configurés
                foreach (var virement in _virementSrv.ItemsList)
                {
                    bool isModified = false;
                    addSrcMessage = "";
                    addDstMessage = "";
                    if (virement.Duree != 0)
                    {
                        SendLogMessage("Traitement virement " + virement.Description);

                        _operationSrv.BeginTransaction();
                        //Récupération de la date du dernier virement
                        //si aucun virement n'a jamais été effectué -> premier du mois
                        DateTime dateDernierVirement = virement.DateDernierVirement == null
                                                           ? new DateTime(maintenant.Year, maintenant.Month, 1)
                                                           : (DateTime)virement.DateDernierVirement;

                        //Log.Debug("Date dernier virement =" + dateDernierVirement);

                        //récupération de tous les mois 
                        var allMonths = GetAllMonths(dateDernierVirement, virement.Duree, dateFinMois, virement.Jour,
                                                     (FrequenceEnum)virement.Frequence);

                        foreach (var datesVirement in allMonths)
                        {
                            foreach (var dateVirement in datesVirement.DatesVirementList)
                            {
                                //si le virement n'a pas encore été effectué: normalement toujours vrai
                                OperationModel opSrc = null, opDst = null;
                                if (dateVirement > dateDernierVirement)
                                {
                                    //Log.Debug("Date prochain virement =" + dateVirement);
                                    //effectuer le virement
                                    if (virement.CompteSrcId > 0)
                                    {
                                        //débit du compte source
                                        opSrc = CreateOperation(dateVirement, virement.CompteSrcId, virement.Ordre,
                                                                 virement.Details, true, virement.Montant, virement.TypePaiement);
                                        if (opSrc != null)
                                        {
                                            if (addSrcMessage != null)
                                            {
                                                addSrcMessage += System.Environment.NewLine;
                                            }
                                            addSrcMessage += "Ajout virements -> ajout d'une opération de débit " +
                                                           opSrc.DateOperation;
                                            _operationSrv.CreateOperationWithDetails(opSrc);
                                            isModified = true;
                                        }
                                    }

                                    if (virement.CompteDstId > 0)
                                    {
                                        //crédit du compte destination
                                        opDst = CreateOperation(dateVirement, virement.CompteDstId, virement.Ordre,
                                                                 virement.Details, false, virement.Montant, virement.TypePaiement);
                                        if (opDst != null)
                                        {
                                            if (addDstMessage != null)
                                            {
                                                addDstMessage += System.Environment.NewLine;
                                            }
                                            addDstMessage += "Ajout virements -> ajout d'une opération de crédit " +
                                                         opDst.DateOperation;
                                            _operationSrv.CreateOperationWithDetails(opDst);
                                            isModified = true;
                                        }
                                    }
                                    
                                    //mettre à jour la date dernier virement => date virement
                                    if (isModified)
                                    {
                                        if(opSrc != null && opDst != null)
                                        {
                                            //2 opérations créées -> on fait le lien entre elles
                                            opSrc.LienOperationId = opDst.Id;
                                            opDst.LienOperationId = opSrc.Id;
                                            foreach (var srcDetail in opSrc.Details)
                                            {
                                                var dstDetail = opDst.Details.FirstOrDefault(d => d.RubriqueId == srcDetail.RubriqueId
                                                    && d.SousRubriqueId == srcDetail.SousRubriqueId);
                                                if (dstDetail != null)
                                                {
                                                    dstDetail.LienDetailId = srcDetail.Id;
                                                    srcDetail.LienDetailId = dstDetail.Id;
                                                }
                                            }
                                            _operationSrv.UpdateOperationWithDetails(opSrc);
                                            _operationSrv.UpdateOperationWithDetails(opDst);
                                        }
                                        virement.DateDernierVirement = dateVirement;
                                        //mettre à jour la duree si elle n'est pas indéfinie
                                        if (virement.Duree > 0)
                                        {
                                            virement.Duree = virement.Duree - 1;
                                        }
                                    }
                                }
                            }
                        }
                        _operationSrv.EndTransaction();
                        //sauvegarde du virement (duree et date dernier ont été modifiés)
                        if (isModified)
                        {
                            _virementSrv.UpdateItem(virement, false);
                            hasModified = true;
                        }
                        if(!string.IsNullOrEmpty(addSrcMessage))
                        {
                            SendLogMessage(addSrcMessage);
                        }
                        if(!string.IsNullOrEmpty(addDstMessage))
                        {
                            SendLogMessage(addDstMessage);
                        }
                    }
                }

                SupprimerReport(maintenant, dateFinMois);
                //fin de traitement
                if (!hasModified)
                {
                    SendLogMessage("Aucun virement à effectuer");
                }
                else
                {
                    SendLogMessage("Virements terminé");
                    InvokeVirementsAdded(EventArgs.Empty);
                }
            }
            catch (Exception e)
            {
                SendLogMessage("Une erreur s'est produite: "+e.Message);
            }
        }

        /// <summary>
        /// supprime la propriété report sur les opérations du mois précédent 
        /// qui avaient été reportées
        /// </summary>
        /// <param name="maintenant"></param>
        private void SupprimerReport(DateTime maintenant, DateTime finDeMois)
        {
            SendLogMessage("Suppression report...");
            var dateDebutMois = new DateTime(maintenant.Year, maintenant.Month, 1);
            foreach (var op in _operationSrv.AllOperations.Where(o => o.DateOperation >= dateDebutMois
            && o.DateOperation < finDeMois && o.Report))
            {
                op.Report = false;
                _operationSrv.UpdateItem(op, false);
            }
            SendLogMessage("Suppression report ok");
        }
    }
}