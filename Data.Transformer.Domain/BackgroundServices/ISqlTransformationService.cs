using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Transformer.Domain.BackgroundServices
{
    public interface ISqlTransformationService
    {
        Task Start();
    }
}
