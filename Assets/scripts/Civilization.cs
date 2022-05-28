using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Civilization {
	static Dictionary<string, Civilization> civilizations = new Dictionary<string, Civilization>();
	public string name { get; protected set; }
	public string adjective { get; protected set; }
	public string empireName { get; protected set; }

	public Dictionary<ResourceType, double> demand = new Dictionary<ResourceType, double>();
	public ResourceType consumedResource { get; protected set; }
	public double consumedAmount { get; protected set; } //per pop
	List<string> tiles = new List<string>();
	public graphics graphics { get; protected set; }
	public player civLeader { get; protected set; }
	public Color tintColor { get; protected set; }

	public static Civilization getCivilization(string name) {
		if (!_initialiazed) {
			initCivilizations();
		}
		if (civilizations.ContainsKey(name)) {
			return civilizations[name];
		}
		return null;
	}

	static JArray CivilizationsJson() {
		JArray civs = new JArray();
		JArray tiles = new JArray();
		///////////////////////////////////////////////////////////////////////////////////////////
		//RACE								HUMAN
		///////////////////////////////////////////////////////////////////////////////////////////
		JObject human = new JObject();
		human["type"] = "race";
		human["name"] = "human";
		human["consumption"] = new JArray("food", "2");
		human["demand"] = new JArray("materia", "0.1", "goods", "0.1");
		human["graphics"] = "human";
		human["tiles"] = new JArray("Terra", "neutral");

		//JArray tmp = new JArray();
		//human["initialResources"] = tmp;
		//tmp.Add(new JArray("money", "10", "human"));
		//tmp.Add(new JArray("science", "5", "human"));
		civs.Add(human);

		///////////////////////////////////////////////////////////////////////////////////////////
		//PLANET							TERRA
		///////////////////////////////////////////////////////////////////////////////////////////
		JObject Terra = new JObject();
		Terra["name"] = "Terra";
		Terra["owner"] = "Human";
		Terra["texture"] = "Terra";
		Terra["wormholes"] = new JArray(1, 6);
		Terra["population"] = 1;
		JArray tmp = new JArray();
		Terra["deposits"] = tmp;
		tmp.Add(new JArray("food", "2"));

		///////////////////////////////////////////////////////////////////////////////////////////
		//RACE								ROMOTO
		///////////////////////////////////////////////////////////////////////////////////////////
		JObject romoto = new JObject();
		romoto["type"] = "race";
		romoto["name"] = "Romoto";
		romoto["consumption"] = new JArray("materia", "2");
		romoto["demand"] = new JArray("science", "0.2");
		romoto["graphics"] = "romoto";
		romoto["tiles"] = new JArray("Root", "Romoto Colony", "neutral");

		tmp = new JArray();
		romoto["initialResources"] = tmp;
		tmp.Add(new JArray("money", "10", "Romoto"));
		tmp.Add(new JArray("science", "5", "Romoto"));
		civs.Add(romoto);
		///////////////////////////////////////////////////////////////////////////////////////////
		//RACE								SANDPEOPLE
		///////////////////////////////////////////////////////////////////////////////////////////
		JObject sandpeople = new JObject();
		sandpeople["name"] = "Sandpeople";
		sandpeople["consumption"] = new JArray("food", "2");
		sandpeople["needs"] = new JArray("materia", "0.5");
		sandpeople["graphics"] = "sandpeople";
		sandpeople["tiles"] = new JArray("Haren");
		tmp = new JArray();
		sandpeople["initialResources"] = tmp;
		tmp.Add(new JArray("money", "10", "Romoto"));
		tmp.Add(new JArray("food", "5", "Romoto"));
		civs.Add(sandpeople);
		///////////////////////////////////////////////////////////////////////////////////////////
		//World						HAREN
		///////////////////////////////////////////////////////////////////////////////////////////
		JObject Haren = new JObject();
		Haren["name"] = "Haren";
		Haren["ownner"] = "Sandpeople";
		Haren["texture"] = "haren";
		Haren["wormholes"] = new JArray(1, 6);
		Haren["population"] = 6;
		tmp = new JArray();
		Haren["deposits"] = tmp;
		tmp.Add(new JArray("materia", "4"));
		tmp.Add(new JArray("food", "4"));

		tmp = new JArray();
		Haren["initialResources"] = tmp;
		tmp.Add(new JArray("materia", "3"));
		tmp.Add(new JArray("science", "2"));

		tmp = new JArray();
		Haren["initialBuildings"] = tmp;
		tmp.Add(new JArray("materia", "small", "Sandpeople", 1)); //last one is initial amount
		tmp.Add(new JArray("materia", "Large", "Sandpeople", 1));
		tmp.Add(new JArray("science", "small", "Sandpeople", 1));

		tmp = new JArray();
		Haren["initialUnits"] = tmp;
		tmp.Add(new JArray("large", "Sandpeople", 2));
		//tmp.Add(new JArray("small", "Sandpeople", 2));
		tiles.Add(Haren);

		///////////////////////////////////////////////////////////////////////////////////////////
		//RACE								BLOBS
		///////////////////////////////////////////////////////////////////////////////////////////
		JObject blobs = new JObject();
		blobs["name"] = "Blobs";
		romoto["type"] = "race";
		blobs["consumption"] = new JArray("food", "3");
		blobs["graphics"] = "blobs";
		blobs["tiles"] = new JArray("Dulcis");
		tmp = new JArray();
		blobs["initialResources"] = tmp;
		tmp.Add(new JArray("money", "0", "Romoto"));
		tmp.Add(new JArray("materia", "5", "Romoto"));
		civs.Add(blobs);


		///////////////////////////////////////////////////////////////////////////////////////////
		//World						DULCIS
		///////////////////////////////////////////////////////////////////////////////////////////
		// scp 999 population
		JObject Dulcis = new JObject();
		Dulcis["name"] = "Dulcis";
		Dulcis["ownner"] = "Blobs";
		Dulcis["texture"] = "Dulcis";
		Dulcis["wormholes"] = new JArray(1, 2, 3, 4, 5, 6);
		Dulcis["population"] = 8;
		tmp = new JArray();
		Dulcis["deposits"] = tmp;
		tmp.Add(new JArray("materia", "10"));
		tmp.Add(new JArray("food", "10"));

		tmp = new JArray();
		Dulcis["initialResources"] = tmp;
		tmp.Add(new JArray("food", "20"));

		tmp = new JArray();
		Dulcis["initialBuilding"] = tmp;
		tmp.Add(new JArray("food", "small", "blobs", 4)); //last one is initial amount
		tmp.Add(new JArray("materia", "Large", "blobs", 2));

		tmp = new JArray();
		Dulcis["initialUnits"] = tmp;
		tmp.Add(new JArray("large", "Ancient", 1));
		tmp.Add(new JArray("medium", "Ancient", 3));
		tmp.Add(new JArray("small", "Ancient", 2));
		tiles.Add(Dulcis);
		///////////////////////////////////////////////////////////////////////////////////////////
		//RACE								SHADOWS (nosedu)
		///////////////////////////////////////////////////////////////////////////////////////////
		JObject shadows = new JObject();
		romoto["type"] = "race";
		shadows["name"] = "Shadows";
		shadows["consumption"] = new JArray("food", "1");
		shadows["demand"] = new JArray("science", "0.1");
		shadows["graphics"] = "shadows";
		shadows["tiles"] = new JArray("Nosedu", "Litless");
		tmp = new JArray();
		shadows["initialResources"] = tmp;
		tmp.Add(new JArray("money", "50"));
		tmp.Add(new JArray("science", "0"));
		civs.Add(shadows);


		JObject sample = new JObject();
		sample["name"] = "Steam";
		sample["consumption"] = new JArray("materia", "2");
		sample["graphics"] = "romoto";
		sample["tiles"] = new JArray("Root", "Romoto Colony", "neutral");
		sample["initialResources"] = new JArray();
		sample["initialResources"][0] = new JArray("money", "10", "Romoto");
		sample["initialResources"][1] = new JArray("science", "5", "Romoto");
		civs.Add(sample);
		/*
		JObject sample = new JObject();
		sample["name"] = "Romoto";
		sample["consumption"] = new JArray("materia", "2");
		sample["graphics"] = "romoto";
		sample["tiles"] = new JArray("Root", "Romoto Colony", "neutral");
		sample["initialResources"] = new JArray();
		sample["initialResources"][0] = new JArray("money", "10", "Romoto");
		sample["initialResources"][1] = new JArray("science", "5", "Romoto");
		civs.Add(sample);
		*/
		return civs;
	}
	static bool _initialiazed = false;
	static void initCivilizations() {

		JArray arr = Civilization.CivilizationsJson();

		JArray tmpTiles = new JArray(); //could be null
		Civilization c = null;
		for (int i = 0; i < arr.Count; i++) {
			JToken civ = arr[i];
			if (civ["type"] != null) {

				c = new Civilization();
				c.name = civ["name"].ToString();
				c.adjective = civ["adjective"].ToString();
				if (c.adjective == null) c.adjective = c.name;
				c.empireName = civ["adjective"].ToString();
				if (c.empireName == null) c.empireName = c.adjective;

				c.consumedAmount = Double.Parse(civ["consumption"][1].ToString());
				c.consumedResource = (ResourceType)Enum.Parse(typeof(ResourceType), civ["consumption"][0].ToString());
				JArray demands = civ["demands"] as JArray;

				for (int j = 0; j < demands.Count; j += 2) {
					c.demand.Add((ResourceType)Enum.Parse(typeof(ResourceType), civ["consumption"][j].ToString()), Double.Parse(civ["consumption"][j + 1].ToString()));
				}
				c.graphics = new graphics(c.name);
				tmpTiles = (JArray)civ["tiles"];
				JArray initialResources = (JArray)civ["initialResources"];
				Color tint = Color.gray;
				if (ColorUtility.TryParseHtmlString(civ["color"].ToString(), out tint)) {
					c.tintColor = tint;
				}
				c.civLeader = playerManager.makeCivilizationLeader(c);
			}
			else //its a tile
			{
				for (; i < arr.Count; i++) {
					tile t = tile.makeTile(c, arr[i]);
				}
			}
		}
	}
}