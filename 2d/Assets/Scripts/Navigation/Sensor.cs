
using System;
using UnityEngine;

namespace gaei.navi
{
    public class Sensor
    {
        Sensor(GameObject observatory, int scanRadius) {
            observatory_ = observatory;
            this.scanRadius = scanRadius;
        }

        public enum ScanResult
        {
            somethingFound, nothingFound, unobservable
        }

        /// <summary>
        /// 障害物を観測した際に発行されるイベントのハンドラの型
        /// </summary>
        /// <param name="sensor">イベントの送信者</param>
        /// <param name="position">観測した障害物が存在する小空間</param>
        /// <param name="velocity">観測した障害物の絶対的な移動ベクトル</param>
        public delegate void ScanEventHandler(in Sensor sensor, Area position, Vector3 velocity);

        // 障害物が観測できなくなったときに発行される
        public event ScanEventHandler onLostObstacle;
        // 障害物が移動したり、新たに観測できるようになったときに発行される
        public event ScanEventHandler onCaptureObstacle;
        // Sensorがスキャンできる半径
        public readonly int scanRadius;
        private GameObject observatory_;
        private System.Collections.Generic.Dictionary<Area, (ScanResult accessibility, Vector3? velocity)> envmap_;

        public void scan() { throw new NotImplementedException(); }

        /// <summary>
        /// 指定された方向をスキャンし、障害物の有無を確かめる。
        /// </summary>
        /// <param name="direction">方向</param>
        /// <param name="position">障害物があればその位置。なければ値は保証されない。</param>
        /// <param name="velocity">障害物があればその移動ベクトル。なければ値は保証されない。</param>
        /// <remarks>スキャンする最大の距離はdirectionの長さです。</remarks>
        public ScanResult lookd(Vector3 direction, ref Area position, ref Vector3 velocity) { throw new NotImplementedException(); }
        /// <summary>
        /// 指定された小空間をスキャンし、障害物の有無を確かめる。
        /// </summary>
        /// <param name="area">見たい小空間</param>
        /// <param name="velocity">障害物が存在していればその移動ベクトル。なければ値は保証されない。</param>
        public ScanResult looka(Area area, ref Vector3 velocity) { throw new NotImplementedException(); }

        // Sensorと紐づくオブジェクトの現在地
        public Vector3 currentLocation { get => observatory_.transform.position; }
        // Sensorが構築する環境マップ
        public System.Collections.ObjectModel.ReadOnlyDictionary<Area, (ScanResult accessibility, Vector3? velocity)> envmap {get;}
    }
}

