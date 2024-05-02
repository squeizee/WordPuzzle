using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [Serializable]
    public struct LevelData
    {
        public string title;
        public List<Tile> tiles;
    }

    [Serializable]
    public struct Tile
    {
        public int id;
        public Vector3 position;
        public string character;
        public List<int> children;
    }
}