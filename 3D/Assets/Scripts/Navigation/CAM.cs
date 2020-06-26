using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace gaei.navi
{
    using ReadOnlyEnvMap = IReadOnlyDictionary<Area, Sensor.ScanResult>;
    public class CAM : LocalPathProposer
    {
        const float C = 2;
        /// <summary>
        /// hereからdestまでの経路をポテンシャル法で検索します。
        /// </summary>
        /// <param name="dest">経路探索の終点</param>
        /// <param name="here">経路探索の始点</param>
        /// <param name="envmap">環境マップ</param>
        /// <returns>動くべき方向</returns>
        public Vector3 getCourse(Vector3 dest, Vector3 here, in ReadOnlyEnvMap envmap)
        {
            // TODO:移動障害物と処理を分離して軽量化
            Vector3 current = default(Vector3);
            Area herearea = new Area(here);
            foreach(var a in from x in envmap where Area.distance(x.Key, herearea) > 1 && x.Value == Sensor.ScanResult.somethingFound && Area.distance(x.Key, herearea) < radius select x)
            {
                var r = a.Key.center - here;
                current -=  r.normalized / Vector3.SqrMagnitude(r);
            }
            var goal = dest - here;
            current += C * goal.normalized;
            current += C * goal.normalized / goal.sqrMagnitude;
            return current;
        }
        public const int radius = 10;
    }
}

