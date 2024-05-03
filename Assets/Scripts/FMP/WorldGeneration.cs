using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Mathematics;
using System;
using UnityEngine.UIElements;

namespace FMP
{
    using XY = ValueTuple<byte, byte>;

    [System.Serializable]
    public enum TileGroup
    {
        Background,
        Foreground,
    }

    public enum TileType
    {
        Air,
        Dirt,
        Grass,
        Stone,
        DarkStone
    }

    [System.Serializable]
    public class Block
    {
        
        public TileType tileType;
    }
    
    [System.Serializable]
    public class Wall
    {
        TileType tileType;
    }

    public interface IItem
    {
        void PrimaryUse();
        void SecondaryUse();
    }

    [System.Serializable]
    public class ItemStack
    {
        public void Add(int change)
        {
            amount += change;
            if (amount <= 0)
            {
                amount = 0;
                item = null;
                itemId = (int)ItemID.None;
            }
        }
        public ItemBase item;
        public int itemId;
        public int amount;
    }

    public class Chest : Block
    {
        public const int Width = 10;
        public const int Height = 4;

    }



    public class WorldGeneration : MonoBehaviour
    {
        public CameraFollow cameraFollow;
        public string loadFile;

        void Start()
        {
            WorldGen(100);
        }

        readonly float factor1 = 50;
        readonly float factor2 = 7;

        void doCircle(bool[,] caveMap, Vector2Int pos, int radius)
        {
            // Angle between pixels
            // float minAngle = Mathf.Acos(1f - 1f / radius);
            
            // for (float angle = 0; angle < 360; angle += minAngle)
            // {
            //     int circumX = pos.x + Mathf.RoundToInt(Mathf.Cos(angle) * radius);
            //     int circumY = pos.y + Mathf.RoundToInt(Mathf.Sin(angle) * radius);

            //     for (int x = pos.x; Mathf.Sign(x - circumX) == Mathf.Sign(pos.x - circumX); x -= Mathf.RoundToInt(Mathf.Sign(pos.x - circumX)))
            //     {
            //         if (0 <= x && x <= caveMap.GetUpperBound(0) && 0 <= circumY && circumY <= caveMap.GetUpperBound(1))
            //             caveMap[x, circumY] = false;
            //     }
            // }

            
            // x ^ 2 + y ^ 2 = r ^ 2
            var r_sq = radius * radius;
            for (int x = 0; x < radius; x++)
            {
                // so then, y = sqrt(r ^ 2 - x ^ 2)
                var x_sq = x * x;
                var max_y = Mathf.RoundToInt(Mathf.Sqrt(r_sq - x_sq));

                System.Action<Vector2Int> doSingleTile = (Vector2Int offset) => {
                    var offsetPos = pos + offset;
                    if (0 < offsetPos.x && offsetPos.x < caveMap.GetUpperBound(0) && 0 < offsetPos.y && offsetPos.y < caveMap.GetUpperBound(1))
                    {
                        caveMap[offsetPos.x, offsetPos.y] = false;
                    }
                };

                for (int y = 0; y <= max_y; ++y)
                {
                    doSingleTile(new Vector2Int(x, y));
                    doSingleTile(new Vector2Int(-x, y));
                    doSingleTile(new Vector2Int(x, -y));
                    doSingleTile(new Vector2Int(-x, -y));
                }
            }
        }
        void doCircleDirect(Vector2Int pos, int radius, TileType tileType)
        {
            // Angle between pixels
            // float minAngle = Mathf.Acos(1f - 1f / radius);
            
            // for (float angle = 0; angle < 360; angle += minAngle)
            // {
            //     int circumX = pos.x + Mathf.RoundToInt(Mathf.Cos(angle) * radius);
            //     int circumY = pos.y + Mathf.RoundToInt(Mathf.Sin(angle) * radius);

            //     for (int x = pos.x; Mathf.Sign(x - circumX) == Mathf.Sign(pos.x - circumX); x -= Mathf.RoundToInt(Mathf.Sign(pos.x - circumX)))
            //     {
            //         if (0 <= x && x <= caveMap.GetUpperBound(0) && 0 <= circumY && circumY <= caveMap.GetUpperBound(1))
            //             caveMap[x, circumY] = false;
            //     }
            // }
            System.Action<Vector2Int> doSingleTile = (Vector2Int offset) => {
                var offsetPos = pos + offset;
                if (0 <= offsetPos.x && 0 <= offsetPos.y)
                {
                    WorldManager.instance.SetBlock(offsetPos, new Block { tileType = tileType});
                }
            };

            
            // x ^ 2 + y ^ 2 = r ^ 2
            var r_sq = radius * radius;
            for (int x = 0; x <= radius; x++)
            {
                // so then, y = sqrt(r ^ 2 - x ^ 2)
                var x_sq = x * x;
                var max_y = Mathf.RoundToInt(Mathf.Sqrt(r_sq - x_sq));

                print("For r = " + radius + ", x = " + x + ": y = " + max_y);

                var y = max_y;
                //for (int y = 0; y <= max_y; ++y)
                //{
                    doSingleTile(new Vector2Int(x, y));
                    doSingleTile(new Vector2Int(-x, y));
                    doSingleTile(new Vector2Int(x, -y));
                    doSingleTile(new Vector2Int(-x, -y));
                    
                    doSingleTile(new Vector2Int(y, x));
                    doSingleTile(new Vector2Int(y, -x));
                    doSingleTile(new Vector2Int(-y, x));
                    doSingleTile(new Vector2Int(-y, -x));
                //}
            }
        }

        void doCave(bool[,] caveMap, Vector2Int pos, int length, int radius)
        {
            Vector2 lastOffset = Vector2.right * Util.GetSimplexNoise(pos.y / 5f / radius, pos.x / 5f / radius) * 5f * radius;
            for (int i = 0; i < length; ++i)
            {
                float theXCoordinate = ((Util.GetSimplexNoise((pos.y - i) / 5f / radius, pos.x / 5f / radius)) * 5f * radius);

                Vector2 nextOffset = new(theXCoordinate, -i);
                var posDiff = (nextOffset - lastOffset).magnitude;
                if (posDiff != 0)
                {
                    for (float l = 0; l < 1; l = Mathf.Min(1, l + 1f / posDiff))
                    {
                        doCircle(caveMap, pos + Vector2Int.RoundToInt(Vector2.Lerp(lastOffset, nextOffset, l)), radius);
                    }
                }
                else
                {
                    doCircle(caveMap, pos + new Vector2Int(Mathf.RoundToInt(theXCoordinate), -i), radius);
                }
                lastOffset = nextOffset;
            }
        }

        void doCaveRandom(bool[,] caveMap, Vector2Int pos)
        {
            int length = UnityEngine.Random.Range(200, 300);
            int radius = UnityEngine.Random.Range(4, 10);
            doCave(caveMap, pos, length, radius);
        }

        void WorldGen(long seed)
        {
            var groundHeight = 250;
            var rngOldState = UnityEngine.Random.state;
            UnityEngine.Random.InitState((int)seed);
            Vector2Int vecSeed = new((int)(seed), (int)(seed >> 32));
            Vector2Int halfWorldSize = new(100, 200);
            // The worldSize goes in both directions
            var worldSize = halfWorldSize * 2;

            bool[,] squiggleCaveMap = new bool[worldSize.x, worldSize.y];
            bool[,] openCaveMap = new bool[worldSize.x, worldSize.y];
            int[] heightMap = new int[worldSize.x];

            for (int i = 0; i < worldSize.x; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    float x = i + j / 10f;
                    Debug.DrawRay(new Vector3(x, Util.GetSimplexNoise(vecSeed.x + (x / factor1), vecSeed.y, 3) * factor2), Vector3.up * 0.1f, Color.red);
                }
                float noiseHeight = Util.GetSimplexNoise(vecSeed.x + (i / factor1), vecSeed.y, 2) * factor2;
                int height = Mathf.RoundToInt(noiseHeight) + worldSize.y - Mathf.CeilToInt(factor2) - 100;
                heightMap[i] = height;
            }

            for (int i = 0; i < worldSize.x; ++i)
            {
                for (int j = 0; j < worldSize.y; ++j)
                {
                    // bool b = (Mathf.PerlinNoise(i / 30.0f, j / 30.0f) + Mathf.PerlinNoise(i / 20.0f, j / 20.0f) * 0.6f) / 1.6f > 0.35f;
                    bool b = j > groundHeight || Util.GetSimplexNoise(i / 128f, j / 128f, 5) < 0.2f;
                    openCaveMap[i, j] = b;
                    squiggleCaveMap[i, j] = true;
                }
            }

            openCaveMap = SmoothMooreCellularAutomata(openCaveMap, true, 1);

            for (int i = 0; i < worldSize.x; ++i)
            {
                for (int j = 0; j < worldSize.y; ++j)
                {
                    bool shouldntCave = UnityEngine.Random.Range(0f, 1f) > 0.00001f;
                    if (!shouldntCave)
                        doCaveRandom(squiggleCaveMap, new Vector2Int(i, j));
                }
            }

            //        doCircle(squiggleCaveMap, worldSize / 2, 50);

            doCave(squiggleCaveMap, new Vector2Int(halfWorldSize.x, heightMap[halfWorldSize.x]), heightMap[halfWorldSize.x] + 2, 5);

            for (int i = 0; i < worldSize.x; ++i)
            {
                for (int y = heightMap[i]; y < worldSize.y; ++y)
                {
                    squiggleCaveMap[i, y] = false;
                }
            }

            for (int i = 0; i < worldSize.x; ++i)
            {
                bool first = true;
                for (int j = worldSize.y - 1; j >= 0; --j)
                {
                    if (squiggleCaveMap[i, j] && openCaveMap[i, j])
                    {
                        if (first)
                        {
                            // TODO: Place Grass
                            WorldManager.instance.SetBlock(new Vector2Int(i, j), new Block { tileType = TileType.Grass });

                            first = false;
                        }
                        else if (j > groundHeight)
                        {
                            //print("placing dirt");
                            // TODO: Place dirt
                            WorldManager.instance.SetBlock(new Vector2Int(i, j), new Block { tileType = TileType.Dirt });
                        }
                        else
                        {
                            // TODO: Place stone
                            WorldManager.instance.SetBlock(new Vector2Int(i, j), new Block { tileType = TileType.Stone });
                        }
                    }
                }
            }

            doCircleDirect(new Vector2Int(10, 10), 6, TileType.DarkStone);
            doCircleDirect(new Vector2Int(10, 10), 5, TileType.DarkStone);

            WorldManager.instance.worldInfo.dimensions = worldSize;
            WorldManager.instance.worldInfo.spawnPoint = new Vector2Int(halfWorldSize.x, heightMap[halfWorldSize.x]);

            cameraFollow.maxCoords = worldSize * 16;

            UnityEngine.Random.state = rngOldState;
        }
        void Update()
        {
        }

        // The following two methods were adapted by me from the algorithm discussed at https://blog.unity.com/technology/procedural-patterns-to-use-with-tilemaps-part-ii
        static int GetMooreSurroundingTiles(bool[,] map, int x, int y, bool edgesAreWalls)
        {
            int tileCount = 0;

            for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
            {
                for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < map.GetUpperBound(0) && neighbourY >= 0 && neighbourY < map.GetUpperBound(1))
                    {
                        //We don't want to count the tile we are checking the surroundings of
                        if (neighbourX != x || neighbourY != y)
                        {
                            tileCount += map[neighbourX, neighbourY] ? 1 : 0;
                        }
                    }
                }
            }
            return tileCount;
        }

        public static bool[,] SmoothMooreCellularAutomata(bool[,] mapA, bool edgesAreWalls, int smoothCount)
        {
            // This is used so that the conditions aren't affected by previous modifications to the map that happened in the same time step
            // If I used a single map then changes to that map would affect the subsequent conditions' evaluation.
            bool[,] mapB = new bool[mapA.GetLength(0), mapA.GetLength(1)];

            for (int i = 0; i < smoothCount; i++)
            {
                for (int x = 0; x <= mapA.GetUpperBound(0); x++)
                {
                    for (int y = 0; y <= mapA.GetUpperBound(1); y++)
                    {
                        int surroundingTiles = GetMooreSurroundingTiles(mapA, x, y, edgesAreWalls);

                        if (edgesAreWalls && (x == 0 || x == mapA.GetUpperBound(0) || y == 0 || y == mapA.GetUpperBound(1)))
                        {
                            //Set the edge to be a wall if we have edgesAreWalls to be true
                            mapB[x, y] = true;
                        }
                        else if (surroundingTiles > 4)
                        {
                            mapB[x, y] = true;
                        }
                        else if (surroundingTiles < 3)
                        {
                            mapB[x, y] = false;
                        }
                        else
                        {
                            mapB[x, y] = mapA[x, y];
                        }
                    }
                }
                (mapA, mapB) = (mapB, mapA);
            }
            //Return the modified map
            return mapA;
        }
    }
}