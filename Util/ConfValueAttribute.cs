using System;
using System.Reflection;

namespace Game.Util {
    [AttributeUsage( AttributeTargets.Property )]
    public class ConfValueAttribute : Attribute {

        private string typename;

        public ConfValueAttribute(string type) {
            typename = type;
        }

        public bool Check(PropertyInfo prop, object instance) {
            //var value = prop.GetValue( instance, null );
            bool result = true;
            //switch( typename ) {
            //    case "icon":
            //        result = Constant.CheckHasIcon( (string)value );
            //        break;
            //    case "item":
            //        var itemDef = ItemDefs.Instance.GetDefByItemID( (string)value );
            //        if( itemDef == null ) {
            //            result = false;
            //        }
            //        break;
            //    case "prefab":
            //        var prefab = PrefabLoader.Instance.LoadPrefab( (string)value );
            //        if( prefab == null ) {
            //            result = false;
            //        }
            //        break;
            //}
            return result;
        }

    }
}