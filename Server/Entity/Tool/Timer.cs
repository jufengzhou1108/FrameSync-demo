using System.Diagnostics;
public class Timer 
{
    private long nextTriggerSeconds;
    private Action action;
    private Stopwatch stopwatch=new();
    private bool isRunning = false;
    private int intervalTime;

    //创建循环任务 
    public void StartRepeatTimer(int intervalTime,Action action)
    {
        if (intervalTime == 0)
        {
           Console.WriteLine("intervaltime不能为0");
        }

        //先重置计时器
        Reset();

        this.action = action;
        nextTriggerSeconds = 0;
        isRunning = true;

        action?.Invoke();
        nextTriggerSeconds += intervalTime;
        this.intervalTime = intervalTime;
        stopwatch.Start();

        LoopTick();
    }

    //开始轮询计时
    private async void LoopTick()
    {
        while (isRunning)
        {
            await Task.Delay(intervalTime);
            Tick();
        }
    }

    //单次触发事件
    private void Tick()
    {
        while (isRunning&&stopwatch.ElapsedMilliseconds >= nextTriggerSeconds)
        {
            action?.Invoke();
            nextTriggerSeconds += intervalTime;
        }
    }


    public void End()
    {
        isRunning = false;
    }

    private void Reset()
    {
        nextTriggerSeconds = 0;
        action = null;
        stopwatch.Reset();
        isRunning = false;
    }
}
