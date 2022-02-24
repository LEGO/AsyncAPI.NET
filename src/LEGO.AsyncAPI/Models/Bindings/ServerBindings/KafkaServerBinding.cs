// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Bindings.ServerBindings
{
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;

    public class KafkaServerBinding : IServerBinding
    {
        public IDictionary<string, IAny> Extensions { get; set; }
    }
}