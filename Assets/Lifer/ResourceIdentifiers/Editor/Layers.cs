/**
 * @file
 * @author Yukio KANEDA
01234567890123456789012345678901234567890123456789012345678901234567890123456789
 */

/*
 *
 */

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngineInternal;

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

	public class Layer : FlatIdentifier {

		protected override string BaseName {
			get {
				return "Layer";
			}
		}

		protected override string NameSpace {
			get {
				return "L";
			}
		}

		protected override List<string> All {
			get {
				return InternalEditorUtility.layers.
					Where(n => Const.CamelRule.IsMatch(n)).ToList();
			}
		}

		protected override string ToLabel(string name) {
			return name.Replace(" ", "");
		}
	}
}

/*
 *
 */
