using CommonLibrary.Models;
using CommonLibrary.Services.Interfaces;
using DataAccessMock;
using NUnit.Framework;

namespace TestCompta
{
    [TestFixture]
    public class TestOperations
    {
        private IOperationService _OperationMock;

        /// <summary>
        ///Initialize() is called once during test execution before
        ///test methods in this test class are executed.
        ///</summary>
        [SetUp]
        public void Initialize()
        {
            WpfIocFactoryMock.Instance.Configure();
            _OperationMock = WpfIocFactoryMock.Instance.Container.Resolve<IOperationService>();
        }

        [Test]
        public void TestFindCheque()
        {
            _OperationMock.AllOperations.Add(new OperationModel {
                Id = 1,
                NumeroCheque = "123456"
            });
            _OperationMock.AllOperations.Add(new OperationModel
            {
                Id = 2,
                NumeroCheque = "012345"
            }
            );

            var result = _OperationMock.FindCheque("123");
            Assert.AreEqual("123457", result);

            result = _OperationMock.FindCheque("012");
            Assert.AreEqual("012346", result);
        }
    }
}
