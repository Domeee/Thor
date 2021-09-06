using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace NavigationSystem
{
    public class NavigationSystem : MonoBehaviour
    {
        [SerializeField] Tilemap ground;
        [SerializeField] Tilemap obstacles;
        [SerializeField] Grid grid;
        private Vector2[] _tilePositions;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var tilePositions = GetTilePositions();

            foreach (var position in tilePositions)
            {
                Gizmos.DrawSphere(position, 0.05f);
            }
        }

        public Vector2 GetNearestNeighbor(Vector2 currentPos)
        {
            // TODO: respect direction to target
            Vector2 startingPosition = Vector2.zero;
            float minDist = Mathf.Infinity;
            var tilePositions = GetTilePositions();

            foreach (var tilePosition in tilePositions)
            {
                // TODO: use sqred len fun to improve performance
                float dist = Vector2.Distance(currentPos, tilePosition);

                if (dist < minDist)
                {
                    startingPosition = tilePosition;
                    minDist = dist;
                }
            }

            return startingPosition;
        }

        public IEnumerable<Vector2> GetShortestPath(Vector2 startPosition, Vector2 endPosition)
        {
            var tilePositions = GetTilePositions();
            var graph = new Graph(tilePositions);

            var startNode = graph.FindNode(startPosition);
            var endNode = graph.FindNode(endPosition);
            
            var nodePath = Dijkstra.FindPath(graph, startNode, endNode);
            var vectorPath = nodePath.Select(node => node.Position);
            return vectorPath;
        }

        private Vector2[] GetTilePositions()
        {
            if (ground is null || grid is null) return null;
            
            return _tilePositions ?? GetTilePositionsInternal();
        }

        private Vector2[] GetTilePositionsInternal()
        {
            var tilePositions = new List<Vector2>();
            var obstaclePositions = new List<Vector2>();

            foreach (var position in obstacles.cellBounds.allPositionsWithin)
            {
                if (!obstacles.HasTile(position)) continue;

                Vector3 positionV3 = position;
                obstaclePositions.Add(positionV3);
            }

            foreach (var position in ground.cellBounds.allPositionsWithin)
            {
                Vector3 positionV3 = position;
                if (!ground.HasTile(position) || obstaclePositions.Contains(positionV3)) continue;

                var worldPos = grid.GetCellCenterWorld(position);
                // tilemap height -> y + 0.5
                var tilePos = new Vector2(worldPos.x, worldPos.y);

                tilePositions.Add(tilePos);
            }

            _tilePositions = tilePositions.ToArray();
            return _tilePositions;
        }
    }
}