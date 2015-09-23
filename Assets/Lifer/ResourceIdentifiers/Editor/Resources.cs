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
				var ext = Path.GetExtension(path);
				var s = source.Length;
				var valid = path.Substring(s, path.Length - s - ext.Length);
				var p = valid;
				p = Const.ShortenRule.Replace(p, "");
				p = Const.UnderRule.Replace(p, "_");
				//
				var crumbs = p.Split('/').Where(n => n != "").ToList();
				string type;
				if (!Const.ExtentionType.TryGetValue(ext.Substring(1), out type)) {
					return;
				}
				crumbs.Add(type);
				// 識別子化可能かチェック
				if (!crumbs.All(n => Const.CamelRule.IsMatch(n))) return;
				tree.Add(crumbs, valid);
			});

			// 有効分
			var avile = new Node<string, string>();
			foreach (var path in book.Avails) treemaker(avile, path);
			// 無効分
			var missing = new Node<string, string>();
			foreach (var path in book.Missing) treemaker(missing, path);
			// 出力
			var prev = "";
			using (var o = new StreamWriter(CS, false, Encoding.UTF8)) {
				// 有効分
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
						if (prev != v) {
							o.WriteLine("public const string _ = \"{0}\";", v);
						}
						o.WriteLine("public const string {0} = _;", k);
						prev = v;
					},
					(k, v) => {
						o.WriteLine("}");
					}
				);
				o.WriteLine(@"}
}");
				// 無効分
				o.Write(@"
#if UNITY_EDITOR
namespace Lifer.Generate {{
public partial class {0} {{
", NameSpace);
				prev = "";
				missing.Traverse(
					(k, v) => {
						o.WriteLine("public partial class {0} {{", k);
					},
					(k, v) => {
						if (prev != v) {
							o.Write(@"[Obsolete(""[Lifer.ResourceIdentifiers] Missing"")]
public const string _ = ""{0}"";
", v);
						}
						o.Write(@"[Obsolete(""[Lifer.ResourceIdentifiers] Missing"")]
public const string {0} = _;
", k);
						prev = v;
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
			AssetDatabase.ImportAsset(CS);
		}
	}
}

/*
 *
 */
