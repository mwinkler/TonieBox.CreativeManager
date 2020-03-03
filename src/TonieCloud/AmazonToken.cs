using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TonieCloud
{
    public class AmazonToken
    { 
        public class Req
        {
            public string Url { get; set; }
            public Fields Fields { get; set; }
        }

        public class Fields
        {
            public string Key { get; set; }
            
            [JsonProperty("x-amz-algorithm")]
            public string AmazonAlgorithm { get; set; }
            
            [JsonProperty("x-amz-credential")]
            public string AmazonCredential { get; set; }
            
            [JsonProperty("x-amz-signature")]
            public string AmazonSignature { get; set; }
            
            [JsonProperty("x-amz-date")]
            public string AmazonDate { get; set; }
            
            public string Policy { get; set; }
        }

        public Req Request { get; set; }
        public string FileId { get; set; }
    }
}
