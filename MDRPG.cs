using Microsoft.Xna.Framework.Graphics;

namespace MDRPG
{
    public sealed class MDRPG
    {
        public readonly XNAInterface xnaInterface;

        public Texture2D playerTexture;
        public Player player;
        public World world;
        public MDRPG()
        {
            xnaInterface = new XNAInterface(Update);
        }
        public void Run()
        {
            Initialize();

            xnaInterface.Run();
        }
        private void Update()
        {
            world.Update();
            player.Update();
        }
        public void Initialize()
        {
            player = new Player(xnaInterface);
            world = new World(xnaInterface);
            world.Generate();
        }
    }
}
