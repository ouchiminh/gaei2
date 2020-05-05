﻿using System.Collections;
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
            Vector3 current = default(Vector3);
            foreach(var a in from x in s.envmap where x.Value.accessibility != Sensor.ScanResult.unobservable select x)
            {
                var r = a.Key.center - s.currentLocation;
                current +=  r / Vector3.SqrMagnitude(r);
            }
            pphv(new Vector3Vel(current));
        }

        public override void onLostObstacle(in Sensor s, Area pos, Vector3 velocity)
        {}
        public override void onDestUpdate(Vector3Pos v)
        {
            dest_ = v;
        }

        Vector3 dest_;
    }
}
