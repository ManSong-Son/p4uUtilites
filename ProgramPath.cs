using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p4uUtilities
{
    public class ProgramPath
    {
        private static ProgramPath _instance;
        private static readonly object locker = new object();
        private readonly string FilePath;
        private JsonFieldsCollector JsonFieldsCollector { get; }

        public static ProgramPath Instance()
        {
            lock (locker)
            {
                if (_instance == null)
                    _instance = new ProgramPath();

                return _instance;
            }
        }

        [JsonProperty("@ROOT")]
        public string Root { get; set; }

        [JsonProperty("@DATA")]
        public string Data { get; set; }

        [JsonProperty("@CAL")]
        public string Cal { get; set; }

        [JsonProperty("@LOG")]
        public string Log { get; set; }

        [JsonProperty("@PROJECT")]
        public string Project { get; set; }

        [JsonProperty("@IMAGE")]
        public string Image { get; set; }

        //[JsonProperty("@OUTOFDAYS")]
        //public string OutOfDaysForLog { get; set; }

        //[JsonProperty("@LOGLEVEL")]
        //public string LogLevel { get; set; }

        public ProgramPath()
        {
            string curPath = System.IO.Directory.GetCurrentDirectory() + "\\";
            FilePath = curPath + @"p4uPath.dat";
            this.Root = curPath;
            this.Data = curPath + @"data\";
            this.Cal = curPath + @"cal\";
            this.Log = curPath + @"log\";
            this.Project = curPath + @"projects\";
            DirectoryInfo dirInfo = new DirectoryInfo(@"D:\");
            if (dirInfo.Exists == true)
                this.Image = @"D:\_Images\";
            else
                this.Image = curPath + @"images\";

            if (File.Exists(this.FilePath))
            {
                StreamReader reader = new StreamReader(this.FilePath);
                string json = reader.ReadToEnd();
                reader.Close();

                JObject obj = JObject.Parse(json);
                JsonFieldsCollector = new JsonFieldsCollector(obj);

                if (JsonFieldsCollector.IsExist("ROOT") == true)
                    this.Root = JsonFieldsCollector.GetValue("ROOT");
                else
                    JsonFieldsCollector.AddKey("ROOT", this.Root);

                if (JsonFieldsCollector.IsExist("DATA") == true)
                    this.Data = JsonFieldsCollector.GetValue("DATA");
                else
                    JsonFieldsCollector.AddKey("DATA", this.Data);

                if (JsonFieldsCollector.IsExist("CAL") == true)
                    this.Cal = JsonFieldsCollector.GetValue("CAL");
                else
                    JsonFieldsCollector.AddKey("CAL", this.Cal);

                if (JsonFieldsCollector.IsExist("LOG") == true)
                    this.Log = JsonFieldsCollector.GetValue("LOG");
                else
                    JsonFieldsCollector.AddKey("LOG", this.Log);

                if (JsonFieldsCollector.IsExist("PROJECT") == true)
                    this.Project = JsonFieldsCollector.GetValue("PROJECT");
                else
                    JsonFieldsCollector.AddKey("PROJECT", this.Project);

                if (JsonFieldsCollector.IsExist("IMAGE") == true)
                    this.Image = JsonFieldsCollector.GetValue("IMAGE");
                else
                    JsonFieldsCollector.AddKey("IMAGE", this.Image);
            }
            else
            {
                JObject obj = new JObject();
                JsonFieldsCollector = new JsonFieldsCollector(obj);

                JsonFieldsCollector.AddKey("ROOT", this.Root);
                JsonFieldsCollector.AddKey("DATA", this.Data);
                JsonFieldsCollector.AddKey("CAL", this.Cal);
                JsonFieldsCollector.AddKey("LOG", this.Log);
                JsonFieldsCollector.AddKey("PROJECT", this.Project);
                JsonFieldsCollector.AddKey("IMAGE", this.Image);

                JsonFieldsCollector.SaveFile(this.FilePath);
            }

            // check folder
            if (!System.IO.Directory.Exists(this.Root))
                System.IO.Directory.CreateDirectory(this.Root);
            if (!System.IO.Directory.Exists(this.Data))
                System.IO.Directory.CreateDirectory(this.Data);
            if (!System.IO.Directory.Exists(this.Cal))
                System.IO.Directory.CreateDirectory(this.Cal);
            if (!System.IO.Directory.Exists(this.Log))
                System.IO.Directory.CreateDirectory(this.Log);
            if (!System.IO.Directory.Exists(this.Project))
                System.IO.Directory.CreateDirectory(this.Project);
            if (!System.IO.Directory.Exists(this.Image))
                System.IO.Directory.CreateDirectory(this.Image);
        }
    }

    public class PathInfo
    {
        public void Init()
        {
            ProgramPath.Instance();
        }

        public string Root
        {
            get
            {
                return ProgramPath.Instance().Root;
            }
        }

        public string Data
        {
            get
            {
                return ProgramPath.Instance().Data;
            }
        }

        public string Log
        {
            get
            {
                return ProgramPath.Instance().Log;
            }
        }

        public string Cal
        {
            get
            {
                return ProgramPath.Instance().Cal;
            }
        }

        public string Project
        {
            get
            {
                return ProgramPath.Instance().Project;
            }
        }

        public string Image
        {
            get
            {
                return ProgramPath.Instance().Image;
            }
        }
    }
}
