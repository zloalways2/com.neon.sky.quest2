using DG.Tweening;
using UnityEngine;

namespace NSQ
{
    public partial class NSQGrid
    {
        private void NSQShiftBallsSideways()
        {
            for (var x = 0; x < NSQSize.x - 1; x++)
            {
                var hasBallsAbove = true;

                for (var y = 0; y < NSQSize.y; y++)
                {
                    var xy = new Vector2Int(x, y);

                    if (_nsqActiveTiles[xy] == null)
                        continue;

                    hasBallsAbove = false;

                    break;
                }

                if (!hasBallsAbove)
                    continue;

                if (x == 0 || x > NSQSize.x)
                    continue;

                var moved = false;

                for (var y = 0; y < NSQSize.y; y++)
                {
                    var xy = new Vector2Int(x, y);

                    if (x > NSQSize.x / 2)
                        xy.x += 1;
                    else
                        xy.x -= 1;

                    var xyNext = new Vector2Int(x, y);


                    if (_nsqActiveTiles[xy] != null)
                    {
                        _nsqActiveTiles[xyNext] = _nsqActiveTiles[xy];
                        _nsqActiveTiles[xy] = null;
                        _nsqActiveTiles[xyNext].transform.DOMove(_nsqGridPositions[xyNext], 0.1f).SetEase(Ease.Linear);
                        _nsqActiveTiles[xyNext].NSQPosition = xyNext;

                        moved = true;
                    }
                }

                if (!moved)
                    continue;

                NSQShiftBallsSideways();
                return;
            }
        }
    }
}