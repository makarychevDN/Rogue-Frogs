using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycledLinkedList : IEnumerable
{
    private QueueNode headNode;

    public QueueNode HeadNode
    {
        get => headNode;
        set => headNode = value;
    }

    #region Constructors
    public CycledLinkedList()
    {
        headNode = null;
    }

    public CycledLinkedList(MapObject mapObject)
    {
        InsertNodeInEmptyList(mapObject);
    }

    public CycledLinkedList(MapObject[] mapObjects)
    {
        foreach (var item in mapObjects)
        {
            Add(item);
        }
    }

    public CycledLinkedList(List<MapObject> mapObjects)
    {
        foreach (var item in mapObjects)
        {
            Add(item);
        }
    }
    #endregion

    #region AddMethods
    public void Add(MapObject mapObject)
    {
        if (headNode == null)
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
        if (headNode == null)
        {
            InsertNodeInEmptyList(mapObject);
        }

        else
        {
            AddToTheEndOfList(mapObject);
            headNode = headNode.Previous;
        }
    }  

    public void AddSecond(MapObject mapObject)
    {
        if (headNode == null)
        {
            InsertNodeInEmptyList(mapObject);
        }

        else
        {
            InsertNode(new QueueNode(mapObject), headNode, headNode.Next);
        }
    }

    public void AddAfterTargetObject(MapObject TargetObject, MapObject newMapObject)
    {
        QueueNode temp = headNode;
            
        while (temp.MapObject != TargetObject)
        {
            temp = temp.Next;
        }

        InsertNode(new QueueNode(newMapObject), temp, temp.Next);
    }
    
    public void AddBeforeTargetObject(MapObject TargetObject, MapObject newMapObject)
    {
        QueueNode temp = headNode;
            
        while (temp.MapObject != TargetObject)
        {
            temp = temp.Next;
        }

        InsertNode(new QueueNode(newMapObject), temp.Previous, temp);
    }
    
    private void AddToTheEndOfList(MapObject mapObject)
    {
        QueueNode temp = headNode;
            
        while (temp.Next != headNode)
        {
            temp = temp.Next;
        }
        
        InsertNode(new QueueNode(mapObject), temp, headNode);        
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
        headNode = new QueueNode(mapObject);
        headNode.Next = headNode;
        headNode.Previous = headNode;
    }
    #endregion

    #region RemoveMethods
    public void Remove(MapObject mapObject)
    {
        if (headNode == null)
            return;

        QueueNode temp = headNode;
        
        while (temp.MapObject != mapObject)
        {
            temp = temp.Next;
        }

        temp.Previous.Next = temp.Next;
        temp.Next.Previous = temp.Previous;
        
        if (headNode.MapObject == mapObject)
        {
            headNode = temp.Next;
        }
    }
    #endregion

    #region InterfaceImplementationStaff
    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
    
    public CycledQueueEnum GetEnumerator()
    {
        return new CycledQueueEnum(headNode);
    }
    #endregion
}

public class CycledQueueEnum : IEnumerator
{
    public QueueNode head;
    public QueueNode current;
    public CycledQueueEnum(QueueNode node)
    {
        head = node;
    }

    public bool MoveNext()
    {
        if (head == null)
        {
            return false;
        }
        
        if (current == null)
        {
            current = head;
            return true;
        }
        
        current = current.Next;
        return (current != head);
    }

    public void Reset()
    {
        Current = head;
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
            return current;
        }

        set
        {
            current = value;
        }
    }
}

public class QueueNode
{
    private MapObject mapObject;
    private QueueNode next;
    private QueueNode previous;


    public MapObject MapObject
    {
        get => mapObject;
        set => mapObject = value;
    }

    public QueueNode Next
    {
        get => next;
        set => next = value;
    }
    
    public QueueNode Previous
    {
        get => previous;
        set => previous = value;
    }

    public QueueNode(MapObject mapObject)
    {
        this.mapObject = mapObject;
    }
}