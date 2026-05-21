using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public static class ColliderTool
{
    /// <summary>
    /// 处理每帧物体的碰撞
    /// </summary>
    public static void FrameCollide(List<PlayerState> playerStates,List<Transform> walls,List<BulletState> bulletStates)
    {
        BulletWithPlayer(playerStates, bulletStates);
        WallWithBullet(walls, bulletStates);
        WallWithPlayer( playerStates,walls);
    }

    /// <summary>
    /// 如果子弹碰到墙面，那么销毁子弹
    /// </summary>
    public static void WallWithBullet(List<Transform> walls, List<BulletState> bulletStates)
    {
        foreach(Transform wall in walls)
        {
            Vector3 size=wall.GetComponent<BoxCollider>().bounds.size;
            for(int i=bulletStates.Count-1;i>-1;i--)
            {
                BulletState bulletState = bulletStates[i];
                if (IsCollide((int)(wall.position.x * 1000),(int) (wall.position.z * 1000), Tool.FloatToInt(size.x), Tool.FloatToInt(size.z), bulletState.x, bulletState.y, bulletState.width, bulletState.height))
                {
                    StateManager.Instance.RemoveBullet(bulletState);
                    bulletStates.RemoveAt(i);
                }
            }
        }
    }

    /// <summary>
    /// 处理子弹和玩家的碰撞
    /// </summary>
    public static void BulletWithPlayer(List<PlayerState> playerStates, List<BulletState> bulletStates)
    {
        foreach (PlayerState playerState in playerStates)
        {
            for(int i = bulletStates.Count - 1; i > -1; i--)
            {
                BulletState bulletState = bulletStates[i];
                //如果没有碰撞那么跳过该子弹
                if (!IsCollide(playerState.x,playerState.y,playerState.width,playerState.height, bulletState.x, bulletState.y, bulletState.width, bulletState.height))
                {
                    continue;
                }

                if (playerState.id != bulletState.ownerId)
                {
                    playerState.Attacked(bulletState.harm);
                    StateManager.Instance.RemoveBullet(bulletState);
                    bulletStates.RemoveAt(i);
                }
            }
        }
    }

    /// <summary>
    /// 处理墙壁和玩家的碰撞
    /// </summary>
    public static void WallWithPlayer(List<PlayerState> playerStates, List<Transform> walls)
    {
        foreach (PlayerState playerState in playerStates)
        {
            foreach(Transform wall in walls)
            {
                Vector3 size = wall.GetComponent<Collider>().bounds.size;
                if (!IsCollide((int)(wall.position.x * 1000), (int)(wall.position.z * 1000), Tool.FloatToInt(size.x), Tool.FloatToInt(size.z), playerState.x, playerState.y, playerState.width, playerState.height))
                {
                    continue;
                }

                //处理玩家和墙壁碰撞
                playerState.x = Tool.ClampInt(playerState.x, -14500, 14500);
                playerState.y = Tool.ClampInt(playerState.y, -14500, 14500);
            }
        }
    }


    /// <summary>
    /// 判断两个矩形是否碰撞
    /// </summary>
    /// <returns></returns>
    public static bool IsCollide(int x1,int y1,int width1,int height1, int x2, int y2,int width2,int height2)
    {
        //获取两个物体的上下左右边界
        int left1 = x1 - width1 / 2;
        int right1 = x1 + width1 / 2;
        int top1 = y1 + height1 / 2;
        int bottom1 = y1 - height1 / 2;

        int left2 = x2 - width2 / 2;
        int right2 = x2 + width2 / 2;
        int top2 = y2 + height2 / 2;
        int bottom2 = y2 - height2 / 2;

        //如果两个物体中的一个的左边界不在另一个的右边界右边，右边界不在左边界左边，上不在下下，
        //下不在上上，那么就可以认为没有碰撞
        if (left1 > right2 || right1 < left2 || top1 < bottom2 || bottom1 > top2)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}