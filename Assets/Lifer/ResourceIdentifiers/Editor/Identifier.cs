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

	public abstract class Identifier {

		protected abstract string BaseName {
			get;
		}

		protected abstract string NameSpace {
			get;
		}

		protected abstract List<string> All {
			get;
		}

		protected string CS {
			get {
				return string.Format("{0}/{1}.cs", Const.Prefix, BaseName);
			}
		}

		protected string XML {
			get {
				return string.Format("{0}/Editor/{1}.xml", Const.Prefix, BaseName);
			}
		}

		public virtual void ClearMissing() {
			if (File.Exists(CS)) File.Delete(CS);
			if (File.Exists(XML)) File.Delete(XML);
		}

		public abstract void Generate();
	}
}

/*
 *
 */
