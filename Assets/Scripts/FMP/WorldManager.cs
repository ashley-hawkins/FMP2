using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FMP
{
    using XY = ValueTuple<byte, byte>;


    public class WorldManager : MonoBehaviour
    {
        public TileBase[] tiles;
        public static WorldManager instance;
        public WorldInformation WorldInfo;
        public Grid grid;
        Dictionary<XY, Block[,]> foregroundInfo;
        Dictionary<XY, Wall[,]> backgroundInfo;
        HashSet<XY> loadedChunks;
        HashSet<XY> nextLoadedChunks;
        public Tilemap foreground;
        public Tilemap background;

        public const int ChunkSize = 16;

        void LoadChunk()
        {

        }

        public Vector2Int WorldToGrid(Vector3 worldPosition)
        {
            return (Vector2Int)grid.WorldToCell(worldPosition);
        }

        public ValueTuple<XY, XY> GetChunkedCoords(int x, int y)
        {
            return (((byte)(x / ChunkSize), (byte)(y / ChunkSize)), ((byte)(x % ChunkSize), (byte)(y % ChunkSize)));
        }

        public void SetBlock(Vector2Int coords, Block block)
        {
            (XY chunk, XY offsets) = GetChunkedCoords(coords.x, coords.y);
            if (!foregroundInfo.ContainsKey(chunk))
            {
                foregroundInfo.Add(chunk, new Block[ChunkSize, ChunkSize]);
            }
            foregroundInfo[chunk][offsets.Item1, offsets.Item2] = block;
            // update tilemap here

            foreground.SetTile(((Vector3Int)coords), tiles[(int)block.tileType]);
        }
        private void GetBlock(uint x, uint y)
        {
        }

        public void RequestLoadChunk(XY chunkCoords)
        {
            nextLoadedChunks.Add(chunkCoords);
        }

        public void LateUpdate()
        {
            LateUpdateChunks();
        }

        private void LateUpdateChunks()
        {
            //var chunksToUnload = loadedChunks;
            //var chunksToLoad = nextLoadedChunks;

            //chunksToUnload.ExceptWith(nextLoadedChunks);
            //chunksToLoad.ExceptWith(loadedChunks);
        }

        private void Start()
        {
            print("setting instance next");
            if (instance != null)
            {
                print("not setting instance");
                Destroy(gameObject);
                return;
            }
            print("setting instance");
            instance = this;
            foregroundInfo = new();
        }
    }

    public struct WorldInformation
    {
        XY dimensions;
        string name;

    }

}
