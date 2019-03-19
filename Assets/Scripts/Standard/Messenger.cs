using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MessengerMapManager {
    private static readonly List<IDictionary> maps = new List<IDictionary>();

    public static void Register(IDictionary map) {
        maps.Add(map);
    }

    public static void ClearAll() {
        foreach (IDictionary map in maps) {
            map.Clear();
        }
    }
}

public static class Messenger {
    private static readonly Dictionary<Message, List<Action>> map;

    static Messenger() {
        map = new Dictionary<Message, List<Action>>();
        MessengerMapManager.Register(map);
    }

    public static void AddHandler(Message message, Action handler) {
        List<Action> handlers;
        if (!map.TryGetValue(message, out handlers)) {
            handlers = new List<Action>();
            map.Add(message, handlers);
        }

        if (!handlers.Contains(handler)) {
            handlers.Add(handler);
        }
    }

    public static void RemoveHandler(Message message, Action handler) {
        List<Action> handlers;
        if (map.TryGetValue(message, out handlers)) {
            if (handlers.Contains(handler)) {
                handlers.Remove(handler);
            }
        }
    }

    public static void Clear() {
        map.Clear();
    }

    public static void SendMessage(Message message) {
        List<Action> handlers;
        if (map.TryGetValue(message, out handlers)) {
            foreach (Action handler in handlers) {
                handler();
            }
        }
    }
}

public static class Messenger<T> {
    private static readonly Dictionary<Message, List<Action<T>>> map;

    static Messenger() {
        map = new Dictionary<Message, List<Action<T>>>();
        MessengerMapManager.Register(map);
    }

    public static void AddHandler(Message message, Action<T> handler) {
        List<Action<T>> handlers;
        if (!map.TryGetValue(message, out handlers)) {
            handlers = new List<Action<T>>();
            map.Add(message, handlers);
        }

        if (!handlers.Contains(handler)) {
            handlers.Add(handler);
        }
    }

    public static void RemoveHandler(Message message, Action<T> handler) {
        List<Action<T>> handlers;
        if (map.TryGetValue(message, out handlers)) {
            if (handlers.Contains(handler)) {
                handlers.Remove(handler);
            }
        }
    }

    public static void Clear() {
        map.Clear();
    }

    public static void SendMessage(Message message, T param) {
        List<Action<T>> handlers;
        if (map.TryGetValue(message, out handlers)) {
            foreach (Action<T> handler in handlers) {
                handler(param);
            }
        }
    }
}

public enum Message {
    Null,
    AddScroller,
    GameOver,
    AddPoints,
    Restart,
    AllTilesRevealed,
    NewGame
}
