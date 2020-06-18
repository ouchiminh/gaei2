using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace gaei.navi
{
    // 世界を分割する小空間(立方体)
    public struct Area : System.IComparable<Area>
    {
        public Area(Vector3Int pos) { representativePoint = pos; }
        public Area(Vector3 pos)
        {
            representativePoint = default(Vector3Int);
            representativePoint.x = (int)System.Math.Floor(pos.x);
            representativePoint.y = (int)System.Math.Floor(pos.y);
            representativePoint.z = (int)System.Math.Floor(pos.z);
        }
        public Area(int x, int y, int z)
        {
            representativePoint = new Vector3Int(x, y, z);
        }
        // 小空間の一辺の長さ
        public const int size = 1;
        // 小空間の代表点(小空間の中で最も座標が小さい点
        public Vector3Int representativePoint;
        public Vector3 center { get => representativePoint + new Vector3((float)size / 2, (float)size / 2, (float)size / 2); }

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
        public int CompareTo(Area other)
        {
            return
                representativePoint.x.CompareTo(other.representativePoint.x) != 0 ? representativePoint.x.CompareTo(other.representativePoint.x) :
                representativePoint.y.CompareTo(other.representativePoint.y) != 0 ? representativePoint.y.CompareTo(other.representativePoint.y) :
                representativePoint.z.CompareTo(other.representativePoint.z) != 0 ? representativePoint.z.CompareTo(other.representativePoint.z) : 0;
        }
    }
}
