using NemTextAlign;
using NemTextAlign.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Range = NemTextAlign.Range;

namespace XU_NemTextAlign {
  public class NemExtensionsTests {
    [Fact]
    public void TestRangeEquals() {
      List<string> str1List = "This is a test of the emergency broadcast system. It is only a test.".Split(' ').ToList();
      List<string> str2List = "This is a test of the emergency system. It is only a test.".Split(' ').ToList();

      Assert.False(str1List.RangeEquals(str2List));
      Assert.True(str1List.GetRange(0, 7).RangeEquals(str2List.GetRange(0, 7)));
      Assert.True(str1List.GetRange(9, 5).RangeEquals(str2List.GetRange(8, 5)));
    }

    [Fact]
    public void TestIEnumerableGetRange() {
      string[] strArray = "This is a test of the emergency broadcast system. It is only a test.".Split(' ');

      Assert.Equal("This", strArray.GetRange(0, 3).ElementAt(0));
      Assert.Equal("is", strArray.GetRange(0, 3).ElementAt(1));
      Assert.Equal("a", strArray.GetRange(0, 3).ElementAt(2));

      Assert.Equal("emergency", strArray.GetRange(6, 2).ElementAt(0));
      Assert.Equal("broadcast", strArray.GetRange(6, 2).ElementAt(1));

      Assert.Equal("It", strArray.GetRange(9, 5).ElementAt(0));
      Assert.Equal("is", strArray.GetRange(9, 5).ElementAt(1));
      Assert.Equal("only", strArray.GetRange(9, 5).ElementAt(2));
      Assert.Equal("a", strArray.GetRange(9, 5).ElementAt(3));
      Assert.Equal("test.", strArray.GetRange(9, 5).ElementAt(4));
    }

    [Fact]
    public void TestIEnumerableIndexesOf() {
      List<string> strList = "This is a test of the emergency broadcast system. It is only a test".Split(' ').ToList();
      List<string> lookFor1 = "a test".Split(' ').ToList();
      List<string> lookFor2 = "of the emergency".Split(' ').ToList();
      List<string> notToFind = "only is It".Split(' ').ToList();

      int[] indexes = strList.IndexesOf("a");
      Assert.Equal(2, indexes.Length);
      Assert.Contains(2, indexes);
      Assert.Contains(12, indexes);

      indexes = strList.IndexesOf(lookFor1);
      Assert.Equal(2, indexes.Length);
      Assert.Contains(2, indexes);
      Assert.Contains(12, indexes);

      indexes = strList.IndexesOf(lookFor2);
      Assert.Single(indexes);
      Assert.Contains(4, indexes);

      indexes = strList.IndexesOf(notToFind);
      Assert.Empty(indexes);
    }

    [Fact]
    public void TestIEnumerableIndexOf() {
      List<string> strList = "This is a test of the emergency broadcast system. It is only a test".Split(' ').ToList();
      List<string> lookFor1 = "a test".Split(' ').ToList();
      List<string> lookFor2 = "of the emergency".Split(' ').ToList();
      List<string> notToFind = "only is It".Split(' ').ToList();

      int index = strList.IndexOf("a");
      Assert.Equal(2, index);

      index = strList.IndexOf(lookFor1);
      Assert.Equal(2, index);

      index = strList.IndexOf(lookFor2);
      Assert.Equal(4, index);

      index = strList.IndexOf(notToFind);
      Assert.Equal(-1, index);
    }

    [Fact]
    public void TestNGrams() {
      List<string> strList = "This is a test of the emergency broadcast system. It is only a test.".Split(' ').ToList();

      IEnumerable<(List<string> ngram, int index)> nGrams = strList.NGrams(1, 3);
      //Tri-grams
      Assert.True(nGrams.ElementAt(0).ngram.RangeEquals(new[] { "This", "is", "a" }));
      Assert.Equal(0, nGrams.ElementAt(0).index);
      Assert.True(nGrams.ElementAt(1).ngram.RangeEquals(new[] { "is", "a", "test" }));
      Assert.Equal(1, nGrams.ElementAt(1).index);
      Assert.True(nGrams.ElementAt(2).ngram.RangeEquals(new[] { "a", "test", "of" }));
      Assert.Equal(2, nGrams.ElementAt(2).index);
      Assert.True(nGrams.ElementAt(3).ngram.RangeEquals(new[] { "test", "of", "the" }));
      Assert.Equal(3, nGrams.ElementAt(3).index);
      Assert.True(nGrams.ElementAt(4).ngram.RangeEquals(new[] { "of", "the", "emergency" }));
      Assert.Equal(4, nGrams.ElementAt(4).index);
      Assert.True(nGrams.ElementAt(5).ngram.RangeEquals(new[] { "the", "emergency", "broadcast" }));
      Assert.Equal(5, nGrams.ElementAt(5).index);
      Assert.True(nGrams.ElementAt(6).ngram.RangeEquals(new[] { "emergency", "broadcast", "system." }));
      Assert.Equal(6, nGrams.ElementAt(6).index);
      Assert.True(nGrams.ElementAt(7).ngram.RangeEquals(new[] { "broadcast", "system.", "It" }));
      Assert.Equal(7, nGrams.ElementAt(7).index);
      Assert.True(nGrams.ElementAt(8).ngram.RangeEquals(new[] { "system.", "It", "is" }));
      Assert.Equal(8, nGrams.ElementAt(8).index);
      Assert.True(nGrams.ElementAt(9).ngram.RangeEquals(new[] { "It", "is", "only" }));
      Assert.Equal(9, nGrams.ElementAt(9).index);
      Assert.True(nGrams.ElementAt(10).ngram.RangeEquals(new[] { "is", "only", "a" }));
      Assert.Equal(10, nGrams.ElementAt(10).index);
      Assert.True(nGrams.ElementAt(11).ngram.RangeEquals(new[] { "only", "a", "test." }));
      Assert.Equal(11, nGrams.ElementAt(11).index);

      //Bi-grams
      Assert.True(nGrams.ElementAt(12).ngram.RangeEquals(new[] { "This", "is" }));
      Assert.Equal(0, nGrams.ElementAt(12).index);
      Assert.True(nGrams.ElementAt(13).ngram.RangeEquals(new[] { "is", "a" }));
      Assert.Equal(1, nGrams.ElementAt(13).index);
      Assert.True(nGrams.ElementAt(14).ngram.RangeEquals(new[] { "a", "test" }));
      Assert.Equal(2, nGrams.ElementAt(14).index);
      Assert.True(nGrams.ElementAt(15).ngram.RangeEquals(new[] { "test", "of" }));
      Assert.Equal(3, nGrams.ElementAt(15).index);
      Assert.True(nGrams.ElementAt(16).ngram.RangeEquals(new[] { "of", "the" }));
      Assert.Equal(4, nGrams.ElementAt(16).index);
      Assert.True(nGrams.ElementAt(17).ngram.RangeEquals(new[] { "the", "emergency" }));
      Assert.Equal(5, nGrams.ElementAt(17).index);
      Assert.True(nGrams.ElementAt(18).ngram.RangeEquals(new[] { "emergency", "broadcast" }));
      Assert.Equal(6, nGrams.ElementAt(18).index);
      Assert.True(nGrams.ElementAt(19).ngram.RangeEquals(new[] { "broadcast", "system." }));
      Assert.Equal(7, nGrams.ElementAt(19).index);
      Assert.True(nGrams.ElementAt(20).ngram.RangeEquals(new[] { "system.", "It" }));
      Assert.Equal(8, nGrams.ElementAt(20).index);
      Assert.True(nGrams.ElementAt(21).ngram.RangeEquals(new[] { "It", "is" }));
      Assert.Equal(9, nGrams.ElementAt(21).index);
      Assert.True(nGrams.ElementAt(22).ngram.RangeEquals(new[] { "is", "only" }));
      Assert.Equal(10, nGrams.ElementAt(22).index);
      Assert.True(nGrams.ElementAt(23).ngram.RangeEquals(new[] { "only", "a" }));
      Assert.Equal(11, nGrams.ElementAt(23).index);
      Assert.True(nGrams.ElementAt(24).ngram.RangeEquals(new[] { "a", "test." }));
      Assert.Equal(12, nGrams.ElementAt(24).index);


      //"Uni-grams
      Assert.True(nGrams.ElementAt(25).ngram.RangeEquals(new[] { "This" }));
      Assert.Equal(0, nGrams.ElementAt(25).index);
      Assert.True(nGrams.ElementAt(26).ngram.RangeEquals(new[] { "is" }));
      Assert.Equal(1, nGrams.ElementAt(26).index);
      Assert.True(nGrams.ElementAt(27).ngram.RangeEquals(new[] { "a" }));
      Assert.Equal(2, nGrams.ElementAt(27).index);
      Assert.True(nGrams.ElementAt(28).ngram.RangeEquals(new[] { "test" }));
      Assert.Equal(3, nGrams.ElementAt(28).index);
      Assert.True(nGrams.ElementAt(29).ngram.RangeEquals(new[] { "of" }));
      Assert.Equal(4, nGrams.ElementAt(29).index);
      Assert.True(nGrams.ElementAt(30).ngram.RangeEquals(new[] { "the" }));
      Assert.Equal(5, nGrams.ElementAt(30).index);
      Assert.True(nGrams.ElementAt(31).ngram.RangeEquals(new[] { "emergency" }));
      Assert.Equal(6, nGrams.ElementAt(31).index);
      Assert.True(nGrams.ElementAt(32).ngram.RangeEquals(new[] { "broadcast" }));
      Assert.Equal(7, nGrams.ElementAt(32).index);
      Assert.True(nGrams.ElementAt(33).ngram.RangeEquals(new[] { "system." }));
      Assert.Equal(8, nGrams.ElementAt(33).index);
      Assert.True(nGrams.ElementAt(34).ngram.RangeEquals(new[] { "It" }));
      Assert.Equal(9, nGrams.ElementAt(34).index);
      Assert.True(nGrams.ElementAt(35).ngram.RangeEquals(new[] { "is" }));
      Assert.Equal(10, nGrams.ElementAt(35).index);
      Assert.True(nGrams.ElementAt(36).ngram.RangeEquals(new[] { "only" }));
      Assert.Equal(11, nGrams.ElementAt(36).index);
      Assert.True(nGrams.ElementAt(37).ngram.RangeEquals(new[] { "a" }));
      Assert.Equal(12, nGrams.ElementAt(37).index);
      Assert.True(nGrams.ElementAt(38).ngram.RangeEquals(new[] { "test." }));
      Assert.Equal(13, nGrams.ElementAt(38).index);

    }

    [Fact]
    public void TestLongestMatchingSegment() {
      List<string> str1List = "This is a test of the emergency broadcast system. It is only a test.".Split(' ').ToList();
      List<string> str2List = "This test of the emergency system. It is a test.".Split(' ').ToList();

      MatchPair matchPair = str1List.LongestMatchingSegment(str2List);

      Assert.Equal(3, matchPair.LeftRange.Start);
      Assert.Equal(6, matchPair.LeftRange.End);
      Assert.Equal(4, matchPair.LeftRange.Length);

      Assert.Equal(1, matchPair.RightRange.Start);
      Assert.Equal(4, matchPair.RightRange.End);
      Assert.Equal(4, matchPair.RightRange.Length);
    }

    [Fact]
    public void TestGetAllMatches() {
      List<string> str1List = "This is a test of the emergency broadcast system. It is only a test.".Split(' ').ToList();
      List<string> str2List = "This test of the emergency system. It is a test.".Split(' ').ToList();

      Matches matches = str1List.GetAllMatches(str2List);

      List<MatchPair> correctMatches = new List<MatchPair>() {
        new MatchPair(new Range(0, 1), new Range(0, 1), MatchTypes.Both), //This
        new MatchPair(new Range(3, 4), new Range(1, 4), MatchTypes.Both), //test of the emergency
        new MatchPair(new Range(8, 3), new Range(5, 3), MatchTypes.Both), //system. It is
        new MatchPair(new Range(12, 2), new Range(8, 2), MatchTypes.Both), //a test.
      };

      foreach(MatchPair correctMatch in correctMatches) {
        Assert.True(matches.Contains(correctMatch));
      }
    }

    [Fact]
    public void TestGetAlignmentData() {
      List<string> str1List = "This is a test of the emergency broadcast system. It is only a test.".Split(' ').ToList();
      List<string> str2List = "This test of the emergency system. It is a test.".Split(' ').ToList();

      Matches matches = str1List.GetAlignmentData(str2List);

      List<MatchPair> correctMatches = new List<MatchPair>() {
        new MatchPair(new Range(0, 1), new Range(0, 1), MatchTypes.Both), //This
        new MatchPair(new Range(1, 1), null, MatchTypes.LeftOnly), //is
        new MatchPair(new Range(2, 1), null, MatchTypes.LeftOnly), //1
        new MatchPair(new Range(3, 4), new Range(1, 4), MatchTypes.Both), //test of the emergency
        new MatchPair(new Range(7, 1), null, MatchTypes.LeftOnly), //broadcast
        new MatchPair(new Range(8, 3), new Range(5, 3), MatchTypes.Both), //system. It is
        new MatchPair(new Range(11, 1), null, MatchTypes.LeftOnly), //only
        new MatchPair(new Range(12, 2), new Range(8, 2), MatchTypes.Both), //a test.
      };

      foreach(MatchPair correctMatch in correctMatches) {
        Assert.True(matches.Contains(correctMatch));
      }
    }

  }
}
