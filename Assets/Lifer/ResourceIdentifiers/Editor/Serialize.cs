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

  public class Serialize {
    /**/
    string FileName;
    public Dictionary<string, XElement> All;
    public List<string> Avail;
    public List<string> Missing;

    public Serialize(string fname) {
      FileName = fname;
    }

    public void Load() {
      try {
        All = null;
        Avail = null;
        Missing = null;
        if (!File.Exists(FileName)) {
          return;
        }
        var xml = XElement.Load(FileName);
        All = xml.Elements("resource").
          ToDictionary(e => e.Attribute("sym").Value);
      }
      catch (XmlException e) {
        Debug.LogError(e);
      }
    }

    public void Save() {
      var dir = Path.GetDirectoryName(FileName);
      Directory.CreateDirectory(dir);
      var xml = new XDocument(
        new XDeclaration("1.0", "utf-8", "true"),
        new XElement("resources", All.Values)
      );
      xml.Save(FileName);
    }

    public void Update(IEnumerable<XElement> updates) {
      // 新規分
      var update_uids = new HashSet<string>(updates.
        Select(e => e.Attribute("sym").Value).ToList());
      // 消失リスト
      if (All == null) {
        All = new Dictionary<string, XElement>();
      }
      Missing = All.Keys.Where(uid => !update_uids.Contains(uid)).ToList();
      foreach(var k in Missing) {
        All[k].SetAttributeValue("idx", -1);
      }
      // 有効リスト
      Avail = update_uids.ToList();
      // 更新
      foreach (var e in updates) {
        var sym = e.Attribute("sym").Value;
        if (All.ContainsKey(sym)) {
          All[sym] = e;
        }
        else {
          All.Add(sym, e);
        }
      }
    }
  }
}

/*
 *
 */

