
using System;
using UnityEngine;
using gaei.navi;
using System.Threading;

namespace gaei.navi
{
    public static class Sensor
    {
        public enum ScanResult
        {
            somethingFound, nothingFound, unobservable
        }

        static Sensor()
        { envmap_ = new System.Collections.Generic.Dictionary<Area, (ScanResult accessibility, Vector3? velocity)>(); }

        private static Collider[] buffer_ = new Collider[3];
        public static void scan()
        {
            for (int x = -scanRadius.x; x <= scanRadius.x; ++x)
                for (int y = -scanRadius.y; y <= scanRadius.y; ++y)
                    for (int z = -scanRadius.z; z <= scanRadius.z; ++z)
                    {
                        var area = new Area(referencePoint + new Vector3(x, y, z));
                        int cnt = Physics.OverlapBoxNonAlloc(area.center, new Vector3(.5f, .5f, .5f), buffer_);
                        Vector3 velocity = default(Vector3);
                        var res = looka(area, ref velocity);
                        envmap_.Add(area, (res, velocity));
                    }
        }

        // Sensorがスキャンできる半径
        public readonly static Vector3Int scanRadius;
        private static System.Collections.Generic.Dictionary<Area, (ScanResult accessibility, Vector3? velocity)> envmap_;

        /// <summary>
        /// 指定された方向をスキャンし、障害物の有無を確かめる。
        /// </summary>
        /// <param name="direction">方向</param>
        /// <param name="position">障害物があればその位置。なければ値は保証されない。</param>
        /// <param name="velocity">障害物があればその移動ベクトル。なければ値は保証されない。</param>
        /// <remarks>スキャンする最大の距離はdirectionの長さです。</remarks>
        public static ScanResult lookd(Vector3 direction, ref Area position, ref Vector3 velocity) { throw new NotImplementedException(); }

        // TODO:障害物速度ベクトルへの対応
        /// <summary>
        /// 指定された小空間をスキャンし、障害物の有無を確かめる。
        /// </summary>
        /// <param name="area">見たい小空間</param>
        /// <param name="velocity">障害物が存在していればその移動ベクトル。なければ値は保証されない。</param>
        public static ScanResult looka(Area area, ref Vector3 velocity)
        {
            int cnt = Physics.OverlapBoxNonAlloc(area.center, new Vector3(.5f, .5f, .5f), buffer_);
            return cnt > 0 ? ScanResult.somethingFound : ScanResult.nothingFound;
        }

        // Sensorと紐づくオブジェクトの現在地
        public static Vector3 referencePoint { get; set; }
        // Sensorが構築する環境マップ
        public static System.Collections.ObjectModel.ReadOnlyDictionary<Area, (ScanResult accessibility, Vector3? velocity)> envmap { get; }
    }
}

