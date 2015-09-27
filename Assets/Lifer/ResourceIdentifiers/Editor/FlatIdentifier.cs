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

namespace Lifer.ResourceIdentifiers {

  public abstract class FlatIdentifier : Identifier {

    public override void Update() {
      // xml 決定
      var book = new Serialize(XML);
      book.Load();
      book.Update(Collect());
      book.Save();
      Generate(book);
    }

    protected abstract void Generate(TextWriter o, XElement e);

    protected override void Generate(Serialize book) {
      var avail = book.Avail.Select(k => book.All[k]).
                 OrderBy(e => int.Parse(e.Attribute("idx").Value)).ToList();
      var missing = book.Missing.Select(k => book.All[k]).
          OrderBy(e => int.Parse(e.Attribute("idx").Value)).ToList();

      // 有効分
      using (var o = new StreamWriter(CS, false, Encoding.UTF8)) {
        o.Write(@"using System;
using System.Diagnostics;

namespace Lifer.Genarate {{
public partial class {0} {{
", NameSpace);
        avail.ForEach(e => Generate(o, e));
        o.Write(@"}
}
");
        /**/
        o.Write(@"
#if UNITY_EDITOR
namespace Lifer.Genarate {{
public partial class {0} {{
", NameSpace);
        missing.ForEach(e => {
          o.WriteLine(@"[Obsolete(""[Lifer.ResourceIdentifiers] Missing"")]");
          Generate(o, e);
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
