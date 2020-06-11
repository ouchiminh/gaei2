using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PriorityQueue;

namespace gaei.navi {
    using EnvMap = System.Collections.Generic.Dictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)>;

    public class Pilot : GlobalPathProposer
    {
        /// <summary>
        /// hereからdestまでの経路をDijkstra法で検索します。
        /// 経路の頂点はenvmapに含まれる頂点から検索されます。したがって、destがenvmapに含まれていない場合何が起こるか分かりません。
        /// </summary>
        /// <param name="dest">経路探索の終点</param>
        /// <param name="here">経路探索の始点</param>
        /// <param name="envmap">環境マップ</param>
        /// <returns>経路</returns>
        /// <exception cref="System.InvalidOperationException">envmapが空の場合</exception>
        public IEnumerable<Area> getPath(Vector3 dest, Vector3 here, in EnvMap envmap)
        {
            var candidate = (from x in envmap select x.Key).ToList();
            double sqrd = double.MaxValue;
            Area g2 = default(Area);
            if (candidate.Count == 0) throw new System.InvalidOperationException("no goal");
            foreach(var x in candidate)
            {
                var buf = (x.center - dest).sqrMagnitude;
                if (buf < sqrd)
                {
                    sqrd = buf;
                    g2 = x;
                }
            }
            // TODO:パスの任意の2頂点間にrayを飛ばして、そのrayが何にもぶつからなければ間の頂点を消す。
            return areaDijkstra(here, envmap, g2);

        }

        private LinkedList<Area> areaDijkstra(Vector3 here, in EnvMap envmap, Area f)
        {
            Dictionary<Area, (float distance, Area? prev)> g = new Dictionary<Area, (float distance, Area? prev)>();
            PriorityQueue<(Area a, float d)> q = new PriorityQueue<(Area a, float d)>((x, y)=>x.d.CompareTo(y.d));
            var s = new Area(here);
            foreach (var a in from x in envmap where x.Value.accessibility == Sensor.ScanResult.nothingFound select x)
            {
                g.Add(a.Key, (float.MaxValue, null));
                q.Enqueue((a.Key, float.MaxValue));
            }
            q.Enqueue((s, 0));
            g[s] = (0, null);
            while(q.Count > 0)
            {
                (var a, var d) = q.Dequeue();
                if (d > g[a].distance) continue;
                foreach(var node in connectedAreas(a))
                {
                    if(g.ContainsKey(node) && g[node].distance > d + 1)
                    {
                        g[node] = (d + 1, a);
                        q.Enqueue((a, d + 1));
                    }
                }
            }
            // ゴールから辿る
            var res = new LinkedList<Area>();
            res.AddFirst(f);
            while (res.First.Value.CompareTo(s) != 0)
            {
                res.AddFirst(g[res.First.Value].prev.Value);
            }
            return res;
        }

        static System.Collections.Generic.IEnumerable<Area> connectedAreas(Area a)
        {
            for (int i = 0; i < 6; ++i)
            {
                var dim = i / 2;
                var diff = 1 == (i & 1) ? 1 : -1;
                var pos = a.representativePoint;
                pos[dim] += diff;
                yield return new Area(pos);
            }
            yield break;
        }

    }
}

