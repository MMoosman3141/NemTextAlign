#define USE_DICTIONARY

using System.Collections.Generic;

namespace NemTextAlign.DifferenceEngine {
  internal class DiffStateList {
    private readonly Dictionary<int, DiffState> _table;

    public DiffStateList(int destCount) {
      _table = new Dictionary<int, DiffState>(destCount);
    }

    public DiffState GetByIndex(int index) {
      if(!_table.ContainsKey(index)) {
        _table.Add(index, new DiffState());
      }

      return _table[index];
    }
  }
}
