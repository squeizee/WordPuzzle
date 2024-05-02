using Tiles;
using UnityEngine;

namespace Holder
{
    public class HolderSlot : MonoBehaviour
    {
        private TileController _tileController;
        
        public void RemoveTile()
        {
            _tileController = null;
        }
        
        public void AddTile(TileController tileController)
        {
            _tileController = tileController;
        }
        
        public bool CanAddTile()
        {
            return !_tileController;
        }
        
        public TileController GetTile()
        {
            return _tileController;
        }
        
        public bool IsFull()
        {
            return  _tileController;
        }
        
        
    }
}