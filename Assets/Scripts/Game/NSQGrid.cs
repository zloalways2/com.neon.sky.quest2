using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NSQ
{
    public partial class NSQGrid : MonoBehaviour
    {
        [SerializeField] private TMP_Text _NSQScore;
        public NSQGameOver NSQGameOver;
        public float NSQcellSize;
        public List<NSQTile> NSQTilesPrefab = new List<NSQTile>();
        [Space] public AudioClip _NSQDestroyClip;
        public AudioSource _NSQSfx;
        private Sequence _nsqSequence;
        private Dictionary<Vector2Int, NSQTile> _nsqActiveTiles = new Dictionary<Vector2Int, NSQTile>();
        private Dictionary<Vector2Int, Vector3> _nsqGridPositions = new Dictionary<Vector2Int, Vector3>();
        private int _nsqScore = 0;
        public Vector2Int NSQSize;
        public Vector2 NSQspacing;

        private void Awake()
        {
            _NSQScore.text = $"{_nsqScore}";

            var offset = new Vector2((NSQSize.x - 1) * NSQspacing.x / 2, (NSQSize.y - 1) * NSQspacing.y / 2);

            var tilesForSession = new List<NSQTile>();

            while (tilesForSession.Count < 3)
            {
                var tilePrefab = NSQTilesPrefab[Random.Range(0, NSQTilesPrefab.Count)];
                if (!tilesForSession.Contains(tilePrefab))
                    tilesForSession.Add(tilePrefab);
            }

            for (var x = 0; x < NSQSize.x; x++)
            {
                for (var y = 0; y < NSQSize.y; y++)
                {
                    var position = transform.position +
                                   new Vector3(x * NSQspacing.x - offset.x, y * NSQspacing.y - offset.y, 0f);

                    var nsqTile = Instantiate(tilesForSession[Random.Range(0, tilesForSession.Count)], position,
                        Quaternion.identity, transform);
                    var inGridPos = new Vector2Int(x, y);
                    nsqTile.NSQPosition = inGridPos;
                    nsqTile.transform.localScale = Vector3.one * NSQcellSize;

                    nsqTile.NSQOnTileClicked += NSQOnTileClicked;

                    _nsqActiveTiles.Add(inGridPos, nsqTile);
                    _nsqGridPositions.Add(inGridPos, position);
                }
            }
        }

        private void NSQOnTileClicked(NSQTile nsqTile)
        {
            var neighbors = new HashSet<NSQTile>();

            NSQCheckAdjacentBalls(nsqTile.NSQPosition, nsqTile.NSQColor, ref neighbors);

            if (neighbors.Count < 2)
                return;

            _nsqSequence?.Kill();
            _nsqSequence = DOTween.Sequence();

            _nsqScore += neighbors.Count * 7 + (neighbors.Count - 2) * 3;

            foreach (var neighbor in neighbors)
            {
                neighbor.NSQIsActive = false;

                _nsqActiveTiles[neighbor.NSQPosition] = null;

                _nsqSequence.Join
                (neighbor.transform
                    .DOPunchScale(neighbor.transform.localScale + Vector3.one * 0.1f, 0.2f).SetEase(Ease.Flash)
                    .OnComplete(() =>
                    {
                        neighbor.NSQDestroy();
                        _NSQScore.text = $"{_nsqScore}";
                        Destroy(neighbor.gameObject);
                    }));
            }

            _NSQSfx.NSQPlayOneShot(_NSQDestroyClip, true);

            _nsqSequence.AppendCallback(() =>
            {
                NSQShiftBallsDown();
                NSQShiftBallsSideways();
                NSQCheckAvailableMoves();
            });
        }

        private void NSQCheckAvailableMoves()
        {
            foreach (var tile in _nsqActiveTiles)
            {
                if (tile.Value == null)
                    continue;

                var neighbors = new HashSet<NSQTile>();

                NSQCheckAdjacentBalls(tile.Key, tile.Value.NSQColor, ref neighbors);

                if (neighbors.Count < 2)
                    continue;

                return;
            }

            NSQGameOver.NSQWin(_nsqScore);
        }

        private void NSQCheckAdjacentBalls(Vector2Int nsqgridPosition, ENSQColor NSQcolor,
            ref HashSet<NSQTile> NSQdimondsToDestroy)
        {
            if (nsqgridPosition.x < 0 || nsqgridPosition.x >= NSQSize.x || nsqgridPosition.y < 0 ||
                nsqgridPosition.y >= NSQSize.y || _nsqActiveTiles[nsqgridPosition] == null ||
                _nsqActiveTiles[nsqgridPosition].NSQColor != NSQcolor ||
                NSQdimondsToDestroy.Contains(_nsqActiveTiles[nsqgridPosition]))
                return;
            NSQdimondsToDestroy.Add(_nsqActiveTiles[nsqgridPosition]);
            var x = nsqgridPosition.x;
            var y = nsqgridPosition.y;
            NSQCheckAdjacentBalls(new Vector2Int(x + 1, y), NSQcolor, ref NSQdimondsToDestroy);
            NSQCheckAdjacentBalls(new Vector2Int(x - 1, y), NSQcolor, ref NSQdimondsToDestroy);
            NSQCheckAdjacentBalls(new Vector2Int(x, y + 1), NSQcolor, ref NSQdimondsToDestroy);
            NSQCheckAdjacentBalls(new Vector2Int(x, y - 1), NSQcolor, ref NSQdimondsToDestroy);
        }

        private void NSQShiftBallsDown()
        {
            for (var x = 0; x < NSQSize.x; x++)
            {
                for (var y = 0; y < NSQSize.y; y++)
                {
                    var xy = new Vector2Int(x, y);

                    if (_nsqActiveTiles[xy] == null)
                    {
                        for (var i = y + 1; i < NSQSize.y; i++)
                        {
                            var xi = new Vector2Int(x, i);

                            if (_nsqActiveTiles[xi] == null) 
                                continue;
                            
                            _nsqActiveTiles[xy] = _nsqActiveTiles[xi];
                            _nsqActiveTiles[xi] = null;
                            _nsqActiveTiles[xy].transform.DOMove(_nsqGridPositions[xy], 0.1f).SetEase(Ease.Linear);
                            _nsqActiveTiles[xy].NSQPosition = xy;
                            break;
                        }
                    }
                }
            }
        }
    }
}