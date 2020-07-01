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
        const float maxSpeed = 2;
        const float sqrMaxSpeed = maxSpeed * maxSpeed;
        public enum Status
        {
            idle, delivery, homing
        }
        public Status status { get => (Status)(status_ %= 3); private set { status_ = (uint)value; } }
        public uint status_;
        public Vector3 velocity { get; private set; }
        private bool isUpToDate_;
        Navigator navi_;

        private void Start()
        {
            navi_ = GetComponent<Navigator>();
            velocity = default;
            status_ = (uint)Status.idle;
            isUpToDate_ = false;
        }
        void Update()
        {
            gameObject.transform.Translate(velocity*Time.deltaTime);
        }
        private void FixedUpdate()
        {
            var velbuf = navi_.getNextCourse(Sensor.envmap);
            velocity = velbuf.sqrMagnitude < sqrMaxSpeed ? velbuf : velbuf.normalized * maxSpeed;
            if (navi_.remainingWayPointCount == 0 && !isUpToDate_)
            {
                status = (status == Status.delivery ? Status.homing : Status.idle);
                Fuhrer.instance.updateDroneState(this);
                isUpToDate_ = true;
                return;
            }
        }
        public void setDestination(Area dest)
        {
            isUpToDate_ = false;
            status_ = (status_+1)%3;
            Sensor.scan();
            navi_.setDestination(dest, Sensor.envmapClone);
        }
    }
}

