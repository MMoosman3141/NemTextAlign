using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NemTextAlign.Extensions {
  public static class Extensions {
    public static (List<T> alignedLeft, List<T> alignedRight) GetAlignedSideBySide<T>(this IList<T> left, IList<T> right) where T : IComparable {
      Matches matches = left.GetAlignmentData(right);

      List<T> leftList = new List<T>();
      List<T> rightList = new List<T>();

      foreach (MatchPair match in matches) {
        switch (match.MatchType) {
          case MatchTypes.RightOnly:  //Exists in destination but not in source
            for (int i = 0; i < match.RightRange.Length; i++) {
              leftList.Add(default);
              rightList.Add(right[i + match.RightRange.Start]);
            }
            break;
          case MatchTypes.LeftOnly:  //Exists in source but not in destination
            for (int i = 0; i < match.LeftRange.Length; i++) {
              leftList.Add(left[i + match.LeftRange.Start]);
              rightList.Add(default);
            }
            break;
          case MatchTypes.Both: //Exists in both source and destination
            for (int i = 0; i < match.LeftRange.Length; i++) {
              leftList.Add(left[i + match.LeftRange.Start]);
              rightList.Add(right[i + match.RightRange.Start]);
            }
            break;
        }
      }

      if (leftList.Count != rightList.Count) {
        int diff = leftList.Count - rightList.Count;
        List<T> shortList = diff > 0 ? leftList : rightList;
        for (int i = 0; i < Math.Abs(diff); i++) {
          shortList.Add(default);
        }
      }

      return (leftList, rightList);
    }

    public static Matches GetAlignmentData<T>(this IList<T> left, IList<T> right) where T : IComparable {
      Matches matches = left.GetAllMatches(right);

      for (int i = 0; i < left.Count; i++) {
        if (!matches.ContainsLeftValue(i)) {
          Range range = new Range(i, 1);

          MatchPair leftOnly = new MatchPair(range, null, MatchTypes.LeftOnly);
          matches.Add(leftOnly);
        }
      }

      for (int i = 0; i < right.Count; i++) {
        if (!matches.ContainsRightValue(i)) {
          Range range = new Range(i, 1);

          MatchPair rightOnly = new MatchPair(null, range, MatchTypes.RightOnly);
          matches.Add(rightOnly);
        }
      }

      matches.Sort();

      return matches;
    }

    public static Matches GetAllMatches<T>(this IList<T> left, IList<T> right) where T : IComparable {
      Matches allMatches = new Matches();
      MatchPair match = left.LongestMatchingSegment(right);

      if (match != null) {
        allMatches.Add(match);
        //If the match is the full list, add it to the matches, and exit
        if (match.LeftRange.Length != left.Count) {
          List<T> topLeft = left.GetRange(0, match.LeftRange.Start).ToList();
          List<T> topRight = right.GetRange(0, match.RightRange.Start).ToList();
          if (topLeft.Count > 0 && topRight.Count > 0) {
            Matches topMatches = GetAllMatches(topLeft, topRight);
            allMatches.AddRange(topMatches);
          }

          int leftStartIndex = match.LeftRange.End + 1;
          int rightStartIndex = match.RightRange.End + 1;

          List<T> bottomLeft = left.GetRange(match.LeftRange.End + 1, left.Count - (match.LeftRange.End + 1)).ToList();
          List<T> bottomRight = right.GetRange(match.RightRange.End + 1, right.Count - (match.RightRange.End + 1)).ToList();
          if (bottomLeft.Count > 0 && bottomRight.Count > 0) {
            Matches bottomMatches = GetAllMatches(bottomLeft, bottomRight);
            bottomMatches.Shift(leftStartIndex, rightStartIndex);
            allMatches.AddRange(bottomMatches);
          }
        }
      }

      return allMatches;
    }

    public static MatchPair LongestMatchingSegment<T>(this IList<T> left, IList<T> right) where T : IComparable {
      int leftCount = left.Count;
      int rightCount = right.Count;
      int shortCount = Math.Min(leftCount, rightCount);

      IList<T> shortList = leftCount == shortCount ? left : right;
      IList<T> longList = leftCount == shortCount ? right : left;
      bool leftIsShort = leftCount == shortCount;

      foreach ((List<T> values, int shortIndex) in shortList.NGrams(1, shortCount)) {
        int valuesCount = values.Count;

        int[] longIndexes = longList.IndexesOf(values);
        if (longIndexes.Length == 0) {  //If we didn't find any matches, continue to the next nGram.
          continue;
        }

        int longIndex = longIndexes[0];
        //If more than one index was found take the one with the closest index to the ngram we're looking for in the short list
        if (longIndexes.Length > 1) {
          longIndex = longIndexes.Aggregate((x, y) => Math.Abs(x - shortIndex) < Math.Abs(y - shortIndex) ? x : y);
        }

        Range longRange = new Range(longIndex, valuesCount);
        Range shortRange = new Range(shortIndex, valuesCount);

        return leftIsShort ? new MatchPair(shortRange, longRange, MatchTypes.Both) : new MatchPair(longRange, shortRange, MatchTypes.Both);
      }

      return null;
    }

    public static IEnumerable<(List<T>, int index)> NGrams<T>(this IList<T> source, int minSize, int maxSize) {
      if (minSize <= 0 || minSize > source.Count || minSize > maxSize) {
        throw new ArgumentOutOfRangeException("Invalid minimum size specified.");
      }
      if (maxSize <= 0 || maxSize > source.Count) {
        throw new ArgumentOutOfRangeException("Invalid maximum size specified.");
      }

      for (int size = maxSize; size >= minSize; size--) {
        for (int i = 0; i < source.Count - size + 1; i++) {
          yield return (source.GetRange(i, size).ToList(), i);
        }
      }
    }

    public static int IndexOf<T>(this IList<T> source, T value) where T : IComparable {
      List<T> valueList = new List<T>() { value };
      return source.IndexOf(valueList);
    }

    public static int IndexOf<T>(this IList<T> source, IList<T> value) where T : IComparable {
      int sourceCount = source.Count;
      int valueCount = value.Count;
      
      for (int i = 0; i < sourceCount - valueCount + 1; i++) {
        List<T> sourceRange = source.GetRange(i, valueCount).ToList();

        if (sourceRange.RangeEquals(value)) {
          return i;
        }
      }

      return -1;
    }

    public static int[] IndexesOf<T>(this IList<T> source, T value) where T : IComparable {
      List<T> valueList = new List<T>() { value };
      return source.IndexesOf(valueList);
    }

    public static int[] IndexesOf<T>(this IList<T> source, IList<T> value) where T : IComparable {
      List<int> indexes = new List<int>();
      int sourceCount = source.Count;
      int valueCount = value.Count;

      Parallel.For(0, sourceCount - valueCount + 1, i => {
        List<T> sourceRange = source.GetRange(i, valueCount).ToList();

        if (sourceRange.RangeEquals(value)) {
          indexes.Add(i);
        }

      });

      return indexes.ToArray();
    }

    public static IEnumerable<T> GetRange<T>(this IList<T> source, int startIndex, int length) {
      if (startIndex < 0) {
        throw new ArgumentOutOfRangeException("startIndex cannot be negative");
      }
      if (length < 0) {
        throw new ArgumentOutOfRangeException("length cannot be less than 0");
      }

      //If length == 0 return an empty IEnumerable
      if (length == 0) {
        yield break;
      }

      for (int i = 0; i < length; i++) {
        yield return source[i + startIndex];
      }

    }

    public static bool RangeEquals<T>(this IList<T> source, IList<T> value) where T : IComparable {
      if (ReferenceEquals(source, value)) {
        return true;
      }

      int sourceCount = source.Count;
      int valueCount = value.Count;

      if (sourceCount != valueCount) {
        return false;
      }

      for (int i = 0; i < sourceCount; i++) {
        if (source.ElementAt(i).CompareTo(value[i]) != 0) {
          return false;
        }
      }

      return true;
    }

    public static void AddAt<T>(this List<T> source, int index, T toAdd, T defaultValue = default) {
      if (index < 0) {
        throw new IndexOutOfRangeException();
      }

      while (index > source.Count - 1) {
        source.Add(defaultValue);
      }

      source[index] = toAdd;
    }
  }
}
