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

namespace ThunderEgg.ResourceIdentifiers {

    public class Resource : Identifier {

        protected override string BaseName {
            get {
                return "Resources";
            }
        }

        protected override string ClassName {
            get {
                return "R";
            }
        }

        const string source = "Assets/Resources/";

        public override void Update() {
            // xml 豎ｺ螳�
            var book = new Serializer(XML);
            book.Load();
            book.Update(Collect());
            book.Save();
            Generater(book);
        }

        protected override List<XElement> Collect() {
            var paths = AssetDatabase.GetAllAssetPaths().Where(n => {
                return !Directory.Exists(n) && n.StartsWith(source);
            });
            var temps = new Dictionary<string, XElement>();
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
                var sym = string.Join("/",
                  crumbs.Select(c => ToIdentifier(c)).ToArray());
                var e = new XElement("resource");
                e.SetAttributeValue("idx", i);
                e.SetAttributeValue("sym", sym);
                e.SetAttributeValue("val", rel);
                e.Value = path;
                SetAttributeDup(temps, sym, e);
                return e;
            }).ToList();
            return elems;
        }

        protected override void Generater(Serializer book) {
            //
            var avail = new Node<string, XElement>();
            book.AvailKeys.Select(k => book.Elements[k]).ToList().ForEach(e => {
                var sym = e.Attribute("sym").Value;
                var crumbs = sym.Split('/').ToList();
                avail.Update(crumbs, book.Elements[sym]);
            });
            //
            var missing = new Node<string, XElement>();
            book.MissingKeys.Select(k => book.Elements[k]).ToList().ForEach(e => {
                var sym = e.Attribute("sym").Value;
                var crumbs = sym.Split('/').ToList();
                missing.Update(crumbs, book.Elements[sym]);
            });
            using (var o = new StreamWriter(CS, false, Encoding.UTF8)) {
                o.Write(@"using System;
using System.Diagnostics;

namespace ThunderEgg.Generate {{
public partial class {0} {{
", ClassName);
                avail.Traverse(
                  (k, v) => {
                      o.WriteLine("public partial class {0} {{", k);
                  },
                  (k, v) => {
                      var val = v.Attribute("val").Value;
                      var dup = (int)v.Attribute("dup");
                      if (dup != 0) {
                          o.WriteLine(@"[Obsolete(""Duplicate"")]");
                      }
                      o.WriteLine("public const string {0} = \"{1}\";", k, val);
                  },
                  (k, v) => {
                      o.WriteLine("}");
                  }
                );
                o.WriteLine(@"}
}");
                // 辟｡蜉ｹ蛻�
                o.Write(@"
#if UNITY_EDITOR
namespace ThunderEgg.Generate {{
public partial class {0} {{
", ClassName);
                missing.Traverse(
                  (k, v) => {
                      o.WriteLine("public partial class {0} {{", k);
                  },
                  (k, v) => {
                      var val = v.Attribute("val").Value;
                      o.Write(
  @"[Obsolete(""Missing"")]
public const string {0} = ""{1}"";
", k, val);
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
