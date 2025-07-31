using System;

namespace MDRPG
{
    public static class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            MDRPG mdrpg = new MDRPG();
            mdrpg.Run();
            return 0;
        }
    }
}