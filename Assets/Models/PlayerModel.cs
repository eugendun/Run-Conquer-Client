using UnityEngine;
using System;
using SimpleJSON;
using System.IO;
using System.Text;

namespace AssemblyCSharp
{
    public class PlayerModel
    {
        public int Id;
        public Vector2 Position;
        public GameInstanceModel GameInstance {get;set;}       

        private TeamModel _team;
        public TeamModel Team {
            get{ return _team; } 
            set{
                if(_team != value){
                    if (_team != null && _team.Players.Contains(this)){
                        _team.Players.Remove(this);
                    }
                    _team = value;
                    if(_team != null && !_team.Players.Contains(this)){
                        _team.Players.Add(this);
                    }
                }
            }
        }

        public PlayerModel(int id)
        {
            Id = id;
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

