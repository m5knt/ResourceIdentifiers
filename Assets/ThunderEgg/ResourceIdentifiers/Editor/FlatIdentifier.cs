/**
 * @file
 * @author Yukio KANEDA
01234567890123456789012345678901234567890123456789012345678901234567890123456789
 */

/*
 *
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;

/*
 *
 */

namespace ThunderEgg.ResourceIdentifiers {

    public abstract class FlatIdentifier : Identifier {

        public override void Update() {
            // xml 決定
            var book = new Serializer(XML);
            book.Load();
            book.Update(Collect());
            book.Save();
            Generater(book);
        }

        protected abstract void Generater(TextWriter o, XElement e);

        protected override void Generater(Serializer book) {
            var avail = book.AvailKeys.Select(k => book.Elements[k]).
                       OrderBy(e => (int)e.Attribute("idx")).ToList();
            var missing = book.MissingKeys.Select(k => book.Elements[k]).
                OrderBy(e => (int)e.Attribute("idx")).ToList();

            // 有効分
            using (var o = new StreamWriter(CS, false, Encoding.UTF8)) {
                o.Write(@"using System;
using System.Diagnostics;

namespace ThunderEgg.Genarate {{
public partial class {0} {{
", ClassName);
                avail.ForEach(e => Generater(o, e));
                o.Write(@"}
}
");
                /**/
                o.Write(@"
#if UNITY_EDITOR
namespace ThunderEgg.Genarate {{
public partial class {0} {{
", ClassName);
                missing.ForEach(e => {
                    o.WriteLine(@"[Obsolete(""Missing"")]");
                    Generater(o, e);
                });
                o.Write(@"}
}
#endif
");
            }
        }
    }
}

/*
 *
 */
