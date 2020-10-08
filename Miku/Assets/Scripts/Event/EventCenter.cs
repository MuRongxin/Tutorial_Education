using System;
using System.Collections.Generic;
class EventCenter
{
     private static Dictionary<EventType, Delegate> m_EvenTable = new Dictionary<EventType, Delegate>(); 
    
    private static void OnListennerAdding(EventType eventType,Delegate eventDelegate)
    {
        if (!m_EvenTable.ContainsKey(eventType))
        {
            m_EvenTable.Add(eventType, null);
        }

        Delegate d = m_EvenTable[eventType];
        if (d != null && d.GetType() != eventDelegate.GetType())
        {
            throw new Exception(string.Format("Erroy:eventType don't belong to eventDelegate", eventType, d.GetType(), eventDelegate.GetType()));
        }
    }

    private static void OnListennerRemoving(EventType eventType, Delegate eventDelegate)
    {
        if (m_EvenTable.ContainsKey(eventType))
        {
            Delegate @delegate = m_EvenTable[eventType];

            if (@delegate == null)
            {
                throw new Exception(string.Format("Error：事件{0}没有对应的委托", eventType));
            }
            else if (@delegate.GetType() != eventDelegate.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的监听，当前委托类型为{1}，要移除的委托类型为{2}", eventType, @delegate, eventDelegate));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错误：没有事件码{0}", eventType));
        }
    }
    
    private static void OnListennerRemoved(EventType eventType)
    {
        if (m_EvenTable[eventType] == null)
        {
            m_EvenTable.Remove(eventType);
        }
    }
    //无参数的添加监听的方法；
    public static void AddListener(EventType eventType,EventDelegate eventDelegate)
    {
        OnListennerAdding(eventType, eventDelegate);
        m_EvenTable[eventType] = (EventDelegate)m_EvenTable[eventType] + eventDelegate;
    }

    //带一个参数的添加监听的方法；
    public static void AddListener<T>(EventType eventType, EventDelegate<T> eventDelegate)
    {
        OnListennerAdding(eventType, eventDelegate);
        m_EvenTable[eventType] = (EventDelegate<T>)m_EvenTable[eventType] + eventDelegate;
    }
    //
    public static void AddListener<T,X>(EventType eventType,EventDelegate<T,X> eventDelegate)
    {
        OnListennerAdding(eventType, eventDelegate);
        m_EvenTable[eventType] = (EventDelegate<T, X>)m_EvenTable[eventType] + eventDelegate;
    }
    //移除监听的方法;
    public static void RemoveLister(EventType eventType, EventDelegate eventDelegate)
    {
        OnListennerRemoving(eventType, eventDelegate);
        m_EvenTable[eventType] = (EventDelegate)m_EvenTable[eventType] - eventDelegate;
        OnListennerRemoved(eventType);
    }

    //
    public static void RemoveLister<T>(EventType eventType, EventDelegate<T> eventDelegate)
    {
        OnListennerRemoving(eventType, eventDelegate);
        m_EvenTable[eventType] = (EventDelegate<T>)m_EvenTable[eventType] - eventDelegate;
        OnListennerRemoved(eventType);
    }

    public static void RemoveLister<T,X>(EventType eventType, EventDelegate<T,X> eventDelegate)
    {
        OnListennerRemoving(eventType, eventDelegate);
        m_EvenTable[eventType] = (EventDelegate<T,X>)m_EvenTable[eventType] - eventDelegate;
        OnListennerRemoved(eventType);
    }

    //广播监听
    public static void Broadcast(EventType eventType)
    {
        Delegate d;

        if (m_EvenTable.TryGetValue(eventType, out d))
        {
            EventDelegate eventDelegate = d as EventDelegate;

            if (eventDelegate != null)
            {
                eventDelegate();
            }
            else
            {
                throw new Exception(string.Format("广播事件错误，事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }

    //
    public static void Broadcast<T>(EventType eventType,T arg)
    {
        Delegate d;

        if (m_EvenTable.TryGetValue(eventType, out d))
        {
            EventDelegate<T> eventDelegate = d as EventDelegate<T>;

            if (eventDelegate != null)
            {
                eventDelegate(arg);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误，事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }

    public static void Broadcast<T,X>(EventType eventType,T arg,X arg2)
    {
        Delegate d;

        if (m_EvenTable.TryGetValue(eventType, out d))
        {
            EventDelegate<T,X> eventDelegate = d as EventDelegate<T,X>;

            if (eventDelegate != null)
            {
                eventDelegate(arg,arg2);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误，事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }
}