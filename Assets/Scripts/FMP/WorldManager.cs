using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FMP
{
    using XY = ValueTuple<byte, byte>;

    public enum ItemID
    {
        None,
        DirtBlock,
        GrassBlock,
        StoneBlock,
        StonePickaxe,
        GoldPickaxe,
        DarkStoneBlock,
        IronOreBlock,
        RefinedIron,
        BasicSword,
        FirstItem = DirtBlock,
        LastItem = BasicSword
    }

    [System.Serializable]
    public struct BlockInfo
    {
        public TileBase tile;
        public Sprite icon;
        public ItemID dropId;
        public int miningLevel;
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
        public LayerMask ignoreTerrainMask;

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

        public bool SetBlock(Vector2Int coords, Block block, bool ignoreExisting = true)
        {
            (XY chunk, XY offsets) = GetChunkedCoords(coords.x, coords.y);
            if (!foregroundInfo.ContainsKey(chunk))
            {
                foregroundInfo.Add(chunk, new Block[ChunkSize, ChunkSize]);
            }
            if (!ignoreExisting)
            {
                var existingBlock = foregroundInfo[chunk][offsets.Item1, offsets.Item2];
                if
                    (
                        (existingBlock != null && existingBlock.tileType != TileType.Air) ||
                        Physics2D.OverlapBox(coords * 16 + new Vector2Int(8, 8), new Vector2Int(16, 16), 0, ignoreTerrainMask) != null
                    )
                    return false;

            } 
            foregroundInfo[chunk][offsets.Item1, offsets.Item2] = block;
            foreground.SetTile(((Vector3Int)coords), blocks[(int)block.tileType].tile);

            return true;
        }

        public void BreakBlock(Vector2Int coords, int miningLevel)
        {
            var block = GetBlock(coords);

            if (block == null) return;

            if (blocks[(int)block.tileType].miningLevel > miningLevel)
                return; // Pickaxe too weak
            if (block == null || block.tileType == TileType.Air)
                return; // already "broken"

            SetBlock(coords, new Block { tileType = TileType.Air });
            var droppedItem = Instantiate(droppedItemPrefab, ((Vector3Int)(coords * 16)) + new Vector3Int(8, 8, 0), Quaternion.identity).GetComponent<DroppedItem>();
            droppedItem.RandomizeHorizontalSpeed();

            print("Breaking block: " + block.tileType.ToString());
            droppedItem.itemStack = new ItemStack
            {
                itemId = blocks[(int)block.tileType].dropId,
                amount = 1
            };
        }
        public Block GetBlock(Vector2Int coords)
        {
            (XY chunk, XY offsets) = GetChunkedCoords(coords.x, coords.y);
            if (!foregroundInfo.ContainsKey(chunk)) return null;

            var block = foregroundInfo[chunk][offsets.Item1, offsets.Item2];
            return block;
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

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        private void Start()
        {
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
