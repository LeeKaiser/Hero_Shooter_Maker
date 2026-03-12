using System;
using System.Collections.Generic;
using UnityEngine;

/*
Event Bus
used to invoke an event of any type
*/
public static class EventBus<T>
{
    private static event Action<T> OnEvent;

    public static void Subscribe(Action<T> handler)
    {
        OnEvent += handler;
    }

    public static void Unsubscribe(Action<T> handler)
    {
        OnEvent -= handler;
    }

    public static void Invoke(T data)
    {
        OnEvent?.Invoke(data);
    }

    public static void Clear()
    {
        OnEvent = null;
    }

    
}

