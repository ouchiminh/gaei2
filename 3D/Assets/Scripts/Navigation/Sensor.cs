
using System;
using System.Collections.Generic;
using UnityEngine;
using gaei.navi;
using System.Threading;
using System.Threading.Tasks;

namespace gaei.navi
{
    using EnvMap = SortedDictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)>;
    using ReadOnlyEnvMap = IReadOnlyDictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)>;
    public static class Sensor
    {
        public enum ScanResult
        {
            somethingFound, nothingFound, unobservable
        }
        static Sensor()
        { envmap_ = new EnvMap(); }

        private static Collider[] buffer_ = new Collider[1];
        public static void scan(Vector3Int? min = null, Vector3Int? size = null)
        {
            //for (int x = scanOffset.x; x <= scanOffset.x + scanSize.x; ++x)
            //    for (int y = scanOffset.y; y <= scanOffset.y + scanSize.y; ++y)
            //        for (int z = scanOffset.z; z <= scanOffset.z + scanSize.z; ++z)
            //        {
            //            var area = new Area(new Vector3(x, y, z));
            //            Vector3 velocity = default;
            //            var res = looka(area, ref velocity);
            //            envmap_.Add(area, (res, velocity));
            //        }
            scan_impl(min??scanOffset, size??scanSize);
        }
        private static async void scan_impl(Vector3Int min, Vector3Int size)
        {
            // 全ての軸の方向の大きさが1以下ならば終了
            if (size.x == 1 && size.y == 1 && size.z == 1)
            {
                var area = new Area(min);
                bool result = Physics.OverlapBoxNonAlloc(area.center, new Vector3(.5f, .5f, .5f), buffer_) > 0;
                if (envmap_.ContainsKey(area))
                    envmap_[area] = (result ? ScanResult.somethingFound : ScanResult.nothingFound, null);
                else
                    envmap_.Add(area, (result ? ScanResult.somethingFound : ScanResult.nothingFound, null));
                return;
            }
            bool res = Physics.OverlapBoxNonAlloc(
                new Vector3((min.x + size.x) / 2.0f, (min.y + size.y) / 2.0f, (min.z + size.z) / 2.0f),
                new Vector3(size.x/2.0f, size.y/2.0f, size.z/2.0f),
                buffer_
                ) > 0;
            if (!res)
            {
                for (int x = min.x; x <= min.x + size.x; ++x)
                    for (int y = min.y; y <= min.y + size.y; ++y)
                        for (int z = min.z; z <= min.z + size.z; ++z)
                        {
                            var area = new Area(new Vector3(x, y, z));
                            if (envmap_.ContainsKey(area))
                                envmap_[area] = (ScanResult.nothingFound, null);
                            else
                                envmap_.Add(area, (ScanResult.nothingFound, null));
                        }
                return;
            }
            bool spx = size.x > 1;
            bool spy = size.y > 1;
            bool spz = size.z > 1;

            const int xmask = 1 << 2;
            const int ymask = 1 << 1;
            const int zmask = 1;
            for (int i = 0; i < 8; ++i)
            {
                // XYZ
                // 001
                if(((i & xmask) > 0 && spx) && ((i & ymask) > 0 && spy) && ((i & zmask) > 0 && spz)) continue;
                var next = new Vector3Int(
                    (i & xmask) > 0 ? (min.x + size.x) / 2 : min.x,
                    (i & ymask) > 0 ? (min.y + size.y) / 2 : min.y,
                    (i & zmask) > 0 ? (min.z + size.z) / 2 : min.z
                    );
                var nextsize = new Vector3Int(
                    (i & xmask) > 0 ? (1 + size.x) / 2 : size.x / 2,
                    (i & ymask) > 0 ? (1 + size.y) / 2 : size.y / 2,
                    (i & zmask) > 0 ? (1 + size.z) / 2 : size.z / 2
                    );
                scan_impl(next, nextsize);
            }
        }

        // Sensorがスキャンできる半径
        public static Vector3Int scanSize;
        public static Vector3Int scanOffset;
        private static EnvMap envmap_;

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
            bool accessible = Physics.OverlapBoxNonAlloc(area.center, new Vector3(.5f, .5f, .5f), buffer_) > 0;
            return accessible ? ScanResult.somethingFound : ScanResult.nothingFound;
        }

        // Sensorが構築する環境マップ
        public static ReadOnlyEnvMap envmap { get => envmap_; }
    }
}

