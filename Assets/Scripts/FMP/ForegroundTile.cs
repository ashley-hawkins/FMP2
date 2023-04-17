using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using System.Linq;

namespace FMP
{
    public enum ForegroundTileType
    {
        Block,
        Decoration
    }
    [CreateAssetMenu(menuName = "FMP/Foreground Tile")]
    public class ForegroundTile : RuleTile<ForegroundTile.Neighbor> {
        public ForegroundTileType type;

        public TileBase[] similarTiles;

        public class Neighbor : RuleTile.TilingRule.Neighbor {
            public const int Similar = 3;
            public const int NotSimilar = 4;
            public const int Block = 5;
            public const int NotBlock = 6;
            public const int BlockNotThis = 7;
        }

        public override bool RuleMatch(int neighbor, TileBase tile) {
            var tileAsFgTile = tile as ForegroundTile;
            switch (neighbor) {
                case Neighbor.Similar: return similarTiles.Contains(tile);
                case Neighbor.NotSimilar: return !similarTiles.Contains(tile);
                case Neighbor.Block: {
                    if (tileAsFgTile == null) return false;
                    return tileAsFgTile.type == ForegroundTileType.Block;
                };
                case Neighbor.NotBlock: return !RuleMatch(Neighbor.Block, tile);
                case Neighbor.BlockNotThis: return RuleMatch(Neighbor.Block, tile) && tile != this;
            }
            return base.RuleMatch(neighbor, tile);
        }
    }
}