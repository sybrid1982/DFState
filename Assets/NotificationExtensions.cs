using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Handler = System.Action<System.Object, System.Object>;

public static class NotificationExtensions {
    public static void PostNotification (this object obj, string notificationName)
    {
        NotificationCenter.instance.PostNotification(notificationName, obj);
    }
    public static void PostNotification (this object obj, string notificationname, object e)
    {
        NotificationCenter.instance.PostNotification(notificationname, obj, e);
    }
    public static void AddObserver (this object obj, Handler handler, string notificationName)
    {
        NotificationCenter.instance.AddObserver(handler, notificationName);
    }
    public static void AddObserver (this object obj, Handler handler, string notificationName, object sender)
    {
        NotificationCenter.instance.AddObserver(handler, notificationName, sender);
    }
    public static void RemoveObserver (this object obj, Handler handler, string notificationName)
    {
        NotificationCenter.instance.RemoveObserver(handler, notificationName);
    }
    public static void RemoveObserver(this object obj, Handler handler, string notificationName, System.Object sender)
    {
        NotificationCenter.instance.RemoveObserver(handler, notificationName, sender);
    }
}
