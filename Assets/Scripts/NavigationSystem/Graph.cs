using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NavigationSystem
{
  public class Graph
  {
    private readonly Dictionary<Vector2, Node> _nodes;

    public Graph(Vector2[] positions)
    {
      _nodes = new Dictionary<Vector2, Node>();

      for (int i = 0; i < positions.Count(); i++)
      {
        var position = positions[i];
        var value = new Node(i, position);
        _nodes.Add(position, value);
      }
    }

    public Node FindNode(Vector2 position)
    {
      return _nodes.ContainsKey(position) ? _nodes[position] : null;
    }

    public IEnumerable<Node> FindNeighbor(Node node)
    {
      var neighbors = new List<Node>();
      var neighborCandidatePositions = node.GetNeighborPositions();

      foreach (var candidatePosition in neighborCandidatePositions)
      {
        var neighbor = FindNode(candidatePosition);
        if (neighbor != null) neighbors.Add(neighbor);
      }

      return neighbors;
    }
  }
}