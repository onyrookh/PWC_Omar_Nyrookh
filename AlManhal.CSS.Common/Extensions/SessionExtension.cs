﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace System
{
    public static class SessionExtension
    {
        public static void SetParameter<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetParameter<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value);
        }
    }
}
