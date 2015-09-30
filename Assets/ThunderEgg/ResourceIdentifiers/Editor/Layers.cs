/**
 * @file
 * @author Yukio KANEDA
01234567890123456789012345678901234567890123456789012345678901234567890123456789
 */

/*
 *
 */

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngineInternal;

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;

/*
 *
 */

namespace ThunderEgg.ResourceIdentifiers {

    public class Layer : FlatIdentifier {

        protected override string BaseName {
            get {
                return "Layer";
            }
        }

        protected override string ClassName {
            get {
                return "L";
            }
        }

        protected override List<XElement> Collect() {
            var i = -1;
            var layers = InternalEditorUtility.layers;
            var temps = new Dictionary<string, XElement>();
            var elems = layers.Select(layer => {
                ++i;
                var sym = ToIdentifier(layer);
                var e = new XElement("resource");
                e.SetAttributeValue("idx", i);
                e.SetAttributeValue("sym", sym);
                e.SetAttributeValue("val", LayerMask.NameToLayer(layer));
                e.Value = layer;
                SetAttributeDup(temps, sym, e);
                return e;
            }).ToList();
            return elems;
        }

        protected override void Generater(TextWriter o, XElement e) {
            var sym = e.Attribute("sym").Value;
            var val = e.Attribute("val").Value;
            var dup = (int)e.Attribute("dup");
            if (dup != 0) {
                o.WriteLine(@"[Obsolete(""Duplicate"")]");
            }
            o.WriteLine(@"const int {0} = {1};", sym, val);
        }
    }
}

/*
 *
 */
