using gaei.navi;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    public Vector3 destination {
        get => dest_;
        set {
            var s = GetComponent<Sensor>();
            pathProposers_.ForEach(x => x.onDestUpdate(s, new Vector3Pos(value)));
            dest_ = value;
        }
    }
    void Start()
    {
        var s = GetComponent<Sensor>();
        pathProposers_ = new List<PathProposer> { new CAM(base.gameObject, s) };
        comID_ = 0;
        foreach (var x in pathProposers_)
        {
            x.pphp += pathPropossalHandlerPos;
            x.pphv += pathPropossalHandlerVel;
            x.cancelCom += cancelCom;
        }
    }

    private void FixedUpdate()
    {
        if (!isPosDesignated_)
        {
            gameObject.transform.position += vector_.normalized;
            return;
        }
        if((new Area(gameObject.transform.position).center - vector_).sqrMagnitude < 1)
            pathProposers_.ForEach(x => x.onArrivalLocalGoal(GetComponent<Sensor>()));

        var direction = vector_ - gameObject.transform.position;
        gameObject.transform.position += direction.normalized;
    }

    private int comID_;
    private int pathPriority_;
    private bool isPosDesignated_;
    private Vector3 vector_;

    private Vector3 dest_;

    private int pathPropossalHandlerPos(Vector3Pos v, PathProposer.Priority p) {
        if (pathPriority_ < (int)p) return 0;
        pathPriority_ = (int)p;
        isPosDesignated_ = true;
        vector_ = v;
        return ++comID_ == 0 ? ++comID_ : comID_;
    }
    private int pathPropossalHandlerVel(Vector3Vel v, PathProposer.Priority p) {
        if (pathPriority_ < (int)p) return 0;
        pathPriority_ = (int)p;
        isPosDesignated_ = false;
        vector_ = v;
        return ++comID_ == 0 ? ++comID_ : comID_;
    }
    private void cancelCom(int commandID)
    {
        if (comID_ != commandID) return;
        pathPriority_ = int.MaxValue;
        isPosDesignated_ = false;
        vector_ = new Vector3(0, 0, 0);
    }

    private List<PathProposer> pathProposers_;
}
