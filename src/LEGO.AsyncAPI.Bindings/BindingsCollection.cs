// Copyright (c) The LEGO Group. All rights reserved.
namespace LEGO.AsyncAPI.Bindings
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Bindings.AMQP;
    using LEGO.AsyncAPI.Bindings.Http;
    using LEGO.AsyncAPI.Bindings.Kafka;
    using LEGO.AsyncAPI.Bindings.Pulsar;
    using LEGO.AsyncAPI.Bindings.Sns;
    using LEGO.AsyncAPI.Bindings.Sqs;
    using LEGO.AsyncAPI.Bindings.WebSockets;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.Interface;

    public static class BindingsCollection
    {
        public static TCollection Add<TCollection, TItem>(
    this TCollection destination,
    IEnumerable<TItem> source)
    where TCollection : ICollection<TItem>
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is List<TItem> list)
            {
                list.AddRange(source);
                return destination;
            }

            foreach (var item in source)
            {
                destination.Add(item);
            }

            return destination;
        }

        public static IEnumerable<IBindingParser<IBinding>> All => new List<IBindingParser<IBinding>>
        {
            Pulsar,
            Kafka,
            Http,
            Websockets,
            Sqs,
            Sns,
            AMQP,
        };

        public static IEnumerable<IBindingParser<IBinding>> Http => new List<IBindingParser<IBinding>>
        {
            new HttpOperationBinding(),
            new HttpMessageBinding(),
        };

        public static IEnumerable<IBindingParser<IBinding>> Websockets => new List<IBindingParser<IBinding>>
        {
            new WebSocketsChannelBinding(),
        };

        public static IEnumerable<IBindingParser<IBinding>> Kafka => new List<IBindingParser<IBinding>>
        {
            new KafkaServerBinding(),
            new KafkaChannelBinding(),
            new KafkaOperationBinding(),
            new KafkaMessageBinding(),
        };

        public static IEnumerable<IBindingParser<IBinding>> Pulsar => new List<IBindingParser<IBinding>>
        {
            new PulsarServerBinding(),
            new PulsarChannelBinding(),
        };

        public static IEnumerable<IBindingParser<IBinding>> Sqs => new List<IBindingParser<IBinding>>
        {
            new SqsChannelBinding(),
            new SqsOperationBinding(),
        };

        public static IEnumerable<IBindingParser<IBinding>> Sns => new List<IBindingParser<IBinding>>
        {
            new SnsChannelBinding(),
            new SnsOperationBinding(),
        };

        public static IEnumerable<IBindingParser<IBinding>> AMQP => new List<IBindingParser<IBinding>>
        {
            new AMQPChannelBinding(),
            new AMQPOperationBinding(),
            new AMQPMessageBinding(),
        };
    }
}
