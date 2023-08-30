using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace krak.Api
{
    class APIRequest
    {
        [JsonProperty("op")]
        public Opcode opcode { get; set; }

        [JsonProperty("d")]
        public dynamic Data { get; set; }

        [JsonProperty("channel")]
        public ulong Channel { get; set; }

    }
}
