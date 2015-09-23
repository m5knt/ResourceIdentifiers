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

/*
 *
 */

namespace Lifer.ResourceIdentifiers {

	public class StreamingAssets : Identifier {

		protected override string BaseName {
			get {
				return "StreamingAssets";
			}
		}

		protected override string NameSpace {
			get {
				return "SA";
			}
		}

		const string source = "Assets/StreamingAssets/";

		protected override List<string> All {
			get {
				var alls = AssetDatabase.GetAllAssetPaths();
				var currents = alls.Where(n => {
					return !Directory.Exists(n) && n.StartsWith(source);
				});
				return currents.ToList();
			}
		}

		public override void Generate() {
			// xml 決定
			var book = new Book(XML);
			book.Load();
			book.Update(All);
			book.Save();
			// 枝作り処理定義
			var treemaker = new Action<Node<string, string>, string>((tree, path) => {
				var valid = path.Substring(source.Count());
				var p = valid;
				p = Const.ShortenRule.Replace(p, "");
				p = Const.UnderRule.Replace(p, "_");
				var crumbs = p.Split('/').Where(n => n != "").ToList();
				// 識別子化可能かチェック
				if (!crumbs.All(n => Const.IdentifyRule.IsMatch(n))) return;
				tree.Add(crumbs, valid);
			});
			// 有効分
			var avile = new Node<string, string>();
			foreach (var path in book.Avails) treemaker(avile, path);
			using (var o = new StreamWriter(CS, false, Encoding.UTF8)) {
				o.Write(@"using System;
using System.Diagnostics;

namespace Lifer.Generate {{
public partial class {0} {{
", NameSpace);
				avile.Traverse(
					(k, v) => {
						o.WriteLine("public partial class {0} {{", k);
					},
					(k, v) => {
						o.WriteLine("public const string {0} = \"{1}\";", k, v);
					},
					(k, v) => {
						o.WriteLine("}");
					}
				);
				o.WriteLine(@"}
}");
			}
			// 無効分
			var missing = new Node<string, string>();
			foreach (var path in book.Missing) treemaker(missing, path);
			using (var o = new StreamWriter(CS, true, Encoding.UTF8)) {
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
