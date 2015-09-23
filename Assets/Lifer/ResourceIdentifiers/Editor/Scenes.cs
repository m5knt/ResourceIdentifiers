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

		protected override List<string> All {
			get {
				return EditorBuildSettings.scenes.Where(n => {
					if (!n.enabled) return false;
					var fname = Path.GetFileNameWithoutExtension(n.path);
					return Const.CamelRule.IsMatch(fname);
				}).Select(n => n.path).ToList();
			}
		}
	}
}

/*
 *
 */
