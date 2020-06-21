using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using gaei.navi;
using System.Linq;

namespace Tests
{
    public class NewTestScriptEdit
    {
        // A Test behaves as an ordinary method
        [Test]
        public void Pilot_test1()
        {
            var envmap = new System.Collections.Generic.Dictionary<Area, (Sensor.ScanResult accsessibility, Vector3? velocity)>();
            Vector3 dest = new Vector3(0, 0, 0);
            Vector3 here = new Vector3(0, 4, 0);
            for(int i = 0; i < 5; ++i)
            {
                for(int j = 0; j < 5; ++j)
                {
                    envmap.Add(new Area(i, j, 0),(Sensor.ScanResult.nothingFound,default(Vector3)));
                }
            }
            envmap[new Area(1, 0, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(2, 0, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(3, 0, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(4, 0, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(0, 2, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(1, 2, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(2, 2, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(3, 2, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(3, 3, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(1, 4, 0)] = (Sensor.ScanResult.somethingFound, null);
            var map=new System.Collections.ObjectModel.ReadOnlyDictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)>(envmap);
            var route = new Pilot().getPath(dest, here, map);
            Assert.AreEqual(15,route.Count());
        }
        [Test]
        public void Pilot_test2()
        {
            var envmap = new System.Collections.Generic.Dictionary<Area, (Sensor.ScanResult accsessibility, Vector3? velocity)>();
            Vector3 dest = new Vector3(1, 0, 0);
            Vector3 here = new Vector3(0, 4, 0);
            for(int i = 0; i < 5; ++i)
            {
                for(int j = 0; j < 5; ++j)
                {
                    envmap.Add(new Area(i, j, 0),(Sensor.ScanResult.nothingFound,default(Vector3)));
                }
            }
            // h * * * *
            // - - - - -
            // * * * - -
            // * * * - -
            // - d - - -
            envmap[new Area(1, 4, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(2, 4, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(3, 4, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(4, 4, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(0, 2, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(1, 2, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(2, 2, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(0, 1, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(1, 1, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(2, 1, 0)] = (Sensor.ScanResult.somethingFound, null);
            var map=new System.Collections.ObjectModel.ReadOnlyDictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)>(envmap);
            var route = new Pilot().getPath(dest, here, map);
            Assert.AreEqual(10,route.Count());
        }
        [Test]
        public void CAM_test1()
        {
            var envmap = new System.Collections.Generic.Dictionary<Area, (Sensor.ScanResult accsessibility, Vector3? velocity)>();
            Vector3 dest = new Area(1, 3, 0).center;
            Vector3 here = new Area(1, 2, 0).center;
            // * * *
            // - - -
            // - h -
            // - d -
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    envmap.Add(new Area(i, j, 0), (Sensor.ScanResult.nothingFound, default(Vector3)));
                }
            }
            envmap[new Area(0, 0, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(1, 0, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(2, 0, 0)] = (Sensor.ScanResult.somethingFound, null);
            var map = new System.Collections.ObjectModel.ReadOnlyDictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)>(envmap);
            Vector3 direction = new CAM().getCourse(dest, here, map);
            Vector3 ans = new Vector3(0,1,0);
            Assert.AreEqual(ans,direction.normalized);
        }
        [Test]
        public void CAM_test2()
        {
            var envmap = new System.Collections.Generic.Dictionary<Area, (Sensor.ScanResult accsessibility, Vector3? velocity)>();
            Vector3 dest = new Area(2, 1, 0).center;
            Vector3 here = new Area(1, 1, 0).center;
            // * * -
            // - h d
            // - - -
            for (int i = 0; i < 5; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    envmap.Add(new Area(i, j, 0), (Sensor.ScanResult.nothingFound, default(Vector3)));
                }
            }
            envmap[new Area(0, 0, 0)] = (Sensor.ScanResult.somethingFound, null);
            envmap[new Area(1, 0, 0)] = (Sensor.ScanResult.somethingFound, null);
            var map = new System.Collections.ObjectModel.ReadOnlyDictionary<Area, (Sensor.ScanResult accessibility, Vector3? velocity)>(envmap);
            Vector3 direction = new CAM().getCourse(dest, here, map);
            Assert.Greater(direction.x, 0);
            Assert.Greater(direction.y, 0);
            Assert.AreEqual(0, direction.z);
        }
    }
}
