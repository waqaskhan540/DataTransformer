using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.Parsers
{
    public interface  ICsvParser<T>
    {
        IEnumerable<IEnumerable<T>> ParseInBatches(string file, int batchSize);
        IEnumerable<T> Parse(string file);
    }
}
