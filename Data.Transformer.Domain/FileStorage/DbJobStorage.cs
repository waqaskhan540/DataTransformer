using Data.Transformer.Domain.Configuration;

namespace Data.Transformer.Domain.FileStorage
{
    public class DbJobStorage : FileStorage , IDbJobStorage
    {
        public DbJobStorage(IJobConfig config) : base(config)
        {

        }
    }
}
