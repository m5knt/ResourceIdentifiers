/**
 * @file
 * @author Yukio KANEDA
01234567890123456789012345678901234567890123456789012345678901234567890123456789
 */

/*
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;

/*
 *
 */

namespace Lifer.ResourceIdentifiers {

    public class Node<K, V>
      where K : IEquatable<K> {
        /**/
        K Key;
        V Value;
        List<Node<K, V>> Child = new List<Node<K, V>>();

        public bool ContainsKey(List<K> keys) {
            var cur = this;
            foreach (var key in keys) {
                var next = cur.Child.FirstOrDefault(t => t.Key.Equals(key));
                if (next == null) {
                    return false;
                }
                cur = next;
            }
            return true;
        }

        public void Add(List<K> keys, V value) {
            var cur = this;
            foreach (var key in keys) {
                var next = cur.Child.FirstOrDefault(t => t.Key.Equals(key));
                if (next == null) {
                    next = new Node<K, V>() { Key = key };
                    cur.Child.Add(next);
                }
                cur = next;
            }
            if (cur.Value != null) {
                throw new ArgumentException();
            }
            cur.Value = value;
        }

        public void Traverse(Action<K, V> down, Action<K, V> term,
          Action<K, V> up) {
            // 枝数・名前順
            var e = Child.OrderByDescending(n => n.Child.Count).
              ThenBy(n => n.Key);
            foreach (var t in e) {
                if (t.Child.Count == 0) {
                    term(t.Key, t.Value);
                }
                else {
                    down(t.Key, t.Value);
                    t.Traverse(down, term, up);
                    up(t.Key, t.Value);
                }
            }
        }
    }
}

/*
 *
 */
