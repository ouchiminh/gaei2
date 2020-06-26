using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace gaei.navi {
    using ReadOnlyEnvMap = IReadOnlyDictionary<Area, Sensor.ScanResult>;
    public class Navigator : MonoBehaviour
    {
        GlobalPathProposer globalPathProposer_;
        LocalPathProposer localPathProposer_;
        LinkedList<Area> path_;
        GameObject localgoal_;
        System.Threading.Tasks.Task<IEnumerable<gaei.navi.Area>> async_path_;
        Navigator()
        {
            globalPathProposer_ = new Pilot();
            localPathProposer_ = new CAM();
            path_ = new LinkedList<Area>();
            async_path_ = null;
        }
        private void Start()
        {
            localgoal_ = UnityEngine.Object.Instantiate((Resources.Load("Marker") as GameObject));
            localgoal_.SetActive(false);
        }
        public int remainingWayPointCount { get => path_.Count; }
        public Vector3 getNextCourse(in ReadOnlyEnvMap envmap) {
            if (async_path_ != null && async_path_.IsCompleted)
            {
                path_ = new LinkedList<Area>(async_path_.Result);
                async_path_ = null;
            }
            if (remainingWayPointCount == 0)
            {
                localgoal_.SetActive(false);
                return localPathProposer_.getCourse(transform.position, transform.position, envmap);
            }
            if (path_.First().CompareTo(new Area(gameObject.transform.position)) == 0) {
                localgoal_.SetActive(remainingWayPointCount > 2);
                path_.RemoveFirst(); path_.RemoveFirst();
                localgoal_.transform.position = path_.First.Value.center;
                Debug.Log(remainingWayPointCount);
            }
            return localPathProposer_.getCourse(path_.First().center, gameObject.transform.position, envmap);
        }
        public async void setDestination(Area dest, ReadOnlyEnvMap envmap) {
            var t = transform.position;
            System.Func<System.Threading.Tasks.Task<IEnumerable<gaei.navi.Area>>> f = async () => globalPathProposer_.getPath(dest.center, t, envmap);
            Debug.Log("async");
            async_path_ = System.Threading.Tasks.Task.Run<IEnumerable<gaei.navi.Area>>(f);
            Debug.Log("cb");
        }
    }
}

