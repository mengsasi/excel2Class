using Newtonsoft.Json.Linq;
using System;
using System.Reflection;
using UnityEngine;

namespace Game.Util {
    [AttributeUsage( AttributeTargets.Property )]
    public class JsonParserAttribute : Attribute {

        private string tokenPath;
        private string methodName;
        private bool containSubPath;
        private string[] subPathes;

        private bool isTranslate;

        public JsonParserAttribute(string path, string method = "", bool isTranslate = false) {
            tokenPath = path;
            methodName = method;
            if( !string.IsNullOrEmpty( tokenPath ) ) {
                containSubPath = tokenPath.Contains( "." );
                if( containSubPath ) {
                    subPathes = tokenPath.Split( '.' );
                }
            }
            this.isTranslate = isTranslate;
        }

        private const string translateDefStringFormat = "{0}:{1}:{2}";

        public void ParseJson(JToken json, PropertyInfo prop, object instance) {
            if( isTranslate ) {
                //try {
                //    string transString = Translate.Instance.GetText( string.Format( translateDefStringFormat, subPathes[0], subPathes[1], json.Value<string>( "_id" ) ) ); ;
                //    prop.SetValue( instance, transString, null );
                //}
                //catch( Exception ) {
                //    Debug.LogWarning( "ParseJson Load Translate PropError: Key:" + tokenPath );
                //    prop.SetValue( instance, "", null );
                //}
            }
            else {
                if( !string.IsNullOrEmpty( methodName ) ) {
                    var instanceType = instance.GetType();
                    var method = instanceType.GetMethod( methodName );
                    var parseResult = method.Invoke( instance, new object[] { json } );
                    prop.SetValue( instance, parseResult, null );
                }
                else {
                    if( string.IsNullOrEmpty( tokenPath ) ) {
                        try {
                            JToken token = json;
                            prop.SetValue( instance, token.ToObject( prop.PropertyType ), null );
                        }
                        catch( Exception ) {
                        }
                    }
                    else {
                        try {
                            JToken token = json;
                            if( containSubPath ) {
                                foreach( var subPath in subPathes ) {
                                    token = token[subPath];
                                    if( token == null ) {
                                        break;
                                    }
                                }
                            }
                            else {
                                token = token[tokenPath];
                            }

                            if( token == null || token.IsNullOrEmpty() ) {
                                prop.SetValue( instance, prop.PropertyType.GetDefaultValue(), null );
                            }
                            else {
                                object value;
                                if( prop.PropertyType == typeof( string ) ) {
                                    value = token.ToString();
                                }
                                else {
                                    value = token.ToObject( prop.PropertyType );
                                }
                                prop.SetValue( instance, value, null );
                            }
                        }
                        catch( Exception ex ) {
                            Debug.LogError( string.Format( "ERROR PARSE JSON {0} AT PATH {1}, ERROR {2}",
                                json.ToString( Newtonsoft.Json.Formatting.None ), tokenPath, ex.Message ) );
                        }
                    }
                }
            }
        }
    }
}