
using System;
using System.Collections.Generic;
using UnityEngine;
using gaei.navi;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace gaei.navi
{
    using EnvMap = Dictionary<Area, Sensor.ScanResult>;
    using ReadOnlyEnvMap = IReadOnlyDictionary<Area, Sensor.ScanResult>;
    public static class Sensor
    {
        public enum ScanResult
        {
            somethingFound, nothingFound, unobservable
        }
        static Sensor() { envmap_ = null; }

        private static Collider[] buffer_ = new Collider[1];
        public static void scan(Vector3Int? min = null, Vector3Int? size = null)
        {
            if (envmap_ == null) envmap_ = new EnvMap(scanSize.x * scanSize.y * scanSize.z * 2);
            var cmin = min ?? scanOffset;
            var csize = size ?? scanSize;
            for (int x = cmin.x; x < cmin.x + csize.x; ++x)
                for (int y = cmin.y; y < cmin.y + csize.y; ++y)
                    for (int z = cmin.z; z < cmin.z + csize.z; ++z)
                    {
                        var area = new Area(new Vector3(x, y, z));
                        var res = looka(area);
                        if (envmap_.ContainsKey(area))
                            envmap_[area] = res;
                        else envmap_.Add(area, res);
                    }
            //scan_impl(min ?? scanOffset, size ?? scanSize);
        }
        private static void scan_impl(Vector3Int min, Vector3Int size)
        {
            // 全ての軸の方向の大きさが1以下ならば終了
            if (size.x == 1 && size.y == 1 && size.z == 1)
            {
                var area = new Area(min);
                bool result = Physics.OverlapBoxNonAlloc(area.center, new Vector3(.5f, .5f, .5f), buffer_) > 0;
                if (envmap_.ContainsKey(area))
                    envmap_[area] = (result ? ScanResult.somethingFound : ScanResult.nothingFound);
                else
                    envmap_.Add(area, (result ? ScanResult.somethingFound : ScanResult.nothingFound));
                return;
            }
            bool res = Physics.OverlapBoxNonAlloc(
                new Vector3((min.x + size.x) / 2.0f, (min.y + size.y) / 2.0f, (min.z + size.z) / 2.0f),
                new Vector3(size.x/2.0f, size.y/2.0f, size.z/2.0f),
                buffer_
                ) > 0;
            if (!res)
            {
                for (int x = min.x; x < min.x + size.x; ++x)
                    for (int y = min.y; y < min.y + size.y; ++y)
                        for (int z = min.z; z < min.z + size.z; ++z)
                        {
                            var area = new Area(new Vector3(x, y, z));
                            if (envmap_.ContainsKey(area))
                                envmap_[area] = (ScanResult.nothingFound);
                            else
                                envmap_.Add(area, (ScanResult.nothingFound));
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
                if (((i & xmask) > 0 && !spx) || ((i & ymask) > 0 && !spy) || ((i & zmask) > 0 && !spz)) continue;
                // 軸方向に分割
                var next = new Vector3Int(
                    (i & xmask) == 0 ? min.x : min.x + (size.x+1) / 2,
                    (i & ymask) == 0 ? min.y : min.y + (size.y+1) / 2,
                    (i & zmask) == 0 ? min.z : min.z + (size.z+1) / 2
                    );
                var nextsize = new Vector3Int(
                    (i & xmask) == 0 ? (1 + size.x) / 2 : size.x / 2,
                    (i & ymask) == 0 ? (1 + size.y) / 2 : size.y / 2,
                    (i & zmask) == 0 ? (1 + size.z) / 2 : size.z / 2
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
        /// <param name="from">directionの起点</param>
        /// <returns>障害物を見つけた場合は距離、そうでない場合はnull</returns>
        /// <remarks>スキャンする最大の距離はdirectionの長さです。</remarks>
        public static float? lookd(Vector3 direction, Vector3 from) {
            var hits = new RaycastHit[4];
            var res = Physics.RaycastNonAlloc(from, direction.normalized, hits, direction.magnitude);
            Debug.DrawRay(from, direction);
            if (res == 0) return null;
            return hits.Take(res).Min(x => x.distance);
        }

        // TODO:障害物速度ベクトルへの対応
        /// <summary>
        /// 指定された小空間をスキャンし、障害物の有無を確かめる。
        /// </summary>
        /// <param name="area">見たい小空間</param>
        public static ScanResult looka(Area area)
        {
            bool accessible = Physics.OverlapBoxNonAlloc(area.center, new Vector3(.5f, .5f, .5f), buffer_) > 0;
            return accessible ? ScanResult.somethingFound : ScanResult.nothingFound;
        }

        // Sensorが構築する環境マップ
        public static ReadOnlyEnvMap envmap { get => envmap_; }
        public static ReadOnlyEnvMap envmapClone { get => new EnvMap(envmap_); }
    }
}

