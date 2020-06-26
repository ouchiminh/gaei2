﻿using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace gaei.navi
{
    using ReadOnlyEnvMap = IReadOnlyDictionary<Area, Sensor.ScanResult>;
    public class CAM : LocalPathProposer
    {
        const float C = 3;
        /// <summary>
        /// hereからdestまでの経路をポテンシャル法で検索します。
        /// </summary>
        /// <param name="dest">経路探索の終点</param>
        /// <param name="here">経路探索の始点</param>
        /// <param name="envmap">環境マップ</param>
        /// <returns>動くべき方向</returns>
        public Vector3 getCourse(Vector3? dest, Vector3 here, in ReadOnlyEnvMap envmap)
        {
            // TODO:移動障害物と処理を分離して軽量化
            Vector3 current = default(Vector3);
            Area herearea = new Area(here);
            for(var x = 1; x < radius; ++x)
                for (var y = 1; y < radius; ++y)
                    for (var z = 1; z < radius; ++z)
                    {
                        Vector3 dummy = default;
                        Vector3Int relative = new Vector3Int(x, y, z);
                        {
                            var larea = new Area(herearea.representativePoint + relative);
                            if (Sensor.looka(larea, ref dummy) == Sensor.ScanResult.nothingFound) goto minus;
                            var r = larea.center - here;
                            current -= r.normalized / Vector3.SqrMagnitude(r);
                        }
                        minus:
                        {
                            var larea = new Area(herearea.representativePoint - relative);
                            if (Sensor.looka(larea, ref dummy) == Sensor.ScanResult.nothingFound) continue;
                            var r = larea.center - here;
                            current -= r.normalized / Vector3.SqrMagnitude(r);
                        }
                    }
            if (dest == null) return current;
            var goal = dest.Value - here;
            current += C/2*goal.normalized;
            return current;
        }
        public const int radius = 5;
    }
}

