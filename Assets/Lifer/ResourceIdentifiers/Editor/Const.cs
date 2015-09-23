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
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

/*
 *
 */

namespace Lifer.ResourceIdentifiers {

	public static class Const {

		public static string Prefix = "Assets/Generate";
		public static Regex IdentifyRule = new Regex(@"[_A-z][_A-z0-9]+");
		public static Regex CamelRule = new Regex(@"[_A-Z][_A-z0-9]+");
		public static Regex TagRule = new Regex(@"[_A-z][_A-z0-9 \.\-]+");

		public static Regex ShortenRule = new Regex(@" +");
		public static Regex UnderRule = new Regex(@"[^A-z_0-9/]+");

		public static Dictionary<string, string> ExtentionType =
			new Dictionary<string, string>() {
			{"anim", "AnimationClip"},
			/**/
			{"wav", "AudioClip"},
			{"mp3", "AudioClip"},
			{"ogg", "AudioClip"},
			{"aif", "AudioClip"},
			{"aiff", "AudioClip"},
			{"xm", "AudioClip"},
			{"mod", "AudioClip"},
			{"it", "AudioClip"},
			{"s3m", "AudioClip"},
			/**/
			{"hdr", "Cubemap"},
			{"cubemap", "Cubemap"},
			/**/
			{"ttf", "Font"},
			{"otf", "Font"},
			{"dfont", "Font"},
			/**/
			{"prefab", "GameObject"},
			/**/
			{"mat", "Material"},
			{"material", "Material"},
			/**/
			{"fbx", "Mesh"},
			{"obj", "Mesh"},
			{"max", "Mesh"},
			{"blend", "Mesh"},
			/**/
			{"mov", "MovieTexture"},
			{"mpg", "MovieTexture"},
			{"mpeg", "MovieTexture"},
			{"mp4", "MovieTexture"},
			{"avi", "MovieTexture"},
			{"asf", "MovieTexture"},
			/**/
			{"physicmaterial", "PhysicMaterial"},
			/**/
			{"shader", "Shader"},
			/**/
			{"txt", "TextAsset"},
			{"html", "TextAsset"},
			{"htm", "TextAsset"},
			{"xml", "TextAsset"},
			{"bytes", "TextAsset"},
			{"json", "TextAsset"},
			{"csv", "TextAsset"},
			{"yaml", "TextAsset"},
			{"fnt", "TextAsset"},
			/**/
			{"exr", "Texture"},
			/**/
			{"psd", "Texture2D"},
			{"tif", "Texture2D"},
			{"tiff", "Texture2D"},
			{"jpg", "Texture2D"},
			{"tga", "Texture2D"},
			{"png", "Texture2D"},
			{"gif", "Texture2D"},
			{"bmp", "Texture2D"},
			{"iff", "Texture2D"},
			{"pict", "Texture2D"},
		};
	}
}
/*
 *
 */

