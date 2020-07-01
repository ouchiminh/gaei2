using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace gaei.navi {
    using ReadOnlyEnvMap = IReadOnlyDictionary<Area, Sensor.ScanResult>;
    public class Navigator : MonoBehaviour
    {
        GlobalPathProposer globalPathProposer_;
        LocalPathProposer localPathProposer_;
        LinkedList<Area> path_;

        GameObject localgoal_;
        GameObject globalgoal_;
        System.Threading.Tasks.Task<IEnumerable<gaei.navi.Area>> async_path_;

        public bool hasDestination { get; private set; }
        Navigator()
        {
            globalPathProposer_ = new Pilot();
            localPathProposer_ = new CAM();
            path_ = new LinkedList<Area>();
            async_path_ = null;
            hasDestination = false;
        }
        private void Start()
        {
            localgoal_ = UnityEngine.Object.Instantiate((Resources.Load("Marker") as GameObject));
            localgoal_.SetActive(false);
            globalgoal_ = UnityEngine.Object.Instantiate((Resources.Load("Destination") as GameObject));
            globalgoal_.SetActive(false);
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
                globalgoal_.SetActive(false);
                return localPathProposer_.getCourse(null, transform.position);
            }
            localgoal_.SetActive(true);
            globalgoal_.SetActive(true);
            localgoal_.transform.position = path_.First.Value.center;
            if (Area.distance(path_.First(), new Area(gameObject.transform.position)) <= 1) {
                path_.RemoveFirst();
                hasDestination = remainingWayPointCount > 0;
                Debug.Log(remainingWayPointCount);
            }
            return localPathProposer_.getCourse(remainingWayPointCount == 0 ? (Vector3?)null : path_.First().center,
                                                gameObject.transform.position);
        }
        public async void setDestination(Area dest, ReadOnlyEnvMap envmap) {
            var t = transform.position;
            hasDestination = true;
            System.Func<System.Threading.Tasks.Task<IEnumerable<gaei.navi.Area>>> f = async () => globalPathProposer_.getPath(dest.center, t, envmap);
            async_path_ = System.Threading.Tasks.Task.Run<IEnumerable<gaei.navi.Area>>(f);
            globalgoal_.SetActive(true);
            globalgoal_.transform.position = dest.center;
        }
    }
}

