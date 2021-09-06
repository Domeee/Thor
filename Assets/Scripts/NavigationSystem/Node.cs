using System.Collections.Generic;
using UnityEngine;

namespace NavigationSystem
{
  public class Node
  {
    public int Id { get; }
    public Vector2 Position { get; }
    public float CostSoFar { get; set; }
    public Node ConnectingCode { get; set; }

    public Node(int id, Vector2 position)
    {
      this.Id = id;
      this.Position = position;
      this.CostSoFar = Mathf.Infinity;
    }

    public IEnumerable<Vector2> GetNeighborPositions()
    {
      var northCandidatePos = new Vector2(Position.x, Position.y + 1f);
      var northEastCandidatePos = new Vector2(Position.x + 1f, Position.y + 1f);
      var northWestCandidatePos = new Vector2(Position.x - 1f, Position.y + 1f);
      var southCandidatePos = new Vector2(Position.x, Position.y - 1f);
      var southEastCandidatePos = new Vector2(Position.x + 1f, Position.y - 1f);
      var southWestCandidatePos = new Vector2(Position.x - 1f, Position.y - 1f);
      
      return new List<Vector2>{northCandidatePos, northEastCandidatePos, northWestCandidatePos, southCandidatePos, southEastCandidatePos, southWestCandidatePos};
    }
  }
}