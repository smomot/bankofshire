using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedComponents
{
    public interface IInspector
    {

        IAsyncEnumerable<string> GetFullSummary();
        Task<string> StartInspection();
        Task<string> FinishInspection();
    }
}
