using NemTextAlign.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;

namespace XU_NemTextAlign {
  public class NemAlignmentTests {
    private static readonly Random _random = new Random();

    private readonly List<string> _immediateFamily = new List<string>() {
        "Karla",
        "Jeanene",
        "Carolyn",
        "Kenneth",
        "Mark",
        "Michael"
      };
    private readonly List<string> _extendedFamily = new List<string>() {
        "Karla",
        "Jeanene",
        "Craig",
        "Tamara",
        "Richard",
        "Kathrine",
        "Brian",
        "Austin",
        "Carolyn",
        "Guy",
        "Brett",
        "Kira",
        "Jessica",
        "Maria",
        "Kenneth",
        "Debbie",
        "Bethany",
        "Corrine",
        "Mark",
        "Michael",
        "Noah",
      };
    private readonly List<string> _emergencyBroadcastPhrase1 = new List<string>() {
      "This", "is", "a", "test", "of", "the", "emergency", "broadcast", "system.", "It", "is", "only", "a", "test."
    };
    private readonly List<string> _emergencyBroadcastPhrase2 = new List<string>() {
      "This", "test", "of", "the", "emergency", "broadcast", "system.", "Well", "it", "is", "a", "test", "and", "a", "good", "one."
    };
    private readonly List<string> _shortListA = new List<string> { "It", "is", "only", "a", "test." };
    private readonly List<string> _shortListB = new List<string> { "It", "is", "a", "different",  "test." };

    [Fact]
    public void TestAlign1() {
      (List<string> immediateOut, List<string> extendedOut) = _immediateFamily.GetAlignedSideBySide<string>(_extendedFamily);

      Assert.Equal(immediateOut.Count, extendedOut.Count);
      Assert.Equal(22, immediateOut.Union(extendedOut).Count());
      Assert.Equal(6, immediateOut.Intersect(extendedOut).Count());
      Assert.Equal(15, extendedOut.Except(immediateOut).Count());

      Assert.Equal(immediateOut.IndexOf("Karla"), extendedOut.IndexOf("Karla"));
      Assert.Equal(immediateOut.IndexOf("Jeanene"), extendedOut.IndexOf("Jeanene"));
      Assert.Equal(immediateOut.IndexOf("Carolyn"), extendedOut.IndexOf("Carolyn"));
      Assert.Equal(immediateOut.IndexOf("Kenneth"), extendedOut.IndexOf("Kenneth"));
      Assert.Equal(immediateOut.IndexOf("Mark"), extendedOut.IndexOf("Mark"));
      Assert.Equal(immediateOut.IndexOf("Michael"), extendedOut.IndexOf("Michael"));
    }

    [Fact]
    public void TestAlign2() {
      (List<string> extendedOut, List<string> immediateOut) = _extendedFamily.GetAlignedSideBySide(_immediateFamily);

      Assert.Equal(immediateOut.Count, extendedOut.Count);
      Assert.Equal(22, immediateOut.Union(extendedOut).Count());
      Assert.Equal(6, immediateOut.Intersect(extendedOut).Count());
      Assert.Equal(15, extendedOut.Except(immediateOut).Count());

      Assert.Equal(immediateOut.IndexOf("Karla"), extendedOut.IndexOf("Karla"));
      Assert.Equal(immediateOut.IndexOf("Jeanene"), extendedOut.IndexOf("Jeanene"));
      Assert.Equal(immediateOut.IndexOf("Carolyn"), extendedOut.IndexOf("Carolyn"));
      Assert.Equal(immediateOut.IndexOf("Kenneth"), extendedOut.IndexOf("Kenneth"));
      Assert.Equal(immediateOut.IndexOf("Mark"), extendedOut.IndexOf("Mark"));
      Assert.Equal(immediateOut.IndexOf("Michael"), extendedOut.IndexOf("Michael"));
    }

    [Fact]
    public void TestAlign3() {
      (List<string> phrase1, List<string> phrase2) = _emergencyBroadcastPhrase1.GetAlignedSideBySide(_emergencyBroadcastPhrase2);

      Assert.Equal(phrase1.Count, phrase2.Count);

      Assert.True(phrase1[0] == "This" && phrase2[0] == "This");
      Assert.True(phrase1[1] == "is" && phrase2[1] is null);
      Assert.True(phrase1[2] == "a" && phrase2[2] is null);
      Assert.True(phrase1[3] == "test" && phrase2[3] == "test");
      Assert.True(phrase1[4] == "of" && phrase2[4] == "of");
      Assert.True(phrase1[5] == "the" && phrase2[5] == "the");
      Assert.True(phrase1[6] == "emergency" && phrase2[6] == "emergency");
      Assert.True(phrase1[7] == "broadcast" && phrase2[7] == "broadcast");
      Assert.True(phrase1[8] == "system." && phrase2[8] == "system.");
      Assert.True(phrase1[9] is null && phrase2[9] == "Well");
      Assert.True(phrase1[10] is null && phrase2[10] == "it");
      Assert.True(phrase1[11] == "It" && phrase2[11] is null);
      Assert.True(phrase1[12] == "is" && phrase2[12] == "is");
      Assert.True(phrase1[13] == "only" && phrase2[13] is null);
      Assert.True(phrase1[14] == "a" && phrase2[14] == "a");
      Assert.True(phrase1[15] is null && phrase2[15] == "test");
      Assert.True(phrase1[16] is null && phrase2[16] == "and");
      Assert.True(phrase1[17] is null && phrase2[17] == "a");
      Assert.True(phrase1[18] is null && phrase2[18] == "good");
      Assert.True(phrase1[19] is null && phrase2[19] == "one.");
      Assert.True(phrase1[20] == "test." && phrase2[20] is null);
    }

    [Fact]
    public void TestAlign4() {
      (List<string> phrase1, List<string> phrase2) = _shortListA.GetAlignedSideBySide(_shortListB);

      Assert.Equal(phrase1.Count, phrase2.Count);

    }

    [Fact]
    public void TestAlignNullDefault() {
      (List<string> immediateOut, List<string> extendedOut) = _immediateFamily.GetAlignedSideBySide(_extendedFamily);

      Assert.Equal(immediateOut.Count, extendedOut.Count);
      Assert.Equal(22, immediateOut.Union(extendedOut).Count());
      Assert.Equal(6, immediateOut.Intersect(extendedOut).Count());
      Assert.Equal(15, extendedOut.Except(immediateOut).Count());

      Assert.Equal(immediateOut.IndexOf("Karla"), extendedOut.IndexOf("Karla"));
      Assert.Equal(immediateOut.IndexOf("Jeanene"), extendedOut.IndexOf("Jeanene"));
      Assert.Equal(immediateOut.IndexOf("Carolyn"), extendedOut.IndexOf("Carolyn"));
      Assert.Equal(immediateOut.IndexOf("Kenneth"), extendedOut.IndexOf("Kenneth"));
      Assert.Equal(immediateOut.IndexOf("Mark"), extendedOut.IndexOf("Mark"));
      Assert.Equal(immediateOut.IndexOf("Michael"), extendedOut.IndexOf("Michael"));

    }

    //[Fact]
    //public void TestAlignCustomComparer() {
    //  (List<string> immediateOut, List<string> extendedOut) = _immediateFamily.GetAlignedSideBySide(_extendedFamily, new CustomComparer());

    //  Assert.Equal(immediateOut.Count, extendedOut.Count);
    //  Assert.Equal(22, immediateOut.Union(extendedOut).Count());
    //  Assert.Equal(6, immediateOut.Intersect(extendedOut).Count());
    //  Assert.Equal(15, extendedOut.Except(immediateOut).Count());

    //  Assert.Equal(immediateOut.IndexOf("Karla"), extendedOut.IndexOf("Karla"));
    //  Assert.Equal(immediateOut.IndexOf("Jeanene"), extendedOut.IndexOf("Jeanene"));
    //  Assert.Equal(immediateOut.IndexOf("Carolyn"), extendedOut.IndexOf("Craig"));
    //  Assert.Equal(immediateOut.IndexOf("Kenneth"), extendedOut.IndexOf("Kathrine"));
    //  Assert.Equal(immediateOut.IndexOf("Mark"), extendedOut.IndexOf("Mark"));
    //  Assert.Equal(immediateOut.IndexOf("Michael"), extendedOut.IndexOf("Michael"));
    //}

    [Fact(Skip = "Skipping")]
    public void TestPerformance() {
      List<string> randomLines1 = GenerateRandomLines(20_000);
      List<string> randomLines2 = GenerateRandomLines(20_000);

      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      randomLines1.GetAlignedSideBySide(randomLines2);
      stopwatch.Stop();

      Assert.True(stopwatch.ElapsedMilliseconds < 60_000);
    }

    private List<string> GenerateRandomLines(int count) {
      List<string> lines = new List<string>();

      for (int i = 0; i < count; i++) {
        lines.Add(GenerateRandomPhrase());
      }

      return lines;
    }

    private string GenerateRandomPhrase() {
      int length = _random.Next(3, 11);

      List<string> words = new List<string>();
      for (int i = 0; i < length; i++) {
        words.Add(GetRandomStr());
      }

      return string.Join(' ', words);
    }

    private string GetRandomStr() {
      string[] letters = "a b c d e f g h i j k l m n o p q r s t u v w x y z".Split(' ');

      int length = _random.Next(3, 8);

      StringBuilder strBuilder = new StringBuilder();
      for (int i = 0; i < length; i++) {
        string letter = letters[_random.Next(letters.Length)];
        strBuilder.Append(letter);
      }

      return strBuilder.ToString();
    }

  }
}
