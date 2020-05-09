using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace gaei.navi
{
    public class CAM : PathProposer
    {
        public CAM(GameObject self, Sensor s)
            : base(self, s) { }
        public override void onCaptureObstacle(in Sensor s, Area pos, Vector3 velocity)
        {
            // TODO:移動障害物と処理を分離して軽量化
            Vector3 current = default(Vector3);
            bool shouldKeepAvoiding = false;
            foreach(var a in from x in s.envmap where x.Value.accessibility == Sensor.ScanResult.somethingFound select x)
            {
                var r = a.Key.center - s.currentLocation;
                current +=  r / Vector3.SqrMagnitude(r);
                shouldKeepAvoiding |= a.Value.velocity == null || a.Value.velocity.Value.magnitude == 0 ? false : true;
            }
            if (!shouldKeepAvoiding) cancelCom(lastComId_);
            lastComId_ = pphv(new Vector3Vel(current), Priority.collisionAvoidance);
        }

        public override void onLostObstacle(in Sensor s, Area pos, Vector3 velocity)
        {}
        public override void onDestUpdate(in Sensor s, Vector3Pos goal)
        {
            dest_ = goal;
        }

        Vector3 dest_;
        int lastComId_;
    }
}

