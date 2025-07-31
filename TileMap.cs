using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDRPG
{
    public sealed class TileMap
    {
        private readonly XNAInterface _xnaInterface;
        public TileMap(XNAInterface xnaInterface)
        {
            if(_xnaInterface is null)
            {
                throw new Exception("xnaInterface may not be null.");
            }
            _xnaInterface = xnaInterface;
        }
    }
    public struct TileData
    {
        public short Temperature;
        public Texture2D Texture;
    }
    public class Tile
    {
        public double SeedChance;
        public double SpreadChance;
        public Texture2D Texture;
    }
}
