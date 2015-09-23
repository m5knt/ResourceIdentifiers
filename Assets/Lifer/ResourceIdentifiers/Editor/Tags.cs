/**
 * @file
 * @author Yukio KANEDA
01234567890123456789012345678901234567890123456789012345678901234567890123456789
 */

/*
 *
 */

using UnityEditorInternal;

using System.Collections.Generic;
using System.Linq;

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

		protected override List<string> All {
			get {
				return InternalEditorUtility.tags.
					Where(n => Const.CamelRule.IsMatch(n)).ToList();
			}
		}
	}
}

/*
 *
 */
