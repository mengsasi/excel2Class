using Game.Util;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Game.GameConfig {

    public class Sound1Config {
		[JsonParser( "_id" )]
		public string ID { get; set; }

		[JsonParser( "playOnStart" )]
		public bool PlayOnStart { get; set; }

		[JsonParser( "prefab" )]
		public string Prefab { get; set; }

		[JsonParser( "description" )]
		public string Description { get; set; }
    }

    public class Sound1Configs {

        private static Sound1Configs instance;
        public static Sound1Configs Instance {
            get { return instance ?? ( instance = new Sound1Configs() ); }
        }

        private Dictionary<string, Sound1Config> Sound1Dict = new Dictionary<string, Sound1Config>();

        public Sound1Configs() {
            var confJObj = ConfigManager.Instance.GetDownloadConfByName( "Sound1" );
            var arr = JArray.FromObject( confJObj );
            var count = arr.Count;
            for( int i = 0; i < count; i++ ) {
                var token = arr[i];
                var data = new Sound1Config();
                ParserHelper.ParseJsonAttributeProperity( data, token );
                Sound1Dict.Add( data.ID, data );
            }
        }

        public Sound1Config GetConfById(string id) {
            Sound1Config conf;
            if( Sound1Dict.TryGetValue( id, out conf ) )
                return conf;
            else
                return null;
        }
    }
}