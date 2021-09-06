using System.Collections.Generic;
using System.Linq;

namespace NavigationSystem
{
  public static class Dijkstra
  {
    public static IEnumerable<Node> FindPath(Graph graph, Node startNode, Node endNode)
    {
      startNode.CostSoFar = 0;
      var openNodes = new List<Node>();
      var closedNodes = new List<Node>();

      openNodes.Add(startNode);

      Node currentNode = null;

      while (openNodes.Count > 0)
      {
        var minCostSoFar = openNodes.Min(node => node.CostSoFar);
        currentNode = openNodes.First(node => node.CostSoFar == minCostSoFar);

        if (currentNode == endNode) break;

        var neighbors = graph.FindNeighbor(currentNode);
        foreach (var neighbor in neighbors)
        {
          if (closedNodes.Contains(neighbor)) continue;

          float costToNeighbor = currentNode.CostSoFar + 1;

          if (neighbor.CostSoFar <= costToNeighbor) continue;
          
          neighbor.CostSoFar = costToNeighbor;
          neighbor.ConnectingCode = currentNode;

          if (!openNodes.Contains(neighbor)) openNodes.Add(neighbor);
        }

        openNodes.Remove(currentNode);
        closedNodes.Add(currentNode);
      }

      if (currentNode != endNode) return null;

      var path = new List<Node>();
      while (currentNode != startNode)
      {
        path.Add(currentNode);
        currentNode = currentNode.ConnectingCode;
      }

      path.Reverse();
      return path;
    }
  }
}