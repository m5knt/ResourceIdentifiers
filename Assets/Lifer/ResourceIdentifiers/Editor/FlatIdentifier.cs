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
using UnityEditorInternal;

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

	public abstract class FlatIdentifier : Identifier {

		protected virtual string ToLabel(string name) {
			return name.Replace(" ", "");
		}

		public override void Generate() {
			// xml 決定
			var book = new Book(XML);
			book.Load();
			book.Update(All);
			book.Save();
			// 有効分
			using (var o = new StreamWriter(CS, false, Encoding.UTF8)) {
				o.Write(@"using System;
using System.Diagnostics;

namespace Lifer.Genarate {{
public partial class {0} {{
", NameSpace);
				book.Avails.ForEach(v => {
					var k = v;
					k = Const.ShortenRule.Replace(k, "");
					k = Const.UnderRule.Replace(k, "_");
					k = k.Replace('/', '_');
					o.WriteLine(@"const string {0} = ""{1}"";", k, v);
				});
				o.Write(@"}
}
");
				/**/
				o.Write(@"
#if UNITY_EDITOR
namespace Lifer.Genarate {{
public partial class {0} {{
", NameSpace);
				book.Missing.ForEach(v => {
					var k = v;
					k = Const.ShortenRule.Replace(k, "");
					k = Const.UnderRule.Replace(k, "_");
					k = k.Replace('/', '_');
					o.WriteLine(@"[Obsolete(""[Lifer.ResourceIdentifiers] Missing"")]
const string {0} = ""{1}"";", k, v);
				});
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
