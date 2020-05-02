using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace gaei.navi
{
    // 世界を分割する小空間(立方体)
    public struct Area
    {
        public Area(Vector3Int pos) { representativePoint = pos; }
        // 小空間の一辺の長さ
        public const int size = 1;
        // 小空間の代表点(小空間の中で最も原点に近い点
        public readonly Vector3Int representativePoint;

        // 小空間の各頂点
        public Vector3Int[] vertexesInt { get {
                Vector3Int[] res = new Vector3Int[8];
                for (int i = 0; i < 8; ++i)
                {
                    res[i] = representativePoint + new Vector3Int((i & 1) == 1 ? size : 0, (i & 2) == 2 ? size : 0, (i & 4) == 4 ? size : 0);
                }
                return res;
            }
        }
        public Vector3[] vertexes { get {
                var res = new Vector3[8];
                var resi = vertexesInt;
                for (int i = 0; i < 8; ++i)
                {
                    res[i] = resi[i];
                }
                return res;
            }
        }
    }
}
