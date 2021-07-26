using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentlyActiveObjects
{
    public static HashSet<MonoBehaviour> activeObjects = new HashSet<MonoBehaviour>();

    public static void Add(MonoBehaviour something)
    {
        activeObjects.Add(something);
    }

    public static void Remove(MonoBehaviour something)
    {
        activeObjects.Remove(something);
    }

    public static bool SomethingIsActNow => activeObjects.Count != 0;
}
