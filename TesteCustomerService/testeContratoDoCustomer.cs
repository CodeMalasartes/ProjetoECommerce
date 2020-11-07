using InfraEstrutura;
using InfraEstrutura.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TesteCustomerService
{
    [TestClass]
    public class testeContratoDoCustomer
    {
        private ICustomerService contrato;
        [TestMethod]
        public void Test_RegisterNewCustomer()
        {
            //Prepare
            contrato = new CustomerService(null);

            //Execute
            Guid id = Guid.NewGuid();

            var r1 = contrato.RegisterNewCustomer(new Customer {CustomerId = id, Name = "testeRR", NIF = "testeRR", Email = "testeRR", Country = "teste", UserName = "teste", PassHash = "teste", Logged = false});
            
            var r2 = contrato.RegisterNewCustomer(new Customer {CustomerId = id, Name = "testeRR", NIF = "testeRR", Email = "testeRR", Country = "teste", UserName = "teste", PassHash = "teste", Logged = false });

            var s1 = contrato.SelectCustomer(id); // 1 ou nulo

            var s2 = contrato.SelectCustomer("RR", "RR"); // 1 ou nulo

            var s3 = contrato.SelectCustomer("teste", "teste"); // 

            var r3 = contrato.DeleteCustomer(id);

            //Assert
            Assert.AreEqual(1, r1);

            Assert.IsNotNull(s1);

            Assert.IsNull(s2);

            Assert.IsNotNull(s3);

            Assert.AreEqual(-1, r2);

            Assert.AreEqual(1, r3);



        }
    }
}
