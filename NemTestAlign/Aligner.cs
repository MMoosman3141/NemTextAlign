using NemTextAlign.DifferenceEngine;
using System.Collections.Generic;
using System.Linq;

namespace NemTextAlign {
  public static class Aligner {
    public static (List<T> expectedOut, List<T> actualOut) Align<T>(IEnumerable<T> expected, IEnumerable<T> actual, T emptyValue) {
      return Align<T>(expected, actual, emptyValue, Comparer<T>.Default);
    }

    public static (List<T> expectedOut, List<T> actualOut) Align<T>(IEnumerable<T> expected, IEnumerable<T> actual, T emptyValue, IComparer<T> comparer) {
      DiffEngine<T> diffEngine = new DiffEngine<T>();
      List<DiffResultSpan> diffResults = diffEngine.ProcessDiff(expected, actual, DiffEngineLevel.FastImperfect, comparer);

      List<T> expectedOut = new List<T>();
      List<T> actualOut = new List<T>();

      foreach (DiffResultSpan diffSpan in diffResults) {
        switch (diffSpan.Status) {
          case DiffResultSpanStatus.AddDestination:  //Exists in destination but not in source
            for (int i = 0; i < diffSpan.Length; i++) {
              expectedOut.Add(emptyValue);
              actualOut.Add(actual.ElementAt(i + diffSpan.DestIndex));
            }
            break;
          case DiffResultSpanStatus.DeleteSource:  //Exists in source but not in destination
            for (int i = 0; i < diffSpan.Length; i++) {
              expectedOut.Add(expected.ElementAt(i + diffSpan.SourceIndex));
              actualOut.Add(emptyValue);
            }
            break;
          case DiffResultSpanStatus.NoChange: //Exists in both source and destination
            for (int i = 0; i < diffSpan.Length; i++) {
              expectedOut.Add(expected.ElementAt(i + diffSpan.SourceIndex));
              actualOut.Add(actual.ElementAt(i + diffSpan.DestIndex));
            }
            break;
          case DiffResultSpanStatus.Replace: // Both contain a new thing
            for (int i = 0; i < diffSpan.Length; i++) {
              expectedOut.Add(emptyValue);
              actualOut.Add(actual.ElementAt(i + diffSpan.DestIndex));
            }
            for (int i = 0; i < diffSpan.Length; i++) {
              expectedOut.Add(expected.ElementAt(i + diffSpan.SourceIndex));
              actualOut.Add(emptyValue);
            }
            break;
        }
      }

      return (expectedOut, actualOut);
    }

  }
}
