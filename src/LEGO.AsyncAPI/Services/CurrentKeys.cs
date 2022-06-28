namespace LEGO.AsyncAPI.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;

    public class CurrentKeys
    {
        public string Channel { get; internal set; }
        public string Extension { get; internal set; }
        public string ServerVariable { get; internal set; }
        public string Anys { get; internal set; }
        public string Server { get; internal set; }
        public string Parameter { get; set; }
    }
}
