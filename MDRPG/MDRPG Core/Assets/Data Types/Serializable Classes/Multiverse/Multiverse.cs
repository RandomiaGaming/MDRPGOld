using System.Collections.Generic;
namespace MDRPG
{
    public class AssetPack
    {
        public string Name;
        public List<MDRPG_Biome> Biomes = new List<MDRPG_Biome>();
        public List<MDRPG_Object> Objects = new List<MDRPG_Object>();
        public List<MDRPG_Demention> Dementions = new List<MDRPG_Demention>();
        public List<MDRPG_Item> Items = new List<MDRPG_Item>();
    }
}