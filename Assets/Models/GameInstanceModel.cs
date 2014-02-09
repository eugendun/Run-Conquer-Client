using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace AssemblyCSharp
{
    public class GameInstanceModel
    {
        [JsonSerializable]
        public int Id { get; set; }

        [JsonSerializable]
        public MapModel Map{ get; set;}

        [JsonSerializable]
        public ICollection<PlayerModel> Players{ get; set;}

        [JsonSerializable]
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


    }
}

