using Holder;
using Tiles;

namespace Commands
{
    public class AddTileToHolderCommand
    {
        public void Execute(TileController tile,TileManager tileManager, HolderManager holderManager)
        {
            tileManager.RemoveTile(tile);
            tileManager.ReCalculateTiles(tile);
            holderManager.AddTile(tile);
        }
    }
}