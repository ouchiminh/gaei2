using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PriorityQueue;
using System.Threading;

namespace gaei.navi {
    using ReadOnlyEnvMap = IReadOnlyDictionary<Area, Sensor.ScanResult>;

    public class Pilot : GlobalPathProposer
    {
        /// <summary>
        /// hereからdestまでの経路をDijkstra法で検索します。
        /// 経路の頂点はenvmapに含まれる頂点から検索されます。destがenvmapに含まれていない場合、
        /// </summary>
        /// <param name="dest">経路探索の終点</param>
        /// <param name="here">経路探索の始点</param>
        /// <param name="envmap">環境マップ</param>
        /// <returns>経路</returns>
        /// <exception cref="System.InvalidOperationException">envmapが空の場合</exception>
        /// <exception cref="System.NullReferenceException">envmapにdestを含む小空間が含まれていない場合</exception>
        public IEnumerable<Area> getPath(Vector3 dest, Vector3 here, in ReadOnlyEnvMap envmap)
        {
            var candidate = (from x in envmap where x.Value == Sensor.ScanResult.nothingFound select x.Key).ToList();
            Area g2 = candidate.First();
            Area s = candidate.First();
            Area goal = new Area(dest);
            Area start = new Area(here);
            foreach (var x in candidate)
            {
                if (Area.distance(x, goal) < Area.distance(goal, g2)) g2 = x;
                if (Area.distance(x, start) < Area.distance(start, s)) s = x;
            }
            if (candidate.Count == 0) throw new System.InvalidOperationException("no path");

            // TODO:パスの任意の2頂点間にrayを飛ばして、そのrayが何にもぶつからなければ間の頂点を消す。
            var path = areaDijkstra(s, envmap, g2, candidate);
            if (path.Count <= 2) return path;
            var first = path.First;
            var last = path.Last;

            //while (first.Next != path.Last)
            //{
            //    var d = last.Value.center - first.Value.center;
            //    if (!Physics.BoxCast(first.Value.center, new Vector3(.5f, .5f, .5f), d, default, d.magnitude))
            //    {
            //        for (var r = first.Next; first.Next != last; r = first.Next)
            //            path.Remove(r);
            //        if (path.Count <= 2) return path;
            //        first = last;
            //        last = path.Last;
            //        continue;
            //    }
            //    last = last.Previous;
            //}
            Debug.Log("done");

            return path;
        }

        private LinkedList<Area> areaDijkstra(Area here, ReadOnlyEnvMap envmap, Area f, IEnumerable<Area> candidates)
        {
            Dictionary<Area, (float distance, Area? prev)> g = new Dictionary<Area, (float distance, Area? prev)>();
            PriorityQueue<(Area a, float d)> q = new PriorityQueue<(Area a, float d)>((x, y)=>x.d.CompareTo(y.d));
            var s = here;
            foreach (var a in candidates)
            {
                g.Add(a, (float.MaxValue, null));
                q.Enqueue((a, float.MaxValue));
            }
            q.Enqueue((s, 0));
            g[s] = (0, null);
            while(q.Count > 0)
            {
                (var a, var d) = q.Dequeue();
                if (d > g[a].distance) continue;
                foreach(var node in from x in connectedAreas(a) where envmap.ContainsKey(x) && envmap[x] == Sensor.ScanResult.nothingFound select x)
                {
                    if(g.ContainsKey(node) && g[node].distance > d + 1)
                    {
                        g[node] = (d + 1, a);
                        q.Enqueue((node, d + 1));
                    }
                    if (node.CompareTo(f) == 0) break;
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

