using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NemTextAlign {
  public class Matches : IEnumerable<MatchPair> {
    private List<MatchPair> _matches = new List<MatchPair>();

    public MatchPair this[int index] {
      get => _matches[index];
    }

    public int Count {
      get => _matches.Count;
    }

    public bool Add(MatchPair matchPair) {
      foreach (MatchPair pair in _matches) {
        if ((pair?.LeftRange?.Overlaps(matchPair?.LeftRange) ?? false) || (pair?.RightRange?.Overlaps(matchPair?.RightRange) ?? false)) {
          return false;
        }
      }

      _matches.Add(matchPair);

      return true;
    }

    public void AddRange(Matches matches) {
      foreach (MatchPair pair in matches._matches) {
        Add(pair);
      }
    }

    public bool ContainsLeftValue(int index) {
      foreach (MatchPair pair in _matches) {
        if (pair.LeftRange?.Contains(index) ?? false) {
          return true;
        }
      }
      return false;
    }
    public bool ContainsRightValue(int index) {
      foreach (MatchPair pair in _matches) {
        if (pair.RightRange?.Contains(index) ?? false) {
          return true;
        }
      }
      return false;
    }

    public bool Contains(MatchPair matchPair) {
      return _matches.Contains(matchPair);
    }

    public void Sort() {
      List<MatchPair> matches = (from pair in _matches
                                 where !(pair.LeftRange is null)
                                 orderby pair.LeftRange.Start
                                 select pair).ToList();

      IEnumerable<MatchPair> rights = from pair in _matches
                                      where pair.LeftRange is null
                                      orderby pair.RightRange.Start
                                      select pair;

      foreach(MatchPair right in rights) {
        int prevIndex = matches.FindIndex(pair => (pair.RightRange?.End ?? -1) == right.RightRange.Start - 1);
        
        matches.Insert(prevIndex + 1, right);
      }

      _matches = matches;
    }

    public IEnumerator<MatchPair> GetEnumerator() {
      return _matches.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Shift(int leftAdjustment, int rightAdjustment) {
      foreach (MatchPair pair in _matches) {
        pair.Shift(leftAdjustment, rightAdjustment);
      }
    }

  }
}
