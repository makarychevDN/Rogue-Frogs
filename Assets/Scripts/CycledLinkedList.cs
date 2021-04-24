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
        InsertNodeInEmptyList(mapObject);
    }

    public void Add(MapObject mapObject)
    {
        if (m_HeadNode == null)
        {
            InsertNodeInEmptyList(mapObject);
        }

        else
        {
            AddToTheEndOfList(mapObject);
        }
    } 

    public void AddFirst(MapObject mapObject)
    {
        if (m_HeadNode == null)
        {
            InsertNodeInEmptyList(mapObject);
        }

        else
        {
            AddToTheEndOfList(mapObject);
            m_HeadNode = m_HeadNode.Previous;
        }
    }  

    public void AddSecond(MapObject mapObject)
    {
        if (m_HeadNode == null)
        {
            InsertNodeInEmptyList(mapObject);
        }

        else
        {
            InsertNode(new QueueNode(mapObject), m_HeadNode, m_HeadNode.Next);
        }
    }

    public void AddAfterTargetObject(MapObject newMapObject, MapObject TargetObject)
    {
        QueueNode temp = m_HeadNode;
            
        while (temp.MapObject != TargetObject)
        {
            temp = temp.Next;
        }

        InsertNode(new QueueNode(newMapObject), temp, temp.Next);
    }
    
    public void AddBeforeTargetObject(MapObject newMapObject, MapObject TargetObject)
    {
        QueueNode temp = m_HeadNode;
            
        while (temp.MapObject != TargetObject)
        {
            temp = temp.Next;
        }

        InsertNode(new QueueNode(newMapObject), temp.Previous, temp);
    }
    
    private void AddToTheEndOfList(MapObject mapObject)
    {
        QueueNode temp = m_HeadNode;
            
        while (temp.Next != m_HeadNode)
        {
            temp = temp.Next;
        }
        
        InsertNode(new QueueNode(mapObject), temp, m_HeadNode);
        
    } 

    private void InsertNode(QueueNode newNode, QueueNode previous, QueueNode next)
    {
        newNode.Next = next;
        newNode.Previous = previous;
        previous.Next = newNode;
        next.Previous = newNode;
    }

    private void InsertNodeInEmptyList(MapObject mapObject)
    {
        m_HeadNode = new QueueNode(mapObject);
        m_HeadNode.Next = m_HeadNode;
        m_HeadNode.Previous = m_HeadNode;
    }
    
    public void Remove(MapObject mapObject)
    {
        if (m_HeadNode == null)
            return;

        QueueNode temp = m_HeadNode;
        
        while (temp.MapObject != mapObject)
        {
            temp = temp.Next;
        }

        temp.Previous.Next = temp.Next;
        temp.Next.Previous = temp.Previous;
        
        if (m_HeadNode.MapObject == mapObject)
        {
            m_HeadNode = temp.Next;
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
    private QueueNode m_Previous;


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
    
    public QueueNode Previous
    {
        get => m_Previous;
        set => m_Previous = value;
    }

    public QueueNode(MapObject mapObject)
    {
        m_MapObject = mapObject;
    }
}