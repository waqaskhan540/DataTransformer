using Data.Transformer.Domain.Configuration;

namespace Data.Transformer.Domain.FileStorage
{
    public class JsonJobStorage : FileStorage , IJsonJobStorage
    {
        public JsonJobStorage(IJobConfig config):base(config)
        {

        }
    }
}
