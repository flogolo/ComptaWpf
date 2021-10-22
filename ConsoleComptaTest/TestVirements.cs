using System;
using System.Collections.Generic;
using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using DataAccessMock;
using CommonLibrary.Managers;
using CommonLibrary.Tools;

namespace ConsoleComptaTest
{
    public class TestVirements
    {
        /// <summary>
        ///Initialize() is called once during test execution before
        ///test methods in this test class are executed.
        ///</summary>
        public void Initialize()
        {
            WpfIocFactoryMock.Instance.Configure();
        }

        public void TestEffectuerVirements()
        {
            var virementMock = WpfIocFactoryMock.Instance.Container.Resolve<IVirementService>();
            var operationMock = WpfIocFactoryMock.Instance.Container.Resolve<IOperationService>();

            var manager = new VirementsTools(virementMock, operationMock);

            var maintenant = new DateTime(2014,5,2);
            //var dernier = new DateTime(maintenant.Year, maintenant.Month, 1);
            //var date = manager.GetNextDateVirement((int)FrequenceEnum.Hebdomadaire, dernier,
            //                                       (int)DayOfWeek.Saturday);
            //while (date.Month == dernier.Month)
            //{
            //    System.Diagnostics.Debug.WriteLine(date.DayOfWeek + ":" + date);
            //    date = date.AddDays(7);
            //}

            virementMock.ItemsList.Add(new VirementModel
                                           {
                                               CompteSrcId = 2,
                                               CompteDstId = 0,
                                               Frequence = (int) FrequenceEnum.Hebdomadaire,
                                               Duree = -1,
                                               Jour = 6,
                                               Ordre = "SuperU",
                                               Montant = 10,
                                               DateDernierVirement = new DateTime(2014,4,25),
                                               Details = new List<VirementDetailModel>
                                                             {
                                                                 new VirementDetailModel
                                                                     {RubriqueId = 1, SousRubriqueId = 1}
                                                             }
                                           });
            manager.LogMessageRequested += ManagerLogMessageRequested;
            manager.EffectuerVirementsNew(maintenant);
            //Assert.AreEqual(8, operationMock.ItemsList.Count);
        }

        static void ManagerLogMessageRequested(object sender, EventArgs<string> e)
        {
            System.Diagnostics.Debug.WriteLine(e.Data);
        }

        public void TestDates()
        {
            var virementMock = WpfIocFactoryMock.Instance.Container.Resolve<IVirementService>();
            var operationMock = WpfIocFactoryMock.Instance.Container.Resolve<IOperationService>();

            var manager = new VirementsTools(virementMock, operationMock);
            manager.GetAllMonths(new DateTime(2014,3,25), -1, DateTime.Now , 5,FrequenceEnum.Mensuel);        
        }

    }
}
