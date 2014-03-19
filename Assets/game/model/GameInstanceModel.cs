using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
    public class GameInstanceModel
    {
        public int Id { get; set; }
        public MapModel Map{ get; set;}
        public ICollection<PlayerModel> Players{ get; set;}
        public ICollection<TeamModel> Teams{ get; set;}

        public GameInstanceModel()
        {
            var obsPlayers = new ObservableCollection<PlayerModel>(new List<PlayerModel>());
            obsPlayers.CollectionChanged += HandlePlayerCollectionChanged;
            Players = obsPlayers;

            var obsTeamCollection = new ObservableCollection<TeamModel>(new List<TeamModel>());
            obsTeamCollection.CollectionChanged += HandleTeamCollectionChanged;
            Teams = obsTeamCollection;
        }

        void HandleTeamCollectionChanged (object sender, ObservableCollectionEventArgs e)
        {
            if(e.Action == ObservableCollectionActions.Add){
                foreach(TeamModel team in e.AddedItems) {
                    if(team.GameInstance != this){
                        var oldGameInstance = team.GameInstance;
                        team.GameInstance = this;
                        if(oldGameInstance != null && oldGameInstance.Teams.Contains(team)){
                            oldGameInstance.Teams.Remove(team);
                        }
                    }
                }
            }
            if(e.Action == ObservableCollectionActions.Remove || e.Action == ObservableCollectionActions.Clear){
                foreach(TeamModel team in e.RemovedItems) {
                    team.GameInstance = null;
                }
            }
        }

        void HandlePlayerCollectionChanged (object sender, ObservableCollectionEventArgs e)
        {
            if(e.Action == ObservableCollectionActions.Add) {
                foreach(PlayerModel player in e.AddedItems) {
                    if(player.GameInstance != this) {
                        player.GameInstance = this;
                    }
                }
            }
            if(e.Action == ObservableCollectionActions.Remove || e.Action == ObservableCollectionActions.Clear) {
                foreach (PlayerModel player in e.RemovedItems) {
                    player.GameInstance = null;
                }
            }
        }

        public string ToJson()
        {
            var jsonGame = new JSONClass();
            jsonGame["Id"].AsInt = Id;
            
            if (this.Players.Count > 0) {
                var jsonPlayers = new JSONArray();
                foreach (var player in this.Players) {
                    var jsonPlayer = new JSONClass();
                    jsonPlayer["Id"].AsInt = player.Id;

                    var jsonPosition = new JSONClass();
                    jsonPosition["x"].AsFloat = player.Position.x;
                    jsonPosition["y"].AsFloat = player.Position.y;
                    jsonPlayer.Add("Position", jsonPosition);

                    jsonPlayers.Add(jsonPlayer);
                }
                jsonGame.Add("Players", jsonPlayers); 
            }            

            return jsonGame.ToString();
        }

        public static GameInstanceModel FromJson(string json) { 
            var game = new GameInstanceModel();

            var jsonGame = JSON.Parse(json);
            game.Id = jsonGame["Id"].AsInt;

            var jsonPlayers = jsonGame["Players"].AsArray;
            foreach (JSONNode jsonPlayer in jsonPlayers) {
                int id = jsonPlayer["Id"].AsInt;
                var jsonPosition = jsonPlayer["Position"].AsObject;
                float x = jsonPosition["x"].AsFloat;
                float y = jsonPosition["y"].AsFloat;
                game.Players.Add(new PlayerModel(id) { Position = new Vector2 { x = x, y = y } });
            }
            return game;
        }

    }
}

