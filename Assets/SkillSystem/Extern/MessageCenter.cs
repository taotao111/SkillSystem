using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class IMessage
{
}
public interface IMessageListener
{
    void HandleMessage(IMessage message);
}
public class MessageCenter<T>  {

    private Dictionary<T, MessageLayer> m_MessageLayers = new Dictionary<T, MessageLayer>();
    private List<MessageSendLater> m_LaterMessage = new List<MessageSendLater>();
    public void Create()
    {
        foreach (T layer in Enum.GetValues(typeof(T)))
        {
            m_MessageLayers.Add(layer, new MessageLayer());
        }
    }
    
    public void Update(float elapsed_sec)
    {
        foreach (var layer in m_MessageLayers)
        {
            layer.Value.Update(elapsed_sec);
        }

        for(int i = m_LaterMessage.Count - 1; i >= 0; i--)
        {
            m_LaterMessage[i].Update(elapsed_sec);
            if (m_LaterMessage[i].IsExpire)
            {
                m_LaterMessage.RemoveAt(i);
            }
        }
    }
    
    public void AddListener(IMessageListener listener, T layer)
    {
        m_MessageLayers[layer].AddListener(listener);
    }
    
    public void RemoveListener(IMessageListener listener, T layer)
    {
        m_MessageLayers[layer].RemoveListener(listener);
    }
    
    public void SendMessage(IMessage message, T layer, float delay = 0)
    {
        m_MessageLayers[layer].SendMessage(message, delay);
    }
    
    public void SendMessage(IMessage message, IMessageListener listener, float delay = 0)
    {
        if(delay <= 0)
        {
            listener.HandleMessage(message);
        }
        else
        {
            m_LaterMessage.Add(new MessageSendLater(listener, message, delay));
        }
    }
}
public class MessageLayer
{
    List<IMessageListener> m_Listeners = new List<IMessageListener>();
    List<DelayMessage> m_Messages = new List<DelayMessage>();

    public void Update(float elapsed_sec)
    {
        for (int index = m_Messages.Count - 1; index >= 0; index--)
        {
            m_Messages[index].duration -= elapsed_sec;
            if (m_Messages[index].duration <= 0)
            {
                DistributeMessage(m_Messages[index].message);
                m_Messages.RemoveAt(index);
            }
        }
    }

    public void AddListener(IMessageListener listener)
    {
        m_Listeners.Add(listener);
    }

    public void RemoveListener(IMessageListener listener)
    {
        m_Listeners.Remove(listener);
    }

    public void SendMessage(IMessage message, float delay = 0)
    {
        if (delay <= 0)
        {
            DistributeMessage(message);
        }
        else
        {
            m_Messages.Add(new DelayMessage(message, delay));
        }
    }

    void DistributeMessage(IMessage message)
    {
        for(int i = 0;i < m_Listeners.Count; i++)
        {
            m_Listeners[i].HandleMessage(message);
        }
    }

    class DelayMessage
    {
        public IMessage message { get; private set; }
        public float duration { get; set; }

        public DelayMessage(IMessage message, float delay)
        {
            this.message = message;
            duration = delay;
        }
    }
}
public class MessageSendLater
{
    private float m_Duration;
    private IMessageListener m_MessageListener;
    private IMessage m_Message;

    public bool IsExpire;

    public MessageSendLater(IMessageListener listener,IMessage message,float delay)
    {
        m_MessageListener = listener;
        m_Duration = delay;
        m_Message = message;
    }
    public void Update(float elapsed_sec)
    {
        m_Duration -= elapsed_sec;
        if (m_Duration <= 0)
        {
            DistributeMessage();
            IsExpire = true;
        }
    }
    public void DistributeMessage()
    {
        m_MessageListener.HandleMessage(m_Message);
    }
}
