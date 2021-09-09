using System;

namespace NemTextAlign.DifferenceEngine {
  public class DiffResultSpan : IComparable {
    private const int BAD_INDEX = -1;
    private readonly int _destIndex;
    private readonly int _sourceIndex;
    private int _length;
    private readonly DiffResultSpanStatus _status;

    public int DestIndex { get => _destIndex; }
    public int SourceIndex { get => _sourceIndex; }
    public int Length { get => _length; }
    public DiffResultSpanStatus Status { get => _status; }

    protected DiffResultSpan(DiffResultSpanStatus status, int destIndex, int sourceIndex, int length) {
      _status = status;
      _destIndex = destIndex;
      _sourceIndex = sourceIndex;
      _length = length;
    }

    public static DiffResultSpan CreateNoChange(int destIndex, int sourceIndex, int length) {
      return new DiffResultSpan(DiffResultSpanStatus.NoChange, destIndex, sourceIndex, length);
    }

    public static DiffResultSpan CreateReplace(int destIndex, int sourceIndex, int length) {
      return new DiffResultSpan(DiffResultSpanStatus.Replace, destIndex, sourceIndex, length);
    }

    public static DiffResultSpan CreateDeleteSource(int sourceIndex, int length) {
      return new DiffResultSpan(DiffResultSpanStatus.DeleteSource, BAD_INDEX, sourceIndex, length);
    }

    public static DiffResultSpan CreateAddDestination(int destIndex, int length) {
      return new DiffResultSpan(DiffResultSpanStatus.AddDestination, destIndex, BAD_INDEX, length);
    }

    public void AddLength(int i) {
      _length += i;
    }

    public override string ToString() {
      return $"{_status} (Dest: {_destIndex}, Source: {_sourceIndex}) {_length}";
    } 
    #region IComparable Members

    public int CompareTo(object obj) {
      return CompareTo((DiffResultSpan)obj);
    }

    public int CompareTo(DiffResultSpan diffResultSpan) {
      return _destIndex.CompareTo(diffResultSpan._destIndex);
    }

    #endregion
  }
}
