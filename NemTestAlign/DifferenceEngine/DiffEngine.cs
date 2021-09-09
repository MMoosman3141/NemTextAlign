using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NemTextAlign.DifferenceEngine {
  public class DiffEngine<T> {
    private IEnumerable<T> _source = null;
    private IEnumerable<T> _dest = null;
    private List<DiffResultSpan> _matchList = null;
    private DiffStateList _stateList = null;
    private DiffEngineLevel _level = DiffEngineLevel.FastImperfect;
    private IComparer<T> _comparer = Comparer<T>.Default;

    public List<DiffResultSpan> ProcessDiff(IEnumerable<T> source, IEnumerable<T> destination, IComparer<T> comparer) {
      _comparer = comparer;
      return ProcessDiff(source, destination);
    }

    public List<DiffResultSpan> ProcessDiff(IEnumerable<T> source, IEnumerable<T> destination, DiffEngineLevel level) {
      _level = level;
      return ProcessDiff(source, destination);
    }

    public List<DiffResultSpan> ProcessDiff(IEnumerable<T> source, IEnumerable<T> destination, DiffEngineLevel level, IComparer<T> comparer) {
      _level = level;
      _comparer = comparer;
      return ProcessDiff(source, destination, comparer);
    }

    public List<DiffResultSpan> ProcessDiff(IEnumerable<T> source, IEnumerable<T> destination) {
      _source = source;
      _dest = destination;
      _matchList = new List<DiffResultSpan>();

      int scount = _source.Count();
      int dcount = _dest.Count();

      if (dcount > 0 && scount > 0) {
        _stateList = new DiffStateList(dcount);
        ProcessRange(0, dcount - 1, 0, scount - 1);
      }

      return DiffReport();
    }

    private List<DiffResultSpan> DiffReport() {
      List<DiffResultSpan> retval = new List<DiffResultSpan>();
      int dcount = _dest.Count();
      int scount = _source.Count();

      //Deal with the special case of empty files
      if (dcount == 0) {
        if (scount > 0) {
          retval.Add(DiffResultSpan.CreateDeleteSource(0, scount));
        }
        return retval;
      } else if (scount == 0) {
        retval.Add(DiffResultSpan.CreateAddDestination(0, dcount));
        return retval;
      }

      _matchList.Sort();
      int curDest = 0;
      int curSource = 0;
      DiffResultSpan last = null;

      //Process each match record
      foreach (DiffResultSpan drs in _matchList) {
        if (!AddChanges(retval, curDest, drs.DestIndex, curSource, drs.SourceIndex) && last != null) {
          last.AddLength(drs.Length);
        } else {
          retval.Add(drs);
          last = drs; //Adjusted: Moved from outside the if/else to inside the else
        }
        curDest = drs.DestIndex + drs.Length;
        curSource = drs.SourceIndex + drs.Length;
      }

      //Process any tail end data
      AddChanges(retval, curDest, dcount, curSource, scount);

      return retval;
    }

    private void ProcessRange(int destStart, int destEnd, int sourceStart, int sourceEnd) {
      int curBestIndex = -1;
      int curBestLength = -1;
      DiffState bestItem = null;

      for (int destIndex = destStart; destIndex <= destEnd; destIndex++) {
        int maxPossibleDestLength = (destEnd - destIndex) + 1;
        if (maxPossibleDestLength <= curBestLength) {
          //we won't find a longer one even if we looked
          break;
        }

        DiffState curItem = _stateList.GetByIndex(destIndex);

        if (!curItem.HasValidLength(sourceStart, sourceEnd, maxPossibleDestLength)) {
          //recalc new best length since it isn't valid or has never been done.
          GetLongestSourceMatch(curItem, destIndex, destEnd, sourceStart, sourceEnd);
        }

        if (curItem.Status == DiffStatus.Matched) {
          if (curItem.Length > curBestLength) {
            //this is longest match so far
            curBestIndex = destIndex;
            curBestLength = curItem.Length;
            bestItem = curItem;
          }

          switch (_level) {
            case DiffEngineLevel.FastImperfect:
              //Jump over the match 
              destIndex += curItem.Length - 1;
              break;
            case DiffEngineLevel.Medium:
              if (curItem.Length > curBestLength) {
                //Jump over the match 
                destIndex += curItem.Length - 1;
              }
              break;
          }
        }
      }
      //Check if there are more matches in this span
      if (curBestIndex >= 0) {
        int sourceIndex = bestItem.StartIndex;
        _matchList.Add(DiffResultSpan.CreateNoChange(curBestIndex, sourceIndex, curBestLength));
        if (destStart < curBestIndex) {
          //Still have more lower destination data
          if (sourceStart < sourceIndex) {
            //Still have more lower source data
            // Recursive call to process lower indexes
            ProcessRange(destStart, curBestIndex - 1, sourceStart, sourceIndex - 1);
          }
        }
        int upperDestStart = curBestIndex + curBestLength;
        int upperSourceStart = sourceIndex + curBestLength;
        if (destEnd >= upperDestStart) {  //Adjusted: from > to >=
          //we still have more upper dest data
          if (sourceEnd >= upperSourceStart) { //Adjusted: from > to >=
            //set still have more upper source data
            // Recursive call to process upper indexes
            ProcessRange(upperDestStart, destEnd, upperSourceStart, sourceEnd);
          }
        }
      }
    }

    private void GetLongestSourceMatch(DiffState curItem, int destIndex, int destEnd, int sourceStart, int sourceEnd) {
      int maxDestLength = (destEnd - destIndex) + 1;
      int curBestLength = 0;
      int curBestIndex = -1;

      for (int sourceIndex = sourceStart; sourceIndex <= sourceEnd; sourceIndex++) {
        int maxLength = Math.Min(maxDestLength, (sourceEnd - sourceIndex) + 1);
        if (maxLength <= curBestLength) {
          //No chance to find a longer one any more
          break;
        }

        int curLength = GetSourceMatchLength(destIndex, sourceIndex, maxLength);
        if (curLength > curBestLength) {
          //This is the best match so far
          curBestIndex = sourceIndex;
          curBestLength = curLength;
        }

        //jump over the match
        sourceIndex += curBestLength;
      }

      if (curBestIndex == -1) {
        curItem.SetNoMatch();
      } else {
        curItem.SetMatch(curBestIndex, curBestLength);
      }

    }

    private int GetSourceMatchLength(int destIndex, int sourceIndex, int maxLength) {
      int matchCount;
      for (matchCount = 0; matchCount < maxLength; matchCount++) {
        if (_comparer.Compare(_dest.ElementAt(destIndex + matchCount), _source.ElementAt(sourceIndex + matchCount)) != 0) {
          break;
        }
      }
      return matchCount;
    }

    private bool AddChanges(List<DiffResultSpan> report, int curDest, int nextDest, int curSource, int nextSource) {
      bool retval = false;
      int diffDest = nextDest - curDest;
      int diffSource = nextSource - curSource;
      if (diffDest > 0) {
        if (diffSource > 0) {
          int minDiff = Math.Min(diffDest, diffSource);
          report.Add(DiffResultSpan.CreateReplace(curDest, curSource, minDiff));
          if (diffDest > diffSource) {
            curDest += minDiff;
            report.Add(DiffResultSpan.CreateAddDestination(curDest, diffDest - diffSource));
          } else {
            if (diffSource > diffDest) {
              curSource += minDiff;
              report.Add(DiffResultSpan.CreateDeleteSource(curSource, diffSource - diffDest));
            }
          }
        } else {
          report.Add(DiffResultSpan.CreateAddDestination(curDest, diffDest));
        }
        retval = true;
      } else {
        if (diffSource > 0) {
          report.Add(DiffResultSpan.CreateDeleteSource(curSource, diffSource));
          retval = true;
        }
      }
      return retval;
    }

  }
}
