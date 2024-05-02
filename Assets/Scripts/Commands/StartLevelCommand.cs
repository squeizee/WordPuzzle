using Holder;
using Tiles;

namespace Commands
{
    public class StartLevelCommand
    {
        public void Execute(int level, TileManager tileManager, HolderManager holderManager)
        {
            tileManager.LoadLevel(level);
            holderManager.Initialize();
        }
    }
}