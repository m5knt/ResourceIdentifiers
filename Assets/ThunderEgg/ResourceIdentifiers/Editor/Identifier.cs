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
using System.Xml.Linq;
using System.Linq;

/*
 *
 */

namespace ThunderEgg.ResourceIdentifiers {

    public abstract class Identifier {

        protected abstract string BaseName {
            get;
        }

        protected abstract string ClassName {
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

        protected string ToIdentifier(string str) {
            var t = str;
            t = Const.ToSpace.Replace(t, "");
            t = Const.ToUnder.Replace(t, "_");
            if (Const.IsStartWithNumber.IsMatch(t) ||
              Const.ReserveIdentifiers.Contains(t)) {
                t = "_" + t;
            }
            return t;
        }

        public virtual void Clear() {
            if (File.Exists(CS)) {
                File.Delete(CS);
            }
            if (File.Exists(XML)) {
                File.Delete(XML);
            }
        }

        protected void SetAttributeDup(Dictionary<string, XElement> temps,
            string sym, XElement e) //
        {
            XElement hit;
            if (!temps.TryGetValue(sym, out hit)) {
                temps.Add(sym, e);
                e.SetAttributeValue("dup", "0");
            }
            else {
                hit.SetAttributeValue("dup", "1");
                e.SetAttributeValue("dup", "1");
            }
        }

        public abstract void Update();

        protected abstract List<XElement> Collect();

        protected abstract void Generater(Serializer book);

    }
}

/*
 *
 */
