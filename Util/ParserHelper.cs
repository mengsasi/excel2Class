using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Util {
    public class ParserHelper {

        public static void ParseJsonAttributeProperity(object instance, JToken json) {
            var proplist = instance.GetType().GetProperties();
            for( int i = 0; i < proplist.Length; i++ ) {
                var prop = proplist[i];
                var catbs = prop.GetCustomAttributes( typeof( JsonParserAttribute ), true );

                for( int j = 0; j < catbs.Length; j++ ) {
                    var attri = catbs[j] as JsonParserAttribute;
                    if( attri != null ) {
                        attri.ParseJson( json, prop, instance );
                    }
                }
            }
        }

        public static void ParseJsonAttributeProperityForChild(object instance, JToken childjson) {
            var proplist = instance.GetType().GetProperties();
            for( int i = 0; i < proplist.Length; i++ ) {
                var prop = proplist[i];
                var catbs = prop.GetCustomAttributes( false );
                var basetype = instance.GetType().BaseType;

                if( basetype.GetProperty( prop.Name ) == null ) {
                    for( int j = 0; j < catbs.Length; j++ ) {
                        var attri = catbs[j];
                        if( attri.GetType() == typeof( JsonParserAttribute ) ) {
                            var jpa = (JsonParserAttribute)attri;
                            jpa.ParseJson( childjson, prop, instance );
                        }
                    }
                }
            }
        }

        public static void ConfValueCheckAttributeProperity(string confName, string id, object instance) {
            var proplist = instance.GetType().GetProperties();
            for( int i = 0; i < proplist.Length; i++ ) {
                var prop = proplist[i];
                var catbs = prop.GetCustomAttributes( false );
                var basetype = instance.GetType().BaseType;

                if( basetype.GetProperty( prop.Name ) == null ) {
                    for( int j = 0; j < catbs.Length; j++ ) {
                        var attri = catbs[j];
                        if( attri.GetType() == typeof( ConfValueAttribute ) ) {
                            var dva = (ConfValueAttribute)attri;
                            var has = dva.Check( prop, instance );
                            if( !has ) {
                                var value = prop.GetValue( instance, null );
                                Debug.LogError( string.Format( "{0} id:{1} - {2}:{3} Error", confName, id, prop.Name, value ) );
                            }
                        }
                    }
                }
            }
        }
    }
}