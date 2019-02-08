using Core;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameConfig {
    [ExecuteInEditMode]
    public class ConfigManager {

        private static ConfigManager instance;
        public static ConfigManager Instance {
            get { return instance ?? ( instance = new ConfigManager() ); }
        }

        private Dictionary<string, JArray> configDic = new Dictionary<string, JArray>();
		
		private int version;
		private JToken configData;

        public int Version {
            get {
                return version;
            }
        }

        public void SetConfData(int version, JToken json) {
            this.version = version;
            configData = json;
            //save()
        }

        JArray LoadArrConf(string name) {
            var confText = Resources.Load<TextAsset>( "Configs/" + name );
            var obj = JArray.Parse( confText.ToString() );
            return obj;
        }

        public JToken GetDownloadConfByName(string name) {
            return configData[name];
        }

        public JArray GetArrConfByName(string name) {
            JArray arr;
            if( configDic.TryGetValue( name, out arr ) == false ) {
                arr = LoadArrConf( name );
                configDic.Add( name, arr );
            }
            return arr;
        }

    }
}