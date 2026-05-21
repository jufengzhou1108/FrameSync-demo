using Fantasy;
using UnityEngine;

public abstract class State
{
    public int bornFrame;     //该物体创建时的帧号
    public long id;            //该物体的id

    public abstract void LogicUpdate();
    /// <summary>
    /// 获取快照 
    /// </summary>
    /// <returns></returns>
    public abstract SnapShot GetShot();
    /// <summary>
    /// 应用快照 
    /// </summary>
    public abstract void ApplyShot(SnapShot snapShot);
}