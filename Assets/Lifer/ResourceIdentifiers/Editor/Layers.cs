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

namespace Lifer.ResourceIdentifiers {

    public class Layer : FlatIdentifier {

        protected override string BaseName {
            get {
                return "Layer";
            }
        }

        protected override string NameSpace {
            get {
                return "L";
            }
        }

        protected override List<XElement> Collect() {
            var i = -1;
            var layers = InternalEditorUtility.layers;
            var elems = layers.Select(layer => {
                ++i;
                var sym = ToIdentifier(layer);
                var e = new XElement("resource");
                e.SetAttributeValue("idx", i);
                e.SetAttributeValue("sym", sym);
                e.SetAttributeValue("val", LayerMask.NameToLayer(layer));
                e.Value = layer;
                return e;
            }).ToList();
            return elems;
        }

        protected override void Generater(TextWriter o, XElement e) {
            var sym = e.Attribute("sym").Value;
            var val = e.Attribute("val").Value;
            o.WriteLine(@"const int {0} = {1};", sym, val);
        }
    }
}

/*
 *
 */
