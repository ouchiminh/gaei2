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
        public Status status { get => (Status)(status_ %= 3); private set { status_ = (uint)value; } }
        public uint status_;
        public Vector3 velocity { get; private set; }
        Navigator navi_;
        Fuhrer fuhrer_;

        public void initialize(Fuhrer fuhrer) { fuhrer_ = fuhrer; }
        private void Start()
        {
            navi_ = GetComponent<Navigator>();
            velocity = default;
            fuhrer_ = Fuhrer.instance;
            status_ = (uint)Status.idle;
            //var scale = 1.0f / gameObject.GetComponent<MeshCollider>().bounds.size.magnitude;
            //transform.localScale = new Vector3(scale, scale, scale);
        }
        void Update()
        {
            gameObject.transform.Translate(velocity*Time.deltaTime);
        }
        private void FixedUpdate()
        {
            Debug.Log(Sensor.envmap.Count);
            var velbuf = navi_.getNextCourse(Sensor.envmap);
            velocity = velbuf.sqrMagnitude < sqrMaxSpeed ? velbuf : velbuf.normalized * maxSpeed;
            if (navi_.remainingWayPointCount == 0)
            {
                status = status == Status.delivery ? Status.homing : Status.idle;
                fuhrer_.updateDroneState(this);
                return;
            }
        }
        public void setDestination(Area dest)
        {
            status_ = (status_+1)%3;
            navi_.setDestination(dest, Sensor.envmap);
        }
    }
}

