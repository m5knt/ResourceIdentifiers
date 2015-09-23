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

/*
 *
 */

namespace Lifer.ResourceIdentifiers {

	public class Editor {
		[MenuItem("Lifer/ResourceIdentifiers/Generate")]
		public static void Generate() {
			new Scene().Generate();
			new Tag().Generate();
			new Layer().Generate();
			new Resource().Generate();
			new StreamingAssets().Generate();
			AssetDatabase.Refresh();// (Const.Prefix, ImportAssetOptions.ImportRecursive);
		}

		[MenuItem("Lifer/ResourceIdentifiers/Clear Missing")]
		public static void ClearMissing() {
			new Scene().ClearMissing();
			new Tag().ClearMissing();
			new Layer().ClearMissing();
			new Resource().ClearMissing();
			new StreamingAssets().ClearMissing();
			Generate();
		}
	}

	public class AssetPostprocessor_ : AssetPostprocessor {
		static void OnPostprocessAllAssets(string[] imported, string[] deleted,
		string[] moved, string[] movedfrom) {
			//Scan.Resources();
		}
#if false
		foreach (var str in imported) {
			Debug.Log("Reimported Asset: " + str);
		}
		foreach (var str in deleted) {
			Debug.Log("Deleted Asset: " + str);
		}
		for (var i = 0; i < moved.Length; i++) {
			Debug.Log("Moved Asset: " + moved[i] + " from: " + movedfrom[i]);
		}
#endif
	}
}

/*
 *
 */
