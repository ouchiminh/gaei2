using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace gaei.navi
{
    using EnvMap = System.Collections.Generic.Dictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)>;
    using ReadOnlyEnvMap = System.Collections.ObjectModel.ReadOnlyDictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)>;
    public class CAM : LocalPathProposer
    {
        const float C = 100;
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
            foreach(var a in from x in envmap where x.Value.accessibility == Sensor.ScanResult.somethingFound select x)
            {
                var r = a.Key.center - here;
                current -=  C * r.normalized / Vector3.SqrMagnitude(r);
            }
            var goal = dest - here;
            var normalgoal = goal.normalized;
            current += normalgoal + C*normalgoal/goal.sqrMagnitude;
            return current;
        }
    }
}

