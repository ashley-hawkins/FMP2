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

    [System.Serializable]
    public struct BlockInfo
    {
        public TileBase tile;
        public Sprite icon;
    }

    public class WorldManager : MonoBehaviour
    {
        public GameObject droppedItemPrefab;
        public BlockInfo[] blocks;
        public static WorldManager instance;
        public WorldInformation worldInfo = new();
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

            foreground.SetTile(((Vector3Int)coords), blocks[(int)block.tileType].tile);
        }

        public void BreakBlock(Vector2Int coords)
        {
            SetBlock(coords, new Block { tileType = TileType.Air });
            var droppedItem = Instantiate(droppedItemPrefab).GetComponent<DroppedItem>();
            droppedItem.item = new BlockItem(GetBlock(coords).tileType);
        }
        public Block GetBlock(Vector2Int coords)
        {
            (XY chunk, XY offsets) = GetChunkedCoords(coords.x, coords.y);
            if (!foregroundInfo.ContainsKey(chunk)) return null;

            return foregroundInfo[chunk][coords.x, coords.y];
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
        public string name;
        public Vector2Int dimensions;
        public Vector2Int spawnPoint;
    }

}
