
using System;
using UnityEngine;
using gaei.navi;
using System.Threading;
using System.Linq;

public class Sensor : MonoBehaviour
{
    public enum ScanResult
    {
        somethingFound, nothingFound, unobservable
    }

    public Sensor()
    { envmap_ = new System.Collections.Generic.Dictionary<Area, (ScanResult accessibility, Vector3? velocity)>(); }

    private Collider[] buffer_ = new Collider[2];
    private void FixedUpdate()
    {
        scan();
    }
    public void scan() {
        for (int x = -scanRadius; x <= scanRadius; ++x)
            for (int y = -scanRadius; y <= scanRadius; ++y)
                for (int z = -scanRadius; z <= scanRadius; ++z)
                {
                    var area = new Area(currentLocation + new Vector3(x, y, z));
                    int cnt = Physics.OverlapBoxNonAlloc(area.center, new Vector3(.5f, .5f, .5f), buffer_);
                    // TODO:障害物速度ベクトルへの対応
                    if (cnt > 0 && (buffer_.Contains(GetComponent<Collider>()) ? cnt == 2 : true) && envmap_[area].accessibility != ScanResult.somethingFound)
                    {
                        envmap_.Add(area, (ScanResult.somethingFound, null));
                        onCaptureObstacle(this, area, new Vector3(0, 0, 0));
                    } else if(cnt == 0 && envmap[area].accessibility == ScanResult.somethingFound)
                    {
                        envmap_.Add(area, (ScanResult.somethingFound, null));
                        onLostObstacle(this, area, new Vector3(0, 0, 0));
                    }
                }
    }

    /// <summary>
    /// 障害物を観測した際に発行されるイベントのハンドラの型
    /// </summary>
    /// <param name="sensor">イベントの送信者</param>
    /// <param name="position">観測した障害物が存在する小空間</param>
    /// <param name="velocity">観測した障害物の絶対的な移動ベクトル</param>
    public delegate void ScanEventHandler(in Sensor sensor, Area position, Vector3 velocity);

    // 障害物が観測できなくなったときに発行される
    public event ScanEventHandler onLostObstacle;
    // 障害物が移動したり、新たに観測できるようになったときに発行される
    public event ScanEventHandler onCaptureObstacle;
    // Sensorがスキャンできる半径
    public readonly int scanRadius = 10;
    private System.Collections.Generic.Dictionary<Area, (ScanResult accessibility, Vector3? velocity)> envmap_;

    /// <summary>
    /// 指定された方向をスキャンし、障害物の有無を確かめる。
    /// </summary>
    /// <param name="direction">方向</param>
    /// <param name="position">障害物があればその位置。なければ値は保証されない。</param>
    /// <param name="velocity">障害物があればその移動ベクトル。なければ値は保証されない。</param>
    /// <remarks>スキャンする最大の距離はdirectionの長さです。</remarks>
    public ScanResult lookd(Vector3 direction, ref Area position, ref Vector3 velocity) { throw new NotImplementedException(); }
    /// <summary>
    /// 指定された小空間をスキャンし、障害物の有無を確かめる。
    /// </summary>
    /// <param name="area">見たい小空間</param>
    /// <param name="velocity">障害物が存在していればその移動ベクトル。なければ値は保証されない。</param>
    public ScanResult looka(Area area, ref Vector3 velocity) {
        if(Physics.Raycast(currentLocation, area.center - currentLocation, Vector3.Distance(area.center, currentLocation) - Area.size))
            return ScanResult.unobservable;
        int cnt = Physics.OverlapBoxNonAlloc(area.center, new Vector3(.5f, .5f, .5f), buffer_);
        return cnt > 0 ? ScanResult.somethingFound : ScanResult.nothingFound;
    }

    // Sensorと紐づくオブジェクトの現在地
    public Vector3 currentLocation { get => base.transform.position; }
    // Sensorが構築する環境マップ
    public System.Collections.ObjectModel.ReadOnlyDictionary<Area, (ScanResult accessibility, Vector3? velocity)> envmap { get; }
}

