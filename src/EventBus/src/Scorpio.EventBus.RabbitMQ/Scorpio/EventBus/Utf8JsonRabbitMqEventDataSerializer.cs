﻿using System;
using System.Collections.Generic;
using System.Text;

using EasyNetQ;

using Scorpio.DependencyInjection;

namespace Scorpio.EventBus
{
    internal class Utf8JsonRabbitMqEventDataSerializer : IRabbitMqEventDataSerializer, ISingletonDependency
    {
        private readonly ISerializer _serializer;

        public Utf8JsonRabbitMqEventDataSerializer(ISerializer serializer)
        {
            _serializer = serializer;
        }
        public object Deserialize(byte[] value, Type type) => _serializer.BytesToMessage(type,value);
        public T Deserialize<T>(byte[] value) => (T)_serializer.BytesToMessage(typeof(T),value);
        public byte[] Serialize(object obj) => _serializer.MessageToBytes(obj.GetType(),obj);
    }
}
