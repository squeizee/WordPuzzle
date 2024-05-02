using Holder;
using Tiles;

namespace Commands
{
    public class UndoCommand
    {
        public void Execute(TileManager tileManager, HolderManager holderManager)
        {
            holderManager.Undo(out TileController tileController);
            tileManager.AddTile(tileController);
            tileManager.ReCalculateTiles(tileController);
        }
    }
}