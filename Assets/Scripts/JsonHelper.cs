using System;
using UnityEngine;

public static class JsonHelper
{
    [Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }

    public static T[] FromJson<T>(string json)
    {
        string newJson = "{\"items\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.items;
    }
}
