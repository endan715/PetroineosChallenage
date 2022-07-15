using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetroineosChallenage
{
    public interface IDataWriter
    {
        Task WriteAsync<T>(IEnumerable<T> records);
    }
}
