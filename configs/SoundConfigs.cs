using Game.Util;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Game.GameConfig {

    public class SoundConfig {
		[JsonParser( "_id" )]
		public string ID { get; set; }

		[JsonParser( "playOnStart" )]
		public bool PlayOnStart { get; set; }

		[JsonParser( "loop" )]
		public bool Loop { get; set; }

		[JsonParser( "track" )]
		public int Track { get; set; }

		[JsonParser( "prefab" )]
		public string Prefab { get; set; }

		[JsonParser( "description" )]
		public string Description { get; set; }
    }

    public class SoundConfigs {

        private static SoundConfigs instance;
        public static SoundConfigs Instance {
            get { return instance ?? ( instance = new SoundConfigs() ); }
        }

        private Dictionary<string, SoundConfig> SoundDict = new Dictionary<string, SoundConfig>();

        public SoundConfigs() {
            var confJObj = ConfigManager.Instance.GetDownloadConfByName( "Sound" );
            var arr = JArray.FromObject( confJObj );
            var count = arr.Count;
            for( int i = 0; i < count; i++ ) {
                var token = arr[i];
                var data = new SoundConfig();
                ParserHelper.ParseJsonAttributeProperity( data, token );
                SoundDict.Add( data.ID, data );
            }
        }

        public SoundConfig GetConfById(string id) {
            SoundConfig conf;
            if( SoundDict.TryGetValue( id, out conf ) )
                return conf;
            else
                return null;
        }
    }
}