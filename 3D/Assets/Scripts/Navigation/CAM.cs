using Microsoft.Win32.SafeHandles;
using System;
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
        const float C = div * 2;
        /// <summary>
        /// hereからdestまでの経路をポテンシャル法で検索します。
        /// </summary>
        /// <param name="dest">経路探索の終点</param>
        /// <param name="here">経路探索の始点</param>
        /// <returns>動くべき方向</returns>
        public Vector3 getCourse(Vector3? dest, Vector3 here)
        {
            Vector3 current = default(Vector3);
            Area herearea = new Area(here);
            for (var x = 0; x < div; x+=2)
                for (var y = 0; y < div; ++y)
                {
                    var theta = x * (float)Math.PI / div;
                    var phi = y * 2 * (float)Math.PI / div;
                    var d = new Vector3((float)(Math.Sin(theta) * Math.Cos(phi)), (float)(Math.Sin(theta) * Math.Sin(phi)), (float)Math.Cos(theta)).normalized;
                    var res = Sensor.lookd(d * radius, here+d/2);
                    if (res == null) continue;
                    current -= (res.Value == 0 ? d*100.0f : d / res.Value);
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

