using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedComponents
{
    public interface ICustomer
    {
        Task<uint?> OpenAccount(string firstName, string lastName, float debtLimit);

        Task<float?> Withdraw(uint accountId, float amount);

        Task Deposit(uint accountId, float amount);

        Task<string> GetHistory(uint accountId);

        Task<bool> CloseAccount(uint accountId);
    }
}
