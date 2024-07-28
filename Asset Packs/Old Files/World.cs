using System.Collections.Generic;
namespace MDRPG
{
    public class WorldMeta
    {
        public string name = null;
        public DementionMeta startingDemention = null;
        public List<DementionMeta> dementions = new List<DementionMeta>();
    }
}