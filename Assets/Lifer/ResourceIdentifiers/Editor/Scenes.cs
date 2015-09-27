/**
 * @file
 * @author Yukio KANEDA
01234567890123456789012345678901234567890123456789012345678901234567890123456789
 */

/*
 *
 */

using UnityEditor;

using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

/*
 *
 */

namespace Lifer.ResourceIdentifiers {

  public class Scene : FlatIdentifier {

    protected override string BaseName {
      get {
        return "Scene";
      }
    }

    protected override string NameSpace {
      get {
        return "S";
      }
    }

    protected override List<XElement> Collect() {
      var i = -1;
      var scenes = EditorBuildSettings.scenes.Where(n => n.enabled);
      var elems = scenes.Select(scene => {
        ++i;
        var val = Path.GetFileNameWithoutExtension(scene.path);
        var sym = ToIdentifier(val);
        var e = new XElement("resource");
        e.SetAttributeValue("idx", i);
        e.SetAttributeValue("sym", sym);
        e.SetAttributeValue("val", val);
        e.Value = scene.path;
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
