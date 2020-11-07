using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraEstrutura.Services
{
    public class EFCustomerService : ICustomerService
    {
        private readonly DbContext _dd;
        // preciso de instalar vários packages
        public EFCustomerService(DbContext dd)
        {
            _dd = dd;
        }
        public int AddCustomerRole(Guid id, string rolename)
        {
            throw new NotImplementedException();
        }

        public int DelCustomerRole(Guid id, string roleName)
        {
            throw new NotImplementedException();
        }

        public int DeleteCustomer(Guid id)
        {
            throw new NotImplementedException();
        }

        public int RegisterNewCustomer(Customer item)
        {
            _dd.Add<Customer>(item);
            _dd.SaveChanges();
            return 1;
        }

        public Customer SelectCustomer(Guid id)
        {
            throw new NotImplementedException();
        }

        public Customer SelectCustomer(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public ICollection<Customer> SelectCustomers()
        {
            throw new NotImplementedException();
        }

        public int UpdateCustomer(Customer item)
        {
            throw new NotImplementedException();
        }
    }
}
