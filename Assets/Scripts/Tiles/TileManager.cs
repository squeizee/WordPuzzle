using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

namespace Tiles
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField] private TileController tileControllerPrefab;
        private List<TileController> _tiles = new();

        private void Initialize()
        {
            _tiles.Clear();
            _tiles = GetComponentsInChildren<TileController>().ToList();
        }

        public void AddTile(TileController tile)
        {
            if (tile != null)
            {
                _tiles.Add(tile);
                var isLocked = _tiles.Any(t => t.remainingChildren.Contains(tile.GetId()));
                tile.ResetTile(isLocked);
            }
        }

        public void RemoveTile(TileController tile)
        {
            _tiles.Remove(tile);
        }

        public void ReCalculateTiles(TileController tile)
        {
            foreach (int childId in tile.remainingChildren)
            {
                var isLocked = _tiles.Any(x => x.remainingChildren.Contains(childId));

                var childTile = _tiles.Find(x => x.GetId() == childId);

                if (childTile)
                {
                    if (isLocked)
                        childTile.LockTile();
                    else
                        childTile.UnlockTile();
                }
            }
        }
        public bool IsAllTilesPlaced()
        {
            return _tiles.Count == 0;
        }
        public void LoadLevel(int level)
        {
            PersistentDataManager.playingLevel = level;
            
            DestroyTiles();
            ResetPosition();
            GenerateTiles(level);
            Initialize();
            
            
        }
        
        public List<string> GetCharactersOnTiles()
        {
            var characters = new List<string>();
            foreach (var tile in _tiles)
            {
                characters.Add(tile.GetCharacter());
            }

            return characters;
        }

        private void ResetPosition()
        {
            transform.position = Vector3.zero;
        }
        private void DestroyTiles()
        {
            foreach (Transform tile in transform)
            {
                Destroy(tile.gameObject);
            }
        }

        private void GenerateTiles(int level)
        {
            var levelData = LevelSo.Instance.GetLevelData(level);

            foreach (Level.Tile tileData in levelData.tiles)
            {
                var tile = Instantiate(tileControllerPrefab, tileData.position, Quaternion.identity, transform);
                var isLocked = levelData.tiles.Any(t => t.children.Contains(tileData.id));
                tile.Initialize(tileData, isLocked);
            }

            var x = (levelData.tiles.Max(t => t.position.x) + levelData.tiles.Min(t => t.position.x)) / -2f;
            var y = (levelData.tiles.Max(t => t.position.y) + levelData.tiles.Min(t => t.position.y)) / -5f;

            transform.position = new Vector3(x, y, 0);
        }
    }
}