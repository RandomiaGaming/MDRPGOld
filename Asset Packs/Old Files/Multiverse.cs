using System.Collections.Generic;
namespace MDRPG
{
    public class Multiverse
    {
        public string Name;
        public List<BiomeMeta> Biomes = new List<BiomeMeta>();
        public List<MDRPG_Object> Objects = new List<MDRPG_Object>();
        public List<DementionMeta> Dementions = new List<DementionMeta>();
        public List<MDRPG_Item> Items = new List<MDRPG_Item>();
    }
}