using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace AssemblyCSharp
{
    public class GameInstanceModel
    {
        public int Id { get; set; }
        public MapModel Map{ get; set;}

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

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

            if (Map != null) {
                var jsonMap = new JSONClass();
                jsonMap["Id"].AsInt = Map.Id;
                jsonMap["Zoom"].AsInt = Map.Zoom;

                var jsonLatLon = new JSONClass();
                jsonLatLon["x"].AsFloat = Map.LatLon.x;
                jsonLatLon["y"].AsFloat = Map.LatLon.y;
                jsonMap.Add("LatLon", jsonLatLon);

                var jsonSize = new JSONClass();
                jsonSize["x"].AsFloat = Map.Size.x;
                jsonSize["y"].AsFloat = Map.Size.y;
                jsonMap.Add("Size", jsonSize);

                jsonGame.Add("Map", jsonMap);
            }

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

            jsonGame["StartDate"] = StartDate.HasValue ? StartDate.Value.ToString() : "";
            jsonGame["EndDate"] = EndDate.HasValue ? EndDate.Value.ToString() : "";
            
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

            var jsonMap = jsonGame["Map"].AsObject;
            if (jsonMap != null) {
                var mapId = jsonMap["Id"].AsInt;
                var mapZoom = jsonMap["Zoom"].AsInt;
                var jsonLatLon = jsonMap["LatLon"].AsObject;
                float latLonX = jsonLatLon["x"].AsFloat;
                float latLonY = jsonLatLon["y"].AsFloat;
                var jsonSize = jsonMap["Size"].AsObject;
                float sizeX = jsonSize["x"].AsFloat;
                float sizeY = jsonSize["y"].AsFloat;
                game.Map = new MapModel { Id = mapId, Zoom = mapZoom, LatLon = new Vector2 { x = latLonX, y = latLonY }, Size = new Vector2 { x = sizeX, y = sizeY } };
            }

            if (jsonGame["StartDate"] != null && jsonGame["StartDate"].Value != "null")
            {
                DateTime startDate = DateTime.ParseExact(jsonGame["StartDate"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                game.StartDate = startDate;
            }

            if (jsonGame["EndDate"] != null && jsonGame["EndDate"].Value != "null")
            {
                DateTime endDate = DateTime.ParseExact(jsonGame["EndDate"].Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                game.EndDate = endDate;
            }

            return game;
        }

    }
}

