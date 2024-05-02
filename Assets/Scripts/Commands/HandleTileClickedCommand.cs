using Holder;
using Tiles;

namespace Commands
{
    public class HandleTileClickedCommand
    {
        public void Execute(TileController tile, TileManager tileManager, HolderManager holder)
        {
            if (holder.CanAddTile())
            {
                new AddTileToHolderCommand().Execute(tile, tileManager, holder);
            }
        }
    }
}