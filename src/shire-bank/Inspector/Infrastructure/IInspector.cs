using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectorClient.Infrastructure
{
    public interface IInspector
    {
        Task<string> GetFullSummary();
        Task StartInspection();
        Task FinishInspection();
    }
}
