using NemTextAlign;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;

namespace XU_NemTextAlign {
  public class PotterAlignmentTests {
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

    [Fact]
    public void TestAlign1() {
      (List<string> immediateOut, List<string> extendedOut) = Aligner.Align<string>(_immediateFamily, _extendedFamily, "");

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
      (List<string> extendedOut, List<string> immediateOut) = Aligner.Align<string>(_extendedFamily, _immediateFamily, "");

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
      (List<string> phrase1, List<string> phrase2) = Aligner.Align<string>(_emergencyBroadcastPhrase1, _emergencyBroadcastPhrase2, "");

      Assert.Equal(phrase1.Count, phrase2.Count);

      Assert.True(phrase1[0] == "This" && phrase2[0] == "This");
      Assert.True(phrase1[1] == "is" && phrase2[1] == "");
      Assert.True(phrase1[2] == "a" && phrase2[2] == "");
      Assert.True(phrase1[3] == "test" && phrase2[3] == "test");
      Assert.True(phrase1[4] == "of" && phrase2[4] == "of");
      Assert.True(phrase1[5] == "the" && phrase2[5] == "the");
      Assert.True(phrase1[6] == "emergency" && phrase2[6] == "emergency");
      Assert.True(phrase1[7] == "broadcast" && phrase2[7] == "broadcast");
      Assert.True(phrase1[8] == "system." && phrase2[8] == "system.");
      Assert.True(phrase1[9] == "" && phrase2[9] == "Well");
      Assert.True(phrase1[10] == "It" && phrase2[10] == "");
      Assert.True(phrase1[11] == "" && phrase2[11] == "it");
      Assert.True(phrase1[12] == "is" && phrase2[12] == "is");
      Assert.True(phrase1[13] == "only" && phrase2[13] == "");
      Assert.True(phrase1[14] == "a" && phrase2[14] == "a");
      Assert.True(phrase1[15] == "" && phrase2[15] == "test");
      Assert.True(phrase1[16] == "test." && phrase2[16] == "");
      Assert.True(phrase1[17] == "" && phrase2[17] == "and");
      Assert.True(phrase1[18] == "" && phrase2[18] == "a");
      Assert.True(phrase1[19] == "" && phrase2[19] == "good");
      Assert.True(phrase1[20] == "" && phrase2[20] == "one.");
    }

    [Fact]
    public void TestAlignNullDefault() {
      (List<string> immediateOut, List<string> extendedOut) = Aligner.Align<string>(_immediateFamily, _extendedFamily, null);

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
    public void TestAlignCustomComparer() {
      (List<string> immediateOut, List<string> extendedOut) = Aligner.Align<string>(_immediateFamily, _extendedFamily, "", new CustomComparer());

      Assert.Equal(immediateOut.Count, extendedOut.Count);
      Assert.Equal(22, immediateOut.Union(extendedOut).Count());
      Assert.Equal(6, immediateOut.Intersect(extendedOut).Count());
      Assert.Equal(15, extendedOut.Except(immediateOut).Count());

      Assert.Equal(immediateOut.IndexOf("Karla"), extendedOut.IndexOf("Karla"));
      Assert.Equal(immediateOut.IndexOf("Jeanene"), extendedOut.IndexOf("Jeanene"));
      Assert.Equal(immediateOut.IndexOf("Carolyn"), extendedOut.IndexOf("Craig"));
      Assert.Equal(immediateOut.IndexOf("Kenneth"), extendedOut.IndexOf("Kathrine"));
      Assert.Equal(immediateOut.IndexOf("Mark"), extendedOut.IndexOf("Mark"));
      Assert.Equal(immediateOut.IndexOf("Michael"), extendedOut.IndexOf("Michael"));
    }

    [Fact(Skip = "Skipping")]
    public void TestPerformance() {
      List<string> randomLines1 = GenerateRandomLines(20_000);
      List<string> randomLines2 = GenerateRandomLines(20_000);

      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      Aligner.Align<string>(randomLines1, randomLines2, null);
      stopwatch.Stop();

      Assert.True(stopwatch.ElapsedMilliseconds < 60_000);
    }

    private List<string> GenerateRandomLines(int count) {
      List<string> lines = new List<string>();

      for(int i = 0; i < count; i++) {
        lines.Add(GenerateRandomPhrase());
      }

      return lines;
    }

    private string GenerateRandomPhrase() {
      int length = _random.Next(3, 11);

      List<string> words = new List<string>();
      for(int i = 0; i < length; i++) {
        words.Add(GetRandomStr());
      }

      return string.Join(' ', words);
    }

    private string GetRandomStr() {
      string[] letters = "a b c d e f g h i j k l m n o p q r s t u v w x y z".Split(' ');

      int length = _random.Next(3, 8);

      StringBuilder strBuilder = new StringBuilder();
      for(int i = 0; i < length; i++) {
        string letter = letters[_random.Next(letters.Length)];
        strBuilder.Append(letter);
      }

      return strBuilder.ToString();
    }

  }
}
