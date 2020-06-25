﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace gaei.navi {
    using ReadOnlyEnvMap = IReadOnlyDictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)>;
    public class Navigator : MonoBehaviour
    {
        GlobalPathProposer globalPathProposer_;
        LocalPathProposer localPathProposer_;
        LinkedList<Area> path_;
        void Start()
        {
            globalPathProposer_ = new Pilot();
            localPathProposer_ = new CAM();
            path_ = new LinkedList<Area>();
        }
        public int remainingWayPointCount { get => path_.Count; }
        public Vector3 getNextCourse(in ReadOnlyEnvMap envmap) {
            if (remainingWayPointCount == 0) return localPathProposer_.getCourse(transform.position, transform.position, envmap);
            if (path_.First().CompareTo(new Area(gameObject.transform.position)) == 0) path_.RemoveFirst();
            return localPathProposer_.getCourse(path_.First().center, gameObject.transform.position, envmap);
        }
        public void setDestination(Area dest, in ReadOnlyEnvMap envmap) {
            path_ = new LinkedList<Area>(globalPathProposer_.getPath(dest.center, gameObject.transform.position, envmap));
        }
    }
}

