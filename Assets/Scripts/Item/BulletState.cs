using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 子弹逻辑类
/// </summary>
public class BulletState : State
{
    public int x;
    public int y;
    public int speed=1;
    public Vector2 dir=Vector2.zero;
    public int width = 250;
    public int height = 250;
    public Bullet bullet;
    public long ownerId;
    public int harm = 10;

    /// <summary>
    /// 创建子弹时，传入子弹需要的所有参数，后续不会修改
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="vector"></param>
    public BulletState(int x,int y,Vector2 vector)
    {
        this.x = x;
        this.y = y;
        this.dir = vector;
    }

    
    public override void ApplyShot(SnapShot snapShot)
    {
        BulletSnapShot bulletSnapShot = snapShot as BulletSnapShot;
        x = bulletSnapShot.x;
        y=bulletSnapShot.y;
        dir = bulletSnapShot.dir;
        ownerId = bulletSnapShot.ownerId;
    }

    public override SnapShot GetShot()
    {
        BulletSnapShot bulletSnapShot = new();
        bulletSnapShot.x = x;
        bulletSnapShot.y = y;
        bulletSnapShot.dir = dir;
        bulletSnapShot.id = id;
        bulletSnapShot.ownerId = ownerId;

        return bulletSnapShot;
    }

    public override void LogicUpdate()
    {
        x += (int)(speed * dir.x * 33);
        y += (int)(speed * dir.y * 33);

        bullet.ViewUpdte(new Vector2(x / 1000f, y / 1000f));
    }
}
