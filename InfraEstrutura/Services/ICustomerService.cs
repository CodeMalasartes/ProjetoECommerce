using System;
using System.Collections.Generic;
using System.Text;

namespace InfraEstrutura.Services
{
    public interface ICustomerService   // Contrato Assinado
    {
        int RegisterNewCustomer(Customer item);
        ICollection<Customer> SelectCustomers();
        Customer SelectCustomer(Guid id);

        Customer SelectCustomer(string userName, string password);
        int DeleteCustomer(Guid id);
        int UpdateCustomer(Customer item);

        int AddCustomerRole(Guid id, string rolename);
        int DelCustomerRole(Guid id, string roleName);
        
            
    }
}
