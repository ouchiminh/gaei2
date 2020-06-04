using System;
using System.Collections.Generic;
using System.Linq;
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
        public Status status {get; set;}
    }
}
