using System;
using System.Collections.Generic;
using System.Text;

namespace NemTextAlign {
  public class Range {
    public int Start { get; private set; }
    public int Length { get; private set; }
    public int End { get; private set; }

    public Range(int start, int length) {
      Start = start;
      Length = length;
      End = start + (length - 1);
    }

    public bool IsContainedBy(Range range) {
      return range != null && range.Start <= Start && range.End >= End;
    }

    public bool Contains(Range range) {
      return range != null && Start <= range.Start && End >= range.End;
    }

    public bool Contains(int index) {
      return Start <= index && End >= index;
    }

    public static bool operator ==(Range rangeA, Range rangeB) {
      if (rangeA is null && rangeB is null) {
        return true;
      } else if (rangeA is null && !(rangeB is null)) {
        return false;
      } else if (!(rangeA is null) && rangeB is null) {
        return false;
      }

      return rangeA.Equals(rangeB);
    }
    public static bool operator !=(Range rangeA, Range rangeB) {
      if(rangeA is null && rangeB is null) {
        return false;
      } else if(rangeA is null && !(rangeB is null)) {
        return true;
      } else if(!(rangeA is null) && rangeB is null) {
        return true;
      }

      return !rangeA.Equals(rangeB);
    }

    public bool Equals(Range range) {
      if (range is null && !(this is null)) {
        return false;
      } else if (!(range is null) && this is null) {
        return false;
      }

      return ReferenceEquals(this, range) ? true : Start == range.Start && Length == range.Length;
    }

    public override bool Equals(object obj) {
      if(!(obj is Range)) {
        return false;
      } else if(obj is null && !(this is null)) {
        return false;
      } else if(!(obj is null) && this is null) {
        return false;
      }

      return Equals((Range)obj);
    }

    public bool Overlaps(Range range) {
      if(range == null) {
        return false;
      }

      if(Equals(range) || IsContainedBy(range) || Contains(range)) {
        return true;
      }

      bool startWithin = Start >= range.Start && Start <= range.End;
      bool endWithin = End >= range.Start && End <= range.End;

      return startWithin || endWithin;
    }

    public override int GetHashCode() {
      int hashCode = 0;
      hashCode += Start.GetHashCode() * 2;
      hashCode += Length.GetHashCode() * 3;

      return hashCode;
    }

    public void Shift(int shiftAmount) {
      Start += shiftAmount;
      End += shiftAmount;
    }

  }
}
