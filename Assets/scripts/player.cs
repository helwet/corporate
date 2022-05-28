using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	public class player {
		static List<player> allPlayers = new List<player>();
		static List<player> humanPlayers = new List<player>();
		static List<player> civilizations = new List<player>();
		public static readonly player neutralPlayer = new player("neutral",false,Color.black);
		const int startingReputation = 2;
		static Dictionary<string, int> diplomaticFriendships = new Dictionary<string, int>();

		//these are added to by makeUnit, makeBuilding, makeTile
		public List<unit> myUnits = new List<unit>();
		public List<building> myBuildings = new List<building>();

		public bool friendly(player p) {
			if(p != this){
				string asd = "" + p.id + "_" + id;
				return diplomaticFriendships[asd] > 0;
			}
			return true;
		}

		internal Dictionary<tile, Dictionary<ResourceType, resourcePile>> resources = new Dictionary<tile, Dictionary<ResourceType, resourcePile>>();
		Dictionary<ResourceType,resourcePile> immaterialResources = new Dictionary<ResourceType,resourcePile>();
		Dictionary<string, int> reputations = new Dictionary<string, int>();
		//int reputation = 0;
		//int badBlood = 0;

		string displayName = "unknown_seeing_this_is_a_bug"; //player name to display ingame
		private static int idTracker = 0;
		public int id { get; private set; } //used to internally track players ex. as which one this player is in Allplayers array
		public Color playerTint{ get; private set; }
		public Color playerColour { get; private set; }
		public Sprite icon { get; private set; }
		public Civilization civ { get; private set; }


		int cardsDrawn = 1;
		int tradingPower = 2; //control how much is sold each round

		private List<tile> highlightedTiles = new List<tile>();

		public void onPressBuildAction(){
			highlightBuildableTiles(3, ResourceType.materia);
		}
		public List<tile> highlightBuildableTiles(int cost, ResourceType type){
			List<tile> highlightThese = new List<tile>();

			return highlightThese;
		}

		internal void pay(ResourceType t, int value) {

		}
		public int payFromImmaterials(ResourceType t, int value) {
			int remainder = immaterialResources[t].canSpend(value);
			if (remainder > 0) 
				return immaterialResources[t].spend(value);
			return remainder;
		}


		public void addImmaterial(ResourceType t, int value) {
			immaterialResources[t].addResources(value);
		}


		public player(string displayName, bool human,Color playerColor) {
			this.displayName = displayName;
			id = idTracker++;
			if (displayName == "") this.displayName = "anon_" + id;

			if (human) humanPlayers.Add(this);
			else civilizations.Add(this);
			allPlayers.Add(this);

			this.playerColour = playerColour;
			//= UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			playerTint = playerColour;


			immaterialResources[ResourceType.money] = resourcePile.makeResourcePile(ResourceType.money, 0, this, null);
			immaterialResources[ResourceType.science] = resourcePile.makeResourcePile(ResourceType.science, 0, this, null);
			immaterialResources[ResourceType.media] = resourcePile.makeResourcePile(ResourceType.media, 0, this, null);
			immaterialResources[ResourceType.materia] = resourcePile.makeResourcePile(ResourceType.materia, 0, this, null);
			immaterialResources[ResourceType.food] = resourcePile.makeResourcePile(ResourceType.food, 0, this, null);
			immaterialResources[ResourceType.karma] = resourcePile.makeResourcePile(ResourceType.karma, 10, this, null);
			initBuildings();
			initReputations();
			civ = Civilization.getCivilization("human");
		}
		void initReputations()
		{
			foreach(player p in civilizations){
				reputations[p.displayName] = startingReputation;
			}
		}

		public Dictionary<buildingType, Dictionary<buildingSize, int>> producedAmounts = new Dictionary<buildingType, Dictionary<buildingSize, int>>();
		public Dictionary<buildingType, Dictionary<buildingSize, List<int>>> buildAndUpgradeCosts = new Dictionary<buildingType, Dictionary<buildingSize, List<int>>>();

		void initBuildings() {
			initBuildingCosts();
			initProduced();
		}
		void initBuildingCosts() {
			buildAndUpgradeCosts[buildingType.mine] = new Dictionary<buildingSize, List<int>>();
			buildAndUpgradeCosts[buildingType.mine][buildingSize.small] = new List<int> { 3, 3, 5 };
			buildAndUpgradeCosts[buildingType.mine][buildingSize.medium] = new List<int> { 5, 0, 4 };
			buildAndUpgradeCosts[buildingType.mine][buildingSize.large] = new List<int> { 8, 0, 0 };

			buildAndUpgradeCosts[buildingType.farm] = new Dictionary<buildingSize, List<int>>();
			buildAndUpgradeCosts[buildingType.farm][buildingSize.small] = new List<int> { 2, 3, 4 };
			buildAndUpgradeCosts[buildingType.farm][buildingSize.medium] = new List<int> { 4, 0, 3 };
			buildAndUpgradeCosts[buildingType.farm][buildingSize.large] = new List<int> { 7, 0, 0 };

			buildAndUpgradeCosts[buildingType.research] = new Dictionary<buildingSize, List<int>>();
			buildAndUpgradeCosts[buildingType.research][buildingSize.small] = new List<int> { 3, 3, 12 };
			buildAndUpgradeCosts[buildingType.research][buildingSize.medium] = new List<int> { 5, 8, 12 };
			buildAndUpgradeCosts[buildingType.research][buildingSize.large] = new List<int> { 8, 8, 12 };
		}

		void initProduced() {
			producedAmounts[buildingType.mine] = new Dictionary<buildingSize, int>();
			producedAmounts[buildingType.mine][buildingSize.small] = 2;
			producedAmounts[buildingType.mine][buildingSize.medium] = 3;
			producedAmounts[buildingType.mine][buildingSize.large] = 5;

			producedAmounts[buildingType.farm] = new Dictionary<buildingSize, int>();
			producedAmounts[buildingType.farm][buildingSize.small] = 3;
			producedAmounts[buildingType.farm][buildingSize.medium] = 5;
			producedAmounts[buildingType.farm][buildingSize.large] = 8;

			producedAmounts[buildingType.research] = new Dictionary<buildingSize, int>();
			producedAmounts[buildingType.research][buildingSize.small] = 1;
			producedAmounts[buildingType.research][buildingSize.medium] = 2;
			producedAmounts[buildingType.research][buildingSize.large] = 3;
		}
		/*
		void makeIntoComputerPlayer(string name, tile home, List<tile> otherSystems = null)
		{
			displayName = name;
			home.owner = 0 ;
			foreach(tile t in otherSystems){
				t.setOwner(this);
			}
		}
		*/
				}
