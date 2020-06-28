using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace gaei.navi
{
    using ReadOnlyEnvMap = IReadOnlyDictionary<Area, Sensor.ScanResult>;
    public class CAM : LocalPathProposer
    {
        const float C = div*div/2;
        /// <summary>
        /// hereからdestまでの経路をポテンシャル法で検索します。
        /// </summary>
        /// <param name="dest">経路探索の終点</param>
        /// <param name="here">経路探索の始点</param>
        /// <param name="envmap">環境マップ</param>
        /// <returns>動くべき方向</returns>
        public Vector3 getCourse(Vector3? dest, Vector3 here, in ReadOnlyEnvMap envmap)
        {
            const float _2pi = 2*(float)System.Math.PI;
            // TODO:移動障害物と処理を分離して軽量化
            Vector3 current = default(Vector3);
            Area herearea = new Area(here);
            for (var x = 0; x < div; ++x)
                for (var y = 0; y < div; ++y)
                {
                    var d = new Vector3((float)System.Math.Sin(x * _2pi / div), (float)System.Math.Cos(y * _2pi / div), (float)System.Math.Cos(x * _2pi / div)).normalized;
                    var res = Sensor.lookd(d * radius, here);
                    if (res == null) continue;
                    current -= d / res.Value;
                }
            if (dest == null) return current;
            var goal = C*(dest.Value - here).normalized;
            current += goal;
            return current;
        }
        public const int radius = 3;
        private const int div = 12;
    }
}

