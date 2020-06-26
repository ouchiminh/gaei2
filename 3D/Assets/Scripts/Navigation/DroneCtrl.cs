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
        public Vector3 velocity_;
        public Vector3 velocity { get=>velocity_; private set=>velocity_=value; }
        Navigator navi_;
        Fuhrer fuhrer_;

        /********debug********/
        int __i = 0;
        /*********************/

        public void initialize(Fuhrer fuhrer) { fuhrer_ = fuhrer; }
        private void Start()
        {
            navi_ = GetComponent<Navigator>();
            velocity = default;
            fuhrer_ = Fuhrer.instance;
            status_ = (uint)Status.idle;
            Bounds? b = default;
            foreach (var x in gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                if (b.HasValue) b.Value.Encapsulate(x.bounds);
                else b = x.bounds;
            }
            var m = 1.0f / b.Value.size.magnitude;
            //transform.localScale=new Vector3(m, m, m);
            Debug.Log(m);
        }
        void Update()
        {
            gameObject.transform.Translate(velocity*Time.deltaTime);
        }
        private void FixedUpdate()
        {
            if(__i < 20 && ++__i == 10)
                setDestination(new Area(-66, 1, -20));
            Sensor.scan(new Area(transform.position - new Vector3(5, 5, 5)).representativePoint, new Vector3Int(CAM.radius, CAM.radius, CAM.radius));
            var velbuf = navi_.getNextCourse(Sensor.envmap);
            velocity = velbuf.sqrMagnitude < sqrMaxSpeed ? velbuf : velbuf.normalized * maxSpeed;
            if (navi_.remainingWayPointCount == 0)
            {
                status = status == Status.delivery ? Status.homing : Status.idle;
                Fuhrer.instance.updateDroneState(this);
                return;
            }
        }
        public void setDestination(Area dest)
        {
            status_ = (status_+1)%3;
            Sensor.scan();
            navi_.setDestination(dest, Sensor.envmapClone);
        }
    }
}

