using System;
using System.Collections.Generic;
using System.Linq;
using CommonLibrary.Managers;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using DataAccessMock;
using CommonLibrary.Tools;
using NUnit.Framework;

namespace TestCompta
{
    [TestFixture]
    public class TestVirements
    {
        private VirementsTools _Manager;
        private IVirementService _VirementMock;
        private IOperationService _OperationMock;

        /// <summary>
        ///Initialize() is called once during test execution before
        ///test methods in this test class are executed.
        ///</summary>
        [SetUp]
        public void Initialize()
        {
            WpfIocFactoryMock.Instance.Configure();
            _VirementMock = WpfIocFactoryMock.Instance.Container.Resolve<IVirementService>();
            _OperationMock = WpfIocFactoryMock.Instance.Container.Resolve<IOperationService>();

            _Manager = new VirementsTools(_VirementMock, _OperationMock);
        }

        [Test]
        public void TestEffectuerVirementsHebdo()
        {
            var maintenant = new DateTime(2014,7,1);
            _VirementMock.ItemsList.Clear();
            _OperationMock.ItemsList.Clear();
            _VirementMock.ItemsList.Add(new VirementModel
                                           {
                                               CompteSrcId = 2,
                                               CompteDstId = 0,
                                               Frequence = (int) FrequenceEnum.Hebdomadaire,
                                               Duree = -1,
                                               Jour = 6,
                                               Ordre = "SuperU",
                                               Montant = 10,
                                               DateDernierVirement = new DateTime(2014,6,28),
                                               Details = new List<VirementDetailModel>
                                                             {
                                                                 new VirementDetailModel
                                                                     {RubriqueId = 1, SousRubriqueId = 1}
                                                             }
                                           });
            _Manager.LogMessageRequested += ManagerLogMessageRequested;
            _Manager.EffectuerVirementsNew(maintenant);
            Assert.AreEqual(4, _OperationMock.ItemsList.Count);
        }

        static void ManagerLogMessageRequested(object sender, EventArgs<string> e)
        {
            System.Diagnostics.Debug.WriteLine(e.Data);
        }

        [Test]
        public void TestEffectuerVirementMensuel()
        {
            var maintenant = new DateTime(2014, 7, 1);

            _VirementMock.ItemsList.Clear();
            _OperationMock.ItemsList.Clear();
            _VirementMock.ItemsList.Add(new VirementModel
            {
                CompteSrcId = 2,
                CompteDstId = 0,
                Frequence = (int)FrequenceEnum.Mensuel,
                Duree = 1,
                Jour = 12,
                Ordre = "Banque",
                Montant = 2000,
                DateDernierVirement = new DateTime(2014, 6, 13),
                Details = new List<VirementDetailModel>
                                                             {
                                                                 new VirementDetailModel
                                                                     {RubriqueId = 1, SousRubriqueId = 1}
                                                             }
            });
            _Manager.LogMessageRequested += ManagerLogMessageRequested;
            _Manager.EffectuerVirementsNew(maintenant);
            Assert.AreEqual(1, _OperationMock.ItemsList.Count);
        }

        [Test]
        public void TestEffectuerVirementAvecLien()
        {
            var maintenant = new DateTime(2014, 7, 1);

            _VirementMock.ItemsList.Clear();
            _OperationMock.ItemsList.Clear();
            _VirementMock.ItemsList.Add(new VirementModel
            {
                CompteSrcId = 2,
                CompteDstId = 1,
                Frequence = (int)FrequenceEnum.Mensuel,
                Duree = 1,
                Jour = 12,
                Ordre = "Banque",
                Montant = 2000,
                DateDernierVirement = new DateTime(2014, 6, 13),
                Details = new List<VirementDetailModel>
                                                             {
                                                                 new VirementDetailModel
                                                                     {RubriqueId = 1, SousRubriqueId = 1}
                                                             }
            });
            _Manager.LogMessageRequested += ManagerLogMessageRequested;
            _Manager.EffectuerVirementsNew(maintenant);
            Assert.AreEqual(2, _OperationMock.ItemsList.Count);
            var op1 = _OperationMock.ItemsList.FirstOrDefault(i=>i.CompteId == 1);
            Assert.IsNotNull(op1);
            var op2 = _OperationMock.ItemsList.FirstOrDefault(i => i.CompteId == 2);
            Assert.IsNotNull(op2);
            Assert.AreEqual(op1.Id, op2.LienOperationId);
            Assert.AreEqual(op2.Id, op1.LienOperationId);
        }
        [Test]
        public void TestDates()
        {
            //tous les mois jusqu'à la date de fin
//            var liste = _Manager.GetAllMonths(new DateTime(2014, 3, 25), -1, new DateTime(2014,06,17));
//            Assert.AreEqual(4, liste.Count);
//
            //tous les mois jusqu'à la date de fin
//            liste = _Manager.GetAllMonths(new DateTime(2014, 3, 25), 10, new DateTime(2014, 06, 17));
//            Assert.AreEqual(4, liste.Count);
//
            //2 échéances
//            liste = _Manager.GetAllMonths(new DateTime(2014, 3, 25), 2, new DateTime(2014, 06, 17));
//            Assert.AreEqual(2, liste.Count);
//
            //aucune échéance
//            liste = _Manager.GetAllMonths(new DateTime(2014, 3, 25), 0, new DateTime(2014, 06, 17));
//            Assert.AreEqual(0, liste.Count);
        }

        [Test]
        public void TestDatesVirement()
        {
            //pour le mois de juin
            var dates = new DatesVirement{DebutMois = new DateTime(2014,6,1), FinMois = new DateTime(2014,6,30)};

            //1 seul virement mensuel
            var datesVirements = _Manager.GetDatesVirementForMonth(dates, FrequenceEnum.Mensuel, 16);
            Assert.AreEqual(1, datesVirements.Count);
            var dateVirement = datesVirements.First();
            Assert.AreEqual(dateVirement.Day, 16);

            //pas de virement annuel
            datesVirements = _Manager.GetDatesVirementForMonth(dates, FrequenceEnum.Annuel, 8);
            Assert.AreEqual(0, datesVirements.Count);

            //1 virement annuel
            datesVirements = _Manager.GetDatesVirementForMonth(dates, FrequenceEnum.Annuel, 5);
            Assert.AreEqual(1, datesVirements.Count);
            dateVirement = datesVirements.First();
            Assert.AreEqual(dateVirement.Day, 6);

            //4 virements hebdo
            datesVirements = _Manager.GetDatesVirementForMonth(dates, FrequenceEnum.Hebdomadaire, 6);
            Assert.AreEqual(4, datesVirements.Count);
            foreach (var virement in datesVirements)
            {
                Assert.AreEqual(6, (int) virement.DayOfWeek);
            }
        }
        [Test]
        public void TestDatesVirementFevrier()
        {
            //pour le mois de juin
            var dates = new DatesVirement { DebutMois = new DateTime(2014, 2, 1), FinMois = new DateTime(2014, 2, 28) };

            //1 seul virement mensuel
            var datesVirements = _Manager.GetDatesVirementForMonth(dates, FrequenceEnum.Mensuel, 30);
            Assert.AreEqual(1, datesVirements.Count);
            var dateVirement = datesVirements.First();
            Assert.AreEqual(dateVirement.Day, 28);
        }

        [Test]
        public void TestDatesSemaine()
        {
            var virementMock = WpfIocFactoryMock.Instance.Container.Resolve<IVirementService>();
            var operationMock = WpfIocFactoryMock.Instance.Container.Resolve<IOperationService>();

            var manager = new VirementsTools(virementMock, operationMock);
            //mois de février, mardi
            var dates = manager.GetAllMonths(new DateTime(2017, 2, 2), -1, new DateTime(2017, 2, 28), (int)DayOfWeek.Tuesday, FrequenceEnum.Hebdomadaire);
            Assert.AreEqual(1, dates.Count);
            var result = dates.First();
            Assert.AreEqual(4, result.DatesVirementList.Count);
            Assert.AreEqual(28, result.FinMois.Day);
            Assert.AreEqual(7, result.DatesVirementList.ElementAt(0).Day);
            Assert.AreEqual(14, result.DatesVirementList.ElementAt(1).Day);
            Assert.AreEqual(21, result.DatesVirementList.ElementAt(2).Day);
            Assert.AreEqual(28, result.DatesVirementList.ElementAt(3).Day);

        }
    }
}
