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
using System.Xml;
using System.Xml.Linq;

/*
 *
 */

namespace ThunderEgg.ResourceIdentifiers {

    public class Serializer {
        /**/
        string FileName;
        public Dictionary<string, XElement> Elements;
        public List<string> AvailKeys;
        public List<string> MissingKeys;

        public Serializer(string fname) {
            FileName = fname;
        }

        public void Load() {
            try {
                Elements = null;
                AvailKeys = null;
                MissingKeys = null;
                if (!File.Exists(FileName)) {
                    return;
                }
                var xml = XElement.Load(FileName);
                Elements = xml.Elements("resource").
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
              new XElement("resources", Elements.Values)
            );
            xml.Save(FileName);
        }

        public void Update(IEnumerable<XElement> latests) {
            Elements = Elements ?? new Dictionary<string, XElement>();
            // 新規分
            var latest_keys = new HashSet<string>(latests.
              Select(e => e.Attribute("sym").Value).ToList());
            // 無効分更新
            MissingKeys = Elements.Keys.Where(
              key => !latest_keys.Contains(key)).ToList();
            foreach (var key in MissingKeys) {
                Elements[key].SetAttributeValue("idx", -1);
            }
            // 有効分更新
            AvailKeys = latest_keys.ToList();
            foreach (var e in latests) {
                var sym = e.Attribute("sym").Value;
                if (Elements.ContainsKey(sym)) {
                    Elements[sym] = e;
                }
                else {
                    Elements.Add(sym, e);
                }
            }
        }
    }
}

/*
 *
 */

