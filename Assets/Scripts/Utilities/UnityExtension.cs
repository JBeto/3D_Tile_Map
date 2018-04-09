using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtension
{
	public static T GetSafeComponent<T>(GameObject gameObject)
    {
        T component =  gameObject.GetComponent<T>();
        if (component == null)
        {
            Debug.LogError("Expected to find component of type "
                + typeof(T) + " but found none", gameObject);
        }
        return component;
    }
}