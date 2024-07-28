using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDRPG
{
    public class World
    {
        private Random RNG = new Random((int)DateTime.Now.Ticks);

        public const int Width = 64;
        public const int Height = 64;
        public Tile[][] TileData;
        public World()
        {
            TileData = new Tile[Width][];
            for (int column = 0; column < Width; column++)
            {
                TileData[column] = new Tile[Height];
            }
        }
        public Tile GroundTile;
        public Tile SandTile;
        private XNAInterface _xnaInterface;
        public World(XNAInterface xnaInterface)
        {
            _xnaInterface = xnaInterface;

            GroundTile = new Tile();
            GroundTile.Texture = _xnaInterface.LoadTexture("MDRPG.GroundTile.png");
            GroundTile.SeedChance = 0.05;
            GroundTile.SpreadChance = 1;

            SandTile = new Tile();
            SandTile.Texture = _xnaInterface.LoadTexture("MDRPG.SandTile.png");
            SandTile.SeedChance = 0.01;
            SandTile.SpreadChance = 1;
        }
        public void Generate()
        {
            int totalTileCount = Width * Height;
            int generatedCount = 0;


            TileData = new Tile[Width][];
            for (int x = 0; x < Width; x++)
            {
                TileData[x] = new Tile[Height];
                for (int y = 0; y < Height; y++)
                {
                    if (RNG.NextDouble() < GroundTile.SeedChance)
                    {
                        TileData[x][y] = GroundTile;
                    }
                    else if (RNG.NextDouble() < SandTile.SeedChance)
                    {
                        TileData[x][y] = SandTile;
                    }
                }
            }
        }
        public void Update()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Tile tile = TileData[x][y];
                    if (tile != null)
                    {
                        _xnaInterface.DrawTexture(tile.Texture, x * 16, y * 16);
                    }
                }
            }
        }
    }
}
