using UnityEngine;

namespace AssemblyCSharp
{
    public class PlayerModel
    {
        public Vector2 Position;

        private TeamModel _team;

        public PlayerModel(int id)
        {
            Id = id;
        }

        [JsonSerializable]
        public int Id { get; set; }

        public GameInstanceModel GameInstance { get; set; }

        public TeamModel Team
        {
            get { return _team; }
            set
            {
                if (_team != value)
                {
                    if (_team != null && _team.Players.Contains(this))
                    {
                        _team.Players.Remove(this);
                    }
                    _team = value;
                    if (_team != null && !_team.Players.Contains(this))
                    {
                        _team.Players.Add(this);
                    }
                }
            }
        }

        public string ToJsonString()
        {
            string json = "{ id : @id, Position : { x : @x, y : @y }}";
            json = json.Replace("@id", Id.ToString());
            json = json.Replace("@x", Position.x.ToString());
            json = json.Replace("@y", Position.y.ToString());
            return json;
        }
    }
}