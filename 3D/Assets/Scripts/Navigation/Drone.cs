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
        public enum Status
        {
            idle, delivery, homing
        }
        public Status status { get => (Status)(status_ %= 3); set { status_ = (uint)value; } }
        public uint status_;
        Navigator navi_;
        Fuhrer fuhrer_;

        public void initialize(Fuhrer fuhrer) { fuhrer_ = fuhrer; }
        void Update()
        {
            gameObject.transform.Translate(Time.deltaTime * navi_.getNextCourse(Sensor.envmap));
        }
        private void FixedUpdate()
        {
            if (navi_.remainingWayPointCount == 0)
            {
                fuhrer_.updateDroneState(this);
            }
        }
    }
}

