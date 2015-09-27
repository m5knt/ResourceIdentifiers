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

namespace Lifer.ResourceIdentifiers {

  public class Tag : FlatIdentifier {

    protected override string BaseName {
      get {
        return "Tag";
      }
    }

    protected override string NameSpace {
      get {
        return "T";
      }
    }

    protected override List<XElement> Collect() {
      var i = -1;
      var tags = InternalEditorUtility.tags;
      var elems = tags.Select(tag => {
        ++i;
        var sym = ToIdentifier(tag);
        var e = new XElement("resource");
        e.SetAttributeValue("idx", i);
        e.SetAttributeValue("sym", sym);
        e.SetAttributeValue("val", tag);
        e.Value = tag;
        return e;
      }).ToList();
      return elems;
    }

    protected override void Generate(TextWriter o, XElement e) {
      var sym = e.Attribute("sym").Value;
      var val = e.Attribute("val").Value;
      o.WriteLine(@"const string {0} = ""{1}"";", sym, val);
    }
  }
}

/*
 *
 */
