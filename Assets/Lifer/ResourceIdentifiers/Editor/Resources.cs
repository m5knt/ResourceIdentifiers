/**
 * @file
 * @author Yukio KANEDA
01234567890123456789012345678901234567890123456789012345678901234567890123456789
 */

/*
 *
 */

using UnityEditor;
using UnityEngine;

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

  public class Resource : Identifier {

    protected override string BaseName {
      get {
        return "Resources";
      }
    }

    protected override string NameSpace {
      get {
        return "R";
      }
    }

    const string source = "Assets/Resources/";

    public override void Update() {
      // xml 豎ｺ螳�
      var book = new Serialize(XML);
      book.Load();
      book.Update(Collect());
      book.Save();
      Generate(book);
    }

    protected override List<XElement> Collect() {
      var paths = AssetDatabase.GetAllAssetPaths().Where(n => {
        return !Directory.Exists(n) && n.StartsWith(source);
      });
      var i = -1;
      var elems = paths.Select(path => {
        ++i;
        var rel = path.Substring(source.Length);
        var fname = Path.GetFileName(rel);
        var fbase = Path.GetFileNameWithoutExtension(fname);
        var ext = Path.GetExtension(fname).Substring(1);
        var type = Const.ExtentionType.ContainsKey(ext) ?
          Const.ExtentionType[ext] : ext + "_bytes";
        var crumbs = rel.Split('/').ToList();
        crumbs.RemoveAt(crumbs.Count - 1);
        crumbs.Add(fbase);
        crumbs.Add(type);
        var syms = string.Join("/",
          crumbs.Select(c => ToIdentifier(c)).ToArray());
        var e = new XElement("resource");
        e.SetAttributeValue("idx", i);
        e.SetAttributeValue("sym", syms);
        e.SetAttributeValue("val", rel);
        e.Value = path;
        return e;
      }).ToList();
      return elems;
    }

    protected override void Generate(Serialize book) {
      //
      var avail = new Node<string, string>();
      book.Avail.Select(k => book.All[k]).ToList().ForEach(e => {
        var crumbs = e.Attribute("sym").Value.Split('/').ToList();
        var val = e.Attribute("val").Value;
        avail.Add(crumbs, val);
      });
      //
      var missing = new Node<string, string>();
      book.Missing.Select(k => book.All[k]).ToList().ForEach(e => {
        var crumbs = e.Attribute("sym").Value.Split('/').ToList();
        var val = e.Attribute("val").Value;
        missing.Add(crumbs, val);
      });
      using (var o = new StreamWriter(CS, false, Encoding.UTF8)) {
        o.Write(@"using System;
using System.Diagnostics;

namespace Lifer.Generate {{
public partial class {0} {{
", NameSpace);
        var prev = "";
        avail.Traverse(
          (k, v) => {
            o.WriteLine("public partial class {0} {{", k);
            prev = "";
          },
          (k, v) => {
            if (prev != k) {
              o.WriteLine("public const string _ = \"{0}\";", v);
            }
            o.WriteLine("public const string {0} = _;", k);
            prev = k;
          },
          (k, v) => {
            o.WriteLine("}");
            prev = "";
          }
        );
        o.WriteLine(@"}
}");
        // 辟｡蜉ｹ蛻�
        o.Write(@"
#if UNITY_EDITOR
namespace Lifer.Generate {{
public partial class {0} {{
", NameSpace);
        missing.Traverse(
          (k, v) => {
            o.WriteLine("public partial class {0} {{", k);
          },
          (k, v) => {
            o.Write(
@"[Obsolete(""[Lifer.ResourceIdentifiers] Missing"")]
public const string {0} = ""{1}"";
", k, v);
          },
          (k, v) => {
            o.WriteLine("}");
          }
        );
        o.Write(@"}
}
#endif
");
      }
    }
  }
}

/*
 *
 */
