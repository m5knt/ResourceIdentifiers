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
    //    public static Regex IdentifyRule = new Regex(@"[_A-z][_A-z0-9]+");
    //    public static Regex CamelRule = new Regex(@"[_A-Z][_A-z0-9]+");
    //    public static Regex TagRule = new Regex(@"[_A-z][_A-z0-9 \.\-]+");

    public static readonly Regex ToShort = new Regex(@" +");
    public static readonly Regex ToUnder = new Regex(@"[^A-z_0-9]+");
    public static readonly Regex IsStartWithNumber = new Regex(@"^[0-9]");
    public static readonly Regex ToUnderLeaveSlash = new Regex(@"[^A-z_0-9/]+");

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

    public static readonly HashSet<string> Reserves = new HashSet<string>() {
      "abstract",
      "add",
      "alias",
      "as",
      "ascending",
      "async",
      "await",
      "base",
      "bool",
      "break",
      "byte",
      "case",
      "catch",
      "char",
      "checked",
      "class",
      "const",
      "continue",
      "decimal",
      "default",
      "delegate",
      "descending",
      "do",
      "double",
      "dynamic",
      "else",
      "enum",
      "event",
      "explicit",
      "extern",
      "false",
      "finally",
      "fixed",
      "float",
      "for",
      "foreach",
      "from",
      "get",
      "global",
      "goto",
      "group",
      "if",
      "implicit",
      "in",
      "int",
      "interface",
      "internal",
      "into",
      "is",
      "join",
      "let",
      "lock",
      "long",
      "namespace",
      "new",
      "null",
      "object",
      "operator",
      "orderby",
      "out",
      "override",
      "params",
      "partial",
      "partial",
      "private",
      "protected",
      "public",
      "readonly",
      "ref",
      "remove",
      "return",
      "sbyte",
      "sealed",
      "select",
      "set",
      "short",
      "sizeof",
      "stackalloc",
      "static",
      "string",
      "struct",
      "switch",
      "this",
      "throw",
      "true",
      "try",
      "typeof",
      "uint",
      "ulong",
      "unchecked",
      "unsafe",
      "ushort",
      "using",
      "value",
      "var",
      "virtual",
      "void",
      "volatile",
      "where",
      "where",
      "while",
      "yield",
    };
  }
}
/*
 *
 */

