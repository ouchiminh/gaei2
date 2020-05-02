
using System;

namespace gaei.navi
{
    public class Sensor
    {
        public enum scanResult
        {
            somethingFound, nothingFound, unobservable
        }

        /// <summary>
        /// 障害物を観測した際にトリガーされるイベントのハンドラの型
        /// </summary>
        /// <param name="position">観測した障害物が存在する小空間</param>
        /// <param name="sensor">イベントの送信者</param>
        /// <param name="velocity">観測した障害物の絶対的な移動ベクトル</param>
        public delegate void ScanEvent(Area position, Sensor sensor, UnityEngine.Vector3 velocity);

        // これまで発見していた障害物を見失った際に発行される
        public ScanEvent onLostObstacle;
        // 未発見或いは見失っていた障害物を捕捉した際に発行される
        public ScanEvent onCaptureObstacle;

        public void scan() { throw new NotImplementedException(); }

        /// <summary>
        /// 指定された方向をスキャンし、障害物の有無を確かめる。
        /// </summary>
        /// <param name="direction">方向</param>
        /// <param name="position">障害物があればその位置。なければ値は保証されない。</param>
        /// <param name="velocity">障害物があればその移動ベクトル。なければ値は保証されない。</param>
        /// <exception cref="NullReferenceException">velocityかpositionにnullが指定されていて、かつ 障害物が見つかった場合</exception>
        public scanResult lookd(UnityEngine.Vector3 direction, ref Area position, ref UnityEngine.Vector3 velocity) { throw new NotImplementedException(); }
        /// <summary>
        /// 指定された小空間をスキャンし、障害物の有無を確かめる。
        /// </summary>
        /// <param name="area">見たい小空間</param>
        /// <param name="velocity">障害物が存在していればその移動ベクトル。なければ値は保証されない。</param>
        public scanResult looka(Area area, ref UnityEngine.Vector3 velocity) { throw new NotImplementedException(); }
    }
}

