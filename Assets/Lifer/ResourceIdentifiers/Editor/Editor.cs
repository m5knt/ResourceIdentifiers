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
    [MenuItem("Lifer/ResourceIdentifiers/Update")]
    public static void Update() {
      new Tag().Update();
      new Layer().Update();
      new Scene().Update();
      new Resource().Update();
      new StreamingAssets().Update();
      AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
    }

    [MenuItem("Lifer/ResourceIdentifiers/Reset")]
    public static void Reset() {
      new Tag().Clear();
      new Layer().Clear();
      new Scene().Clear();
      new Resource().Clear();
      new StreamingAssets().Clear();
      Update();
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
