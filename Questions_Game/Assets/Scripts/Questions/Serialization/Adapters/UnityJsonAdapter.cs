using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PizzaPie.Questions.Serialization
{
    public class UnityJsonAdapter : IJsonAdapter
    {
        public object Deserialize(string json, Type type)
        {
            return JsonUtility.FromJson(json, type);
        }

        public T Deserialize<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public string Serialize(object obj)
        {
            return JsonUtility.ToJson(obj);
        }
    }
}
