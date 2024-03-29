using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    public class TeamModel
    {
		public static int teamIdCounter = 0;

        public int Id;
		public int internalId;
        public string Color;						// not used yet
        public string Name;							// not used yet
		public ICollection<PlayerModel> Players;	// not used yet

        private GameInstanceModel _gameInstance;
        public GameInstanceModel GameInstance{
            get{ return _gameInstance; }
            set{
                if(_gameInstance != value){
                    if(_gameInstance != null && _gameInstance.Teams.Contains(this)){
                        _gameInstance.Teams.Remove(this);
                    }
                    _gameInstance = value;
                    if(_gameInstance != null && !_gameInstance.Teams.Contains(this)){
                        _gameInstance.Teams.Add(this);
                    }
                }
            }
        }

        public TeamModel(int id)
        {
            Id = id;
            var obsPlayerCollection = new ObservableCollection<PlayerModel>(new List<PlayerModel>());
            obsPlayerCollection.CollectionChanged += HandleCollectionChanged;
            Players = obsPlayerCollection;

			internalId = teamIdCounter;
			teamIdCounter++;
        }

        void HandleCollectionChanged(object sender, ObservableCollectionEventArgs e)
        {
            if (e.Action == ObservableCollectionActions.Add)
            {
                foreach (PlayerModel player in e.AddedItems){
                    if(player.Team != this){
                        var oldTeam = player.Team;
                        player.Team = this;
                        if(oldTeam != null && oldTeam.Players.Contains(player)){
                            oldTeam.Players.Remove(player);
                        }
                    }
                }
            }
            if(e.Action == ObservableCollectionActions.Remove || e.Action == ObservableCollectionActions.Clear){
                foreach(PlayerModel player in e.RemovedItems){
                    player.Team = null;
                }
            }
        }
    }
}

