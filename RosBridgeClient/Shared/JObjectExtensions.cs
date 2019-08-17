﻿/*
© Siemens AG, 2017
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

<http://www.apache.org/licenses/LICENSE-2.0>.

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using Newtonsoft.Json.Linq;

namespace RosSharp.RosBridgeClient
{
    public static class JObjectExtensions
    {
        public static string GetOperation(this JObject jObject)
        {
            return jObject.GetValue("op").ToString();
        }
        public static string GetServiceId(this JObject jObject)
        {
            JToken jToken = jObject.GetValue("id");
            return (jToken == null ? "" : jToken.ToString());
        }
        public static string GetTopic(this JObject jObject)
        {
            return jObject.GetValue("topic").ToString();
        }
        public static string GetService(this JObject jObject)
        {
            return jObject.GetValue("service").ToString();
        }
        public static JObject GetValues(this JObject jObject)
        {
            return (JObject)jObject.GetValue("values");
        }
        public static JObject GetMessage(this JObject jObject)
        {
            return (JObject)jObject.GetValue("msg");
        }
        public static JObject GetArguments(this JObject jObject)
        {
            return (JObject)jObject.GetValue("args");
        }
    }
}
