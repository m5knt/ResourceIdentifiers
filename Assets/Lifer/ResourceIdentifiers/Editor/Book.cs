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

	public class Book {
		/**/
		string FileName;
		public List<string> Alls;
		public List<string> Avails;
		public List<string> Missing;

		public Book(string fname) {
			FileName = fname;
		}

		public void Load() {
			try {
				if (!File.Exists(FileName)) {
					Alls = new List<string>();
					Avails = new List<string>();
					Missing = new List<string>();
					return;
				}
				var xml = XElement.Load(FileName);
				Alls = xml.Elements("resource").
					Select(e => e.Value).
					OrderBy(n => n).ToList();
				Avails = Alls.ToList();
				Missing = new List<string>();
			}
			catch (XmlException e) {
				Debug.LogError(e);
				Alls = new List<string>();
				Avails = new List<string>();
				Missing = new List<string>();
			}
		}

		public void Save() {
			var dir = Path.GetDirectoryName(FileName);
			Directory.CreateDirectory(dir);
			var xml = new XDocument(
				new XDeclaration("1.0", "utf-8", "true"),
				new XElement("resources",
					Alls.Select(e => new XElement("resource", e))
					)
			);
			xml.Save(FileName);
		}

		public void Update(IEnumerable<string> sets) {
			var t = new HashSet<string>(sets);
			Missing = Alls.Where(n => !t.Contains(n)).
				OrderBy(n => n).ToList();
			Avails = sets.OrderBy(n => n).ToList();
			Alls = sets.ToList();
			Alls.AddRange(Missing.ToList());
			Alls = Alls.OrderBy(n => n).ToList();
		}
	}
}

/*
 *
 */

