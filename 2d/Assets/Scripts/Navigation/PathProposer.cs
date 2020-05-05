
using UnityEngine;

namespace gaei.navi
{
    public struct Vector3Pos {
        public float x, y, z;
    }
    public struct Vector3Vel {
        public float x, y, z;
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
        public abstract void onCaptureObstacle(in Sensor s, Area pos, Vector3 velocity);
        public abstract void onLostObstacle(in Sensor s, Area pos, Vector3 velocity);
        public abstract void onDestUpdate(Vector3Pos v);

        // 位置を指定して経路を提案する
        public delegate void PathProposalHandlerPos(Vector3Pos v);
        // 方向を指定して経路を提案する
        public delegate void PathProposalHandlerVel(Vector3Vel v);

        public event PathProposalHandlerPos pphp;
        public event PathProposalHandlerVel pphv;

    }
}
