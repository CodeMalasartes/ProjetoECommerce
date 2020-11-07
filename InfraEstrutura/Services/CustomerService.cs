using Dapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace InfraEstrutura.Services
{
    public class CustomerService : ICustomerService
    {
        private string _user = "Anónimo";
        private string _connection = "Data Source=ea-sql.eastus2.cloudapp.azure.com;Initial Catalog=ECommerceDB;Persist Security Info=True;User ID=sa;Password=Pa55w.rd";
        public CustomerService(IHttpContextAccessor context)  //Coockies name Servidor--Copia deste cookie          
        {        
            if (context != null) 
            {
                if (context.HttpContext.User.Identity.IsAuthenticated)  //quando gerir a sessao, verifica se o user esta identificado e autenticado 
                {
                    _user = context.HttpContext.User.Identity.Name;
                    
                }
            }           
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
            using (IDbConnection db = new SqlConnection(_connection))
            {
                return db.Execute("DELETE FROM Customers WHERE CustomerId=@id", new { id=id });
            }
        }
        
        public int RegisterNewCustomer(Customer item)
        {
            // Validar se o Customer não está repetido
            //item.CustomerId = Guid.NewGuid();
            // Pesquisar a lista sobre os critérios SAFT- username, nif, email
            string txt = "SELECT * FROM Customers WHERE Username=@UserName OR NIF=@NIF OR Email=@Email"; 
            // caso estes critérios não sejam infrigidos
            
            using (IDbConnection db = new SqlConnection(_connection))
            {
                Customer r = db.QueryFirstOrDefault<Customer>(txt, item);
                if (r == null)
                {
                    txt = "INSERT INTO Customers (CustomerId,Name,Email,NIF,UserName,PassHash,Country,Logged)" + 
                        "VALUES (@CustomerId,@Name,@Email,@NIF,@UserName,@PassHash,@Country,@Logged)";
                    item.PassHash = CodificaPassword(item.PassHash);
                    return db.Execute(txt, item);
                }
            }
            return -1;
            //então, nessa caso, adiciona
        }
        public Customer SelectCustomer(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connection))
            {
                return db.QueryFirstOrDefault<Customer>("SELECT * FROM Customers WHERE Customerid=@id",
                    new { id = id });
            }
        }
        private string CodificaPassword(string password) //Encriptar pass
            // Não usar MD5 na encriptação da palavra pass

            // AZURE KEY VAULT -sem o certificado nao conseguimos desencriptar o codigo
        {
            var saltBytes = Encoding.UTF8.GetBytes("Saudade");//Aplicação
            string resultado = "";

            var hashBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 8

                );

            //for (int i = 0; i < 10000000; i++)
            //{
            //Letras Mn Num Car 
            //}
            resultado = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            return resultado;
        }
        public Customer SelectCustomer(string userName, string password)
        {
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var c = db.QueryFirstOrDefault<Customer>("SELECT * FROM Customers WHERE UserName=@UserName AND PassHash = @PassHash", 
                    new { UserName= userName, PassHash= CodificaPassword(password) });
                c.CustomerRoles.Add(new CustomerRole { RoleName = "Guest" });
                return c;
            }
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
