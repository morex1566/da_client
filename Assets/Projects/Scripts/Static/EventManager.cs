using Common;
using System;
using System.Collections.Generic;

// 실행 대기 중인 이벤트들을 담는 큐
public class EventManager : MonoBehaviourSingleton<EventManager>
{
    public Queue<Action> Events = new();

    public static void Init()
    {
        GetInstance();
    }

    private void Update()
    {
        int evtCount = Events.Count;

        for ( int i = 0; i < evtCount; i++ )
        {
            Action evt = Events.Dequeue();
            evt?.Invoke();
        }
    }
}
