using System;
using System.Collections;
using System.Collections.Generic;

namespace InfraEstrutura
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string NIF { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PassHash { get; set; } // naõ devemos guardar a passaword mas sim a sua hash
        public ICollection<CustomerRole> CustomerRoles { get; set; } = new List<CustomerRole>(); // claims
        public bool Logged { get; set; } = false;
    }
}
