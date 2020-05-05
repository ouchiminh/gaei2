using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gaei.navi
{
    public class CAM : PathProposer
    {
        public override void onCaptureObstacle(in Sensor s, Area pos, Vector3 velocity)
        {

        }

        public override void onLostObstacle(in Sensor s, Area pos, Vector3 velocity)
        {
        }
    }
}

