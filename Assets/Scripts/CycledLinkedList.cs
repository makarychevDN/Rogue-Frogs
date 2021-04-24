using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycledLinkedList : IEnumerable
{
    private QueueNode m_HeadNode;
    
    public QueueNode HeadNode
    {
        get => m_HeadNode;
        set => m_HeadNode = value;
    }

    public CycledLinkedList()
    {
        m_HeadNode = null;
    }

    public CycledLinkedList(MapObject mapObject)
    {
        m_HeadNode = new QueueNode(mapObject);
        m_HeadNode.Next = m_HeadNode;
    }
    
    public void Remove(MapObject mapObject)
    {
        if (m_HeadNode == null)
        {
            return;
        }

        else
        {
            QueueNode temp = m_HeadNode;
            
            while (temp.Next.MapObject != mapObject)
            {
                temp = temp.Next;
            }

            temp.Next = temp.Next.Next;
        }
    }
    

    public void Add(MapObject mapObject)
    {
        if (m_HeadNode == null)
        {
            m_HeadNode = new QueueNode(mapObject);
            m_HeadNode.Next = m_HeadNode;
        }

        else
        {
            QueueNode temp = m_HeadNode;
            
            while (temp.Next != m_HeadNode)
            {
                temp = temp.Next;
            }

            temp.Next = new QueueNode(mapObject);
            temp.Next.Next = m_HeadNode;
        }
    }

    public void AddFirst(MapObject mapObject) //player always first
    {
        if (m_HeadNode == null)
        {
            m_HeadNode = new QueueNode(mapObject);
            m_HeadNode.Next = m_HeadNode;
        }

        else
        {
            QueueNode temp = m_HeadNode;
            m_HeadNode = new QueueNode(mapObject);
            m_HeadNode.Next = temp;

            temp = m_HeadNode;
            
            while (temp.Next != m_HeadNode)
            {
                temp = temp.Next;
            }

            temp.Next = new QueueNode(mapObject);
            temp.Next.Next = m_HeadNode;
        }
    }

    public void AddSecond(MapObject mapObject) //player always first
    {
        if (m_HeadNode == null)
        {
            m_HeadNode = new QueueNode(mapObject);
            m_HeadNode.Next = m_HeadNode;
        }

        else
        {
            var temp = m_HeadNode.Next;
            m_HeadNode.Next = new QueueNode(mapObject);
            m_HeadNode.Next.Next = temp;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
    
    public CycledQueueEnum GetEnumerator()
    {
        return new CycledQueueEnum(m_HeadNode);
    }
}

public class CycledQueueEnum : IEnumerator
{
    public QueueNode _head;
    public QueueNode _Current;
    public CycledQueueEnum(QueueNode node)
    {
        _head = node;
    }

    public bool MoveNext()
    {
        if (_head == null)
        {
            return false;
        }
        
        if (_Current == null)
        {
            _Current = _head;
            return true;
        }
        
        _Current = _Current.Next;
        return (_Current != _head);
    }

    public void Reset()
    {
        Current = _head;
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public QueueNode Current
    {
        get
        {
            return _Current;
        }

        set
        {
            _Current = value;
        }
    }
}

public class QueueNode
{
    private MapObject m_MapObject;
    private QueueNode m_Next;


    public MapObject MapObject
    {
        get => m_MapObject;
        set => m_MapObject = value;
    }

    public QueueNode Next
    {
        get => m_Next;
        set => m_Next = value;
    }

    public QueueNode(MapObject mapObject)
    {
        m_MapObject = mapObject;
    }
}