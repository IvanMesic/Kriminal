using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.ServiceInterfaces
{
    public interface ITransactionService
    {
        Transaction CreateTransaction(int userId);
        IQueryable<Transaction> GetUserTransactions(int userId);
        Transaction GetTransactionById(int id); 
    }
}
