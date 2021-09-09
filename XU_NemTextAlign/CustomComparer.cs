using System;
using System.Collections.Generic;
using System.Text;

namespace XU_NemTextAlign {
  public class CustomComparer : IComparer<string> {
    public int Compare(string x, string y) {
      return x.StartsWith(y[0]) ? 0 : x.CompareTo(y);
    }
  }
}
