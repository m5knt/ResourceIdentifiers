/**
 * @file
 * @author Yukio KANEDA
01234567890123456789012345678901234567890123456789012345678901234567890123456789
 */

/*
 *
 */

using UnityEditorInternal;

using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

/*
 *
 */

namespace ThunderEgg.ResourceIdentifiers {

    public class Tag : FlatIdentifier {

        protected override string BaseName {
            get {
                return "Tag";
            }
        }

        protected override string ClassName {
            get {
                return "T";
            }
        }

        protected override List<XElement> Collect() {
            var i = -1;
            var tags = InternalEditorUtility.tags;
            var temps = new Dictionary<string, XElement>();
            var elems = tags.Select(tag => {
                ++i;
                var sym = ToIdentifier(tag);
                var e = new XElement("resource");
                e.SetAttributeValue("idx", i);
                e.SetAttributeValue("sym", sym);
                e.SetAttributeValue("val", tag);
                e.Value = tag;
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
            o.WriteLine(@"const string {0} = ""{1}"";", sym, val);
        }
    }
}

/*
 *
 */
