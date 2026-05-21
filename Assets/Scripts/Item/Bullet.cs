using UnityEngine;

public class Bullet:MonoBehaviour
{
    private Vector2 oriPosition;
    private Vector2 aimPosition;
    private Vector2 nowPosition;
    private float t;

    /// <summary>
    /// 根据每帧执行时间插值计算当前渲染位置
    /// </summary>
    private void Update()
    {
        t += Time.deltaTime / 0.033f;
        t = Mathf.Clamp01(t);

        nowPosition = Vector2.Lerp(oriPosition, aimPosition, t);

        this.transform.position = new Vector3(nowPosition.x, 1, nowPosition.y);
    }

    //更新当前的目标位置
    public void ViewUpdte(Vector2 aimPosition)
    {
        t = 0;

        oriPosition = nowPosition;
        this.aimPosition = aimPosition;
    }

    /// <summary>
    /// 初始化位置
    /// </summary>
    public void Init(Vector2 position)
    {
        nowPosition = position;
        oriPosition = position;
        aimPosition = position;
        t = 1;
    }
}