using System;

namespace NemTextAlign {
  public class MatchPair : IComparable {
    public Range LeftRange { get; private set; }
    public Range RightRange { get; private set; }
    public MatchTypes MatchType { get; set; } = MatchTypes.None;
    public int Length {
      get => LeftRange?.Length ?? RightRange.Length;
    }

    public MatchPair(Range left, Range right, MatchTypes matchType) {
      LeftRange = left;
      RightRange = right;
      MatchType = matchType;
    }

    public void Shift(int leftAdjustment, int rightAdjustment) {
      LeftRange.Shift(leftAdjustment);
      RightRange.Shift(rightAdjustment);
    }

    public static bool operator ==(MatchPair a, MatchPair b) {
      return a?.Equals(b) ?? b is null;
    }

    public static bool operator !=(MatchPair a, MatchPair b) {
      return !(a?.Equals(b) ?? b is null);
    }

    public bool Equals(MatchPair matchPair) {
      if (matchPair is null && !(this is null)) {
        return false;
      } else if (!(matchPair is null) && this is null) {
        return false;
      }

      if (ReferenceEquals(this, matchPair)) {
        return true;
      }

      if (LeftRange != matchPair.LeftRange) {
        return false;
      }

      if (RightRange != matchPair.RightRange) {
        return false;
      }

      if (MatchType != matchPair.MatchType) {
        return false;
      }

      return true;
    }
    public override bool Equals(object obj) {
      if (!(obj is MatchPair)) {
        return false;
      } else if (obj is null && !(this is null)) {
        return false;
      } else if (!(obj is null) && this is null) {
        return false;
      }

      return Equals((MatchPair)obj);
    }

    public override int GetHashCode() {
      int hashCode = 0;
      hashCode += LeftRange.GetHashCode() * 2;
      hashCode += RightRange.GetHashCode() * 3;
      hashCode += MatchType.GetHashCode() * 5;

      return hashCode;
    }

    public override string ToString() {
      return $"{MatchType} (Left Start: {LeftRange?.Start.ToString() ?? ""}, Right Start: {RightRange?.Start.ToString() ?? ""}) {LeftRange?.Length ?? RightRange.Length }";
    }

    public int CompareTo(object obj) {
      return CompareTo((MatchPair)obj);
    }

    public int CompareTo(MatchPair matchPair) {
      int thisStart = RightRange?.Start ?? -1;
      int thatStart = matchPair.RightRange?.Start ?? -1;
      return thisStart.CompareTo(thatStart);
    }
  }
}
