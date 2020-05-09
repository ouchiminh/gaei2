
using System.Runtime.CompilerServices;
using UnityEngine;

namespace gaei.navi
{
    public struct Vector3Pos {
        public float x, y, z;
        public static implicit operator Vector3(Vector3Pos v) => new Vector3(v.x, v.y, v.z);
        public Vector3Pos(Vector3 v) { x = v.x; y = v.y; z = v.z; }
    }
    public struct Vector3Vel {
        public float x, y, z;
        public static implicit operator Vector3(Vector3Vel v) => new Vector3(v.x, v.y, v.z);
        public Vector3Vel(Vector3 v) { x = v.x; y = v.y; z = v.z; }
    }

    public abstract class PathProposer
    {
        // 経路の優先度
        GameObject self_;
        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="self">経路に沿って動いてほしいオブジェクト</param>
        public PathProposer(GameObject self, Sensor s) {
            self_ = self;
            s.onCaptureObstacle += onCaptureObstacle;
            s.onLostObstacle += onLostObstacle;
        }

        /// <summary>
        /// Sensorが障害物を発見したときに呼ばれます。
        /// </summary>
        /// <param name="s">Sensorのインスタンス</param>
        /// <param name="pos">新たに障害物が見つかった場所</param>
        /// <param name="velocity">見つかった障害物の速度</param>
        /// <remarks>
        /// 発見された障害物が新たに発見されたものとは限りません。
        /// もともと障害物がなかった小空間に障害物が観測された場合に、この関数が呼び出されます。
        /// </remarks>
        public abstract void onCaptureObstacle(in Sensor s, Area pos, Vector3 velocity);
        /// <summary>
        /// Sensorが障害物を見失ったときに呼ばれます。
        /// </summary>
        /// <param name="s">Sensorのインスタンス</param>
        /// <param name="pos">観測できなくなった障害物のあった場所</param>
        /// <param name="velocity">意味を持ちません</param>
        public abstract void onLostObstacle(in Sensor s, Area pos, Vector3 velocity);
        /// <summary>
        /// pphpで指定された局所的なゴールに到達した場合に呼ばれます。
        /// </summary>
        /// <param name="s">センサーのインスタンス</param>
        public virtual void onArrivalLocalGoal(in Sensor s) { }
        /// <summary>
        /// 大局的な目的地が変更された場合に呼ばれます。
        /// </summary>
        /// <param name="currentLocation">現在の位置</param>
        /// <param name="goal">目的地の場所</param>
        public abstract void onDestUpdate(in Sensor s, Vector3Pos goal);

        public enum Priority
        {
            collisionAvoidance, globalPath, neutral
        }

        // 位置を指定して経路を提案する
        public delegate int PathProposalHandlerPos(Vector3Pos v, Priority p);
        // 方向を指定して経路を提案する
        public delegate int PathProposalHandlerVel(Vector3Vel v, Priority p);
        // 移動指令をキャンセルする
        public delegate void ComCanceler(int commandID);

        public PathProposalHandlerPos pphp;
        public PathProposalHandlerVel pphv;
        public ComCanceler cancelCom;

    }
}
