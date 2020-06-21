using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace gaei.navi
{
    public class DroneCtrl : MonoBehaviour
    {
        // 最大の速さ (area/seconds)
        const float maxSpeed = 1;
        const float sqrMaxSpeed = maxSpeed * maxSpeed;
        public enum Status
        {
            idle, delivery, homing
        }
        public Status status { get => (Status)(status_ %= 3); set { status_ = (uint)value; } }
        public uint status_;
        public Vector3 velocity { get; private set; }
        Navigator navi_;
        Fuhrer fuhrer_;

        public void initialize(Fuhrer fuhrer) { fuhrer_ = fuhrer; }
        void Update()
        {
            gameObject.transform.Translate(velocity*Time.deltaTime);
        }
        private void FixedUpdate()
        {
            var velbuf = navi_.getNextCourse(Sensor.envmap);
            velocity = velbuf.sqrMagnitude < sqrMaxSpeed ? velbuf : velbuf.normalized * maxSpeed;

            if (navi_.remainingWayPointCount == 0)
            {
                fuhrer_.updateDroneState(this);
            }
        }
        public void setDestination(Area dest)
        {
            navi_.setDestination(dest, Sensor.envmap);
        }
    }
}

