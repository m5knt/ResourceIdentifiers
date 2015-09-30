/**
 * @file
 * @author Yukio KANEDA
01234567890123456789012345678901234567890123456789012345678901234567890123456789
 */

using UnityEditor;
using UnityEngine;

/*
 *
 */

namespace ThunderEgg.ResourceIdentifiers {

    public static class Editor {

        const string Menu = "ThunderEgg/ResourceIdentifiers";

        [MenuItem(Menu + "/Update")]
        static void Update() {
            DoUpdate();
            Debug.Log("[ResourceIdentifiers] Updated");
        }

        [MenuItem(Menu + "/Reset")]
        static void Reset() {
            new Tag().Clear();
            new Layer().Clear();
            new Scene().Clear();
            new Resource().Clear();
            new StreamingAssets().Clear();
            DoUpdate();
            Debug.Log("[ResourceIdentifiers] Reset");
        }

        static void DoUpdate() {
            new Tag().Update();
            new Layer().Update();
            new Scene().Update();
            new Resource().Update();
            new StreamingAssets().Update();
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
        }
    }
#if false
    public class AssetPostprocessor_ : AssetPostprocessor {
        static void OnPostprocessAllAssets(string[] imported, string[] deleted,
        string[] moved, string[] movedfrom) {
            //Scan.Resources();
        }
		foreach (var str in imported) {
			Debug.Log("Reimported Asset: " + str);
		}
		foreach (var str in deleted) {
			Debug.Log("Deleted Asset: " + str);
		}
		for (var i = 0; i < moved.Length; i++) {
			Debug.Log("Moved Asset: " + moved[i] + " from: " + movedfrom[i]);
		}
}
#endif
}

/*
 *
 */
