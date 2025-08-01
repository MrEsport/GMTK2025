using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using RNG = UnityEngine.Random;

public static class ListExtension
{
    /// <summary>
    /// Return a random element from the list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T Random<T>(this IList<T> list)
    {
        return list[RNG.Range(0, list.Count)];
    }

    /// <summary>
    /// Return a random index from the list [0...<paramref name="list"/>.Count - 1]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static int RandomIndex<T>(this IList<T> list)
    {
        return RNG.Range(0, list.Count);
    }

    /// <summary>
    /// Returns and removes from the list element at <paramref name="index"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static T PopAt<T>(this IList<T> list, int index)
    {
        T t = list[index];
        list.RemoveAt(index);
        return t;
    }

    /// <summary>
    /// Returns and Removes from the list its first element
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T PopFront<T>(this IList<T> list)
    {
        return list.PopAt(0);
    }

    /// <summary>
    /// Returns and removes from the list its last element
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T PopBack<T>(this IList<T> list)
    {
        return list.PopAt(list.Count - 1);
    }

    /// <summary>
    /// Returns if at least one element matches the <paramref name="predicate"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="predicate"></param>
    /// <param name="item">First matching element found, otherwise, the default <typeparamref name="T"/> value</param>
    /// <returns></returns>
    public static bool TryGetFirst<T>(this IList<T> list, Func<T, bool> predicate, out T item)
    {
        try
        {
            item = list.First(predicate);
            return true;
        }
        catch (InvalidOperationException)
        {
            item = default;
            return false;
        }
    }

    /// <summary>
    /// Returns a string printing out each element of the list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static string ContentToString<T>(this IList<T> list)
    {
        string str = list.ToString();
        if (list.Count <= 0) return str + ": EMPTY";
        
        for (int i = 0; i < list.Count; ++i)
        {
            str += i > 0 ? ", " : ": (";
            str += list[i].ToString();
        }
        return str + ")";
    }

    /// <summary>
    /// Returns a string printing out each element of the list through <paramref name="toStringFunc"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="toStringFunc">optional function used to write for each list element (default is <typeparamref name="T"/>.ToString())</param>
    /// <returns></returns>
    public static string ContentToString<T>(this IList<T> list, System.Func<T, string> toStringFunc)
    {
        if(toStringFunc == null) return list.ContentToString();

        string str = list.ToString();
        if (list.Count <= 0) return str + ": EMPTY";
        
        for (int i = 0; i < list.Count; ++i)
        {
            str += i > 0 ? ", " : ": (";
            str += toStringFunc.Invoke(list[i]);
        }
        return str + ")";
    }
}
