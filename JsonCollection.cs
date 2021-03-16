using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p4uUtilities
{
    //class JsonCollection
    //{
    //    [JsonProperty("@name")]
    //    public string Name { get; set; } = "BLANK";

    //    [JsonProperty("@desc")]
    //    public string Description { get; set; }

    //    [JsonProperty("@value")]
    //    public string Value { get; set; }

    //    [JsonProperty("@value_type")]
    //    public string ValueType { get; set; }

    //    [JsonProperty("@value_range")]
    //    public Array ValueRange { get; set; }

    //    //[JsonProperty("READ_ONLY")]
    //    //public string ReadOnly { get; set; } = "0";

    //    [JsonProperty("@IsBrowsable")]
    //    public bool IsBrowsable { get; set; } = true;
    //}

    public class JsonFieldsCollector
    {
        private /*readonly*/ Dictionary<string, JValue> fields;
        private JObject _jObj;

        public JsonFieldsCollector(JToken token)
        {
            _jObj = (JObject)(token.ToObject<JObject>().DeepClone());

            fields = new Dictionary<string, JValue>();
            CollectFields(token);
        }

        private void CollectFields(JToken jToken)
        {
            switch (jToken.Type)
            {
                case JTokenType.Object:
                    foreach (var child in jToken.Children<JProperty>())
                        CollectFields(child);
                    break;
                case JTokenType.Array:
                    foreach (var child in jToken.Children())
                        CollectFields(child);
                    break;
                case JTokenType.Property:
                    CollectFields(((JProperty)jToken).Value);
                    break;
                default:
                    fields.Add(jToken.Path, (JValue)jToken);
                    break;
            }
        }

        private JToken FindKey(JToken jToken, string sPath)
        {
            JToken retJToken = null;

            retJToken = jToken.SelectToken(sPath);

            return retJToken;
        }

        public bool IsExist(string sPath)
        {
            if (_jObj.SelectToken(sPath) == null)
                return false;
            else
                return true;
        }

        public void UpdateJson(JToken source, string path, JToken value)
        {
            UpdateJsonInternal(source, path.Split('.'), 0, value);
        }

        private void UpdateJsonInternal(JToken source, string[] path, int pathIndex, JToken value)
        {
            if (pathIndex == path.Length - 1)
            {
                if (source is JArray)
                {
                    ((JArray)source)[int.Parse(path[pathIndex])] = value;
                }
                else if (source is JObject)
                {
                    if (((JObject)source)[path[pathIndex]] == null)
                    {
                        ((JObject)source)[path[pathIndex]] = new JObject(new JProperty("@desc", (JValue)""), new JProperty("@value", value));
                        //((JObject)source)[path[pathIndex]] = new JObject(new JProperty("@value", value));
                        //((JObject)source)[path[pathIndex]].Add("@value", value);
                        //((JObject)source)[path[pathIndex]] = new JProperty("@value", value);

                        this.fields.Add(((JObject)source)[path[pathIndex]].Path + ".@desc", (JValue)"");
                        this.fields.Add(((JObject)source)[path[pathIndex]].Path + ".@value", (JValue)value);
                    }
                    else
                        ((JObject)source)[path[pathIndex]] = value;
                }
            }
            else if (source is JArray)
            {
                UpdateJsonInternal(((JArray)source)[int.Parse(path[pathIndex])], path, pathIndex + 1, value);
            }
            else if (source is JObject)
            {
                if (((JObject)source)[path[pathIndex]] == null)
                {
                    ((JObject)source)[path[pathIndex]] = new JObject(new JProperty("@IsBrowsable", false), new JProperty("@desc", (JValue)""));

                    this.fields.Add(((JObject)source)[path[pathIndex]].Path + ".@IsBrowsable", (JValue)false);
                    this.fields.Add(((JObject)source)[path[pathIndex]].Path + ".@desc", (JValue)"");

                    UpdateJsonInternal(((JObject)source)[path[pathIndex]], path, pathIndex + 1, value);
                    //File.WriteAllText(@"c:/Luli Sense/options_new.dat", ((JObject)source)[path[pathIndex]].ToString());
                }
                else
                    UpdateJsonInternal(((JObject)source)[path[pathIndex]], path, pathIndex + 1, value);
            }
        }

        public void SaveFile(string sPath)
        {
            File.WriteAllText(sPath, _jObj.ToString());
            //File.WriteAllText(@"c:/Luli Sense/options_new.dat", jobj.ToString());

            //using (StreamWriter file = File.CreateText(@"c:/Luli Sense/options_new.dat"))
            //{
            //    using (JsonTextWriter writer = new JsonTextWriter(file))
            //    {
            //        jobj.WriteTo(writer);
            //    }
            //}

            //using (StreamWriter file = File.CreateText(@"c:/Luli Sense/options_new.dat"))
            //{
            //    //JsonSerializer serializer = new JsonSerializer();
            //    //serializer.Serialize(file, movie);
            //    //file.Write(jobj.Root);
            //    file.WriteAllText(jobj.ToString());
            //}
        }

        public string GetValue(string sPath)
        {
            string sTarget = sPath + ".@value";

            if (this.fields.ContainsKey(sTarget))
                return this.fields[sTarget].ToString();
            else
                return "";
        }

        public void AddKey(string sPath, string sValue)
        {
            JToken jTarget;
            jTarget = this.FindKey(_jObj, sPath);

            if (jTarget == null)
            {
                this.UpdateJson(_jObj, sPath, sValue);
            }
            else
            {
                this.UpdateJson(_jObj, sPath + ".@value", sValue);
                this.fields[sPath + ".@value"].Value = sValue;
            }

        }

        public void DeleteKey(string sPath)
        {
            JToken jTarget;
            jTarget = this.FindKey(_jObj, sPath);

            if (jTarget != null)
                jTarget.Remove();
        }

        public IEnumerable<KeyValuePair<string, JValue>> GetAllFields() => fields;
    }
}
