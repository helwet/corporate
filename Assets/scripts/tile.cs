using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine.Tilemaps;

public class tile : Selectable {
	public static GameObject tilePrefab = GameObject.Find("tilePrefab");
	JObject tileRoot() {
		JObject Root = new JObject();
		Root["name"] = "Root";
		Root["ownner"] = "Romoto";
		Root["texture"] = "romotoHomeworld";
		Root["wormholes"] = new JArray(1, 2, 3, 4, 5, 6);
		Root["population"] = 5;
		Root["deposits"] = new JArray();

		Root["deposits"][0] = new JArray("materia", "3");
		Root["deposits"][2] = new JArray("food", "3");

		Root["initialResources"] = new JArray();
		Root["initialResources"][0] = new JArray("materia", "3");
		Root["initialResources"][2] = new JArray("science", "2");

		Root["initialBuilding"][0] = new JArray("materia", "small", "Romoto", 1); //last one is initial amount
		Root["initialBuilding"][1] = new JArray("materia", "Large", "Romoto", 1);
		Root["initialBuilding"][2] = new JArray("science", "small", "Romoto", 1);

		Root["ships"][0] = new JArray("large", "Romoto", 1);
		Root["ships"][1] = new JArray("small", "Romoto", 2);
		return Root;
	}

	JObject sampleJson() {
		JObject romotoOutpost = new JObject();
		romotoOutpost["name"] = "Romoto Outpost";
		romotoOutpost["ownner"] = "Romoto";
		romotoOutpost["texture"] = "romotoOutpost";
		romotoOutpost["wormholes"] = new JArray(1, 2, 4, 6);
		romotoOutpost["population"] = 3;
		romotoOutpost["deposits"] = new JArray();
		romotoOutpost["deposits"][0] = new JArray("materia", "3", "neutral");
		romotoOutpost["deposits"][2] = new JArray("science", "2", "Romoto");

		romotoOutpost["initialResources"] = new JArray();
		romotoOutpost["initialResources"][0] = new JArray("materia", "3", "neutral");
		romotoOutpost["initialResources"][2] = new JArray("science", "2", "Romoto");

		romotoOutpost["initialBuilding"][0] = new JArray("materia", "small", "Romoto", 2); //last one is initial amount
		romotoOutpost["initialBuilding"][2] = new JArray("science", "small", "Romoto", 1);

		romotoOutpost["ships"][0] = new JArray("large", "Romoto", 1);
		return romotoOutpost;
	}

	JObject samplesJson() {
		JObject sampleTile = new JObject();
		sampleTile["name"] = "Root";
		sampleTile["ownner"] = "Romoto";
		sampleTile["texture"] = "romotoHomeworld";
		sampleTile["wormholes"] = new JArray(1, 2, 3, 4, 5, 6);
		sampleTile["population"] = 6;
		sampleTile["deposits"] = new JArray();
		sampleTile["deposits"][0] = new JArray("materia", "3", "neutral");
		sampleTile["deposits"][2] = new JArray("science", "2", "Romoto");

		sampleTile["initialResources"] = new JArray();
		sampleTile["initialResources"][0] = new JArray("materia", "3", "neutral");
		sampleTile["initialResources"][2] = new JArray("science", "2", "Romoto");

		sampleTile["initialBuilding"][0] = new JArray("materia", "small", "Romoto", 1); //last one is initial amount
		sampleTile["initialBuilding"][1] = new JArray("materia", "Large", "Romoto", 1);
		sampleTile["initialBuilding"][2] = new JArray("science", "small", "Romoto", 1);

		sampleTile["ships"][0] = new JArray("large", "Romoto", 1);
		sampleTile["ships"][1] = new JArray("small", "Romoto", 2);
		return sampleTile;
	}
	class wormhole {
		public static GameObject wormHoleAnimation;
		bool exists = false;
		GameObject sprite = null;

		public void makeReal() {
			exists = true;
			sprite = GameObject.Instantiate(wormHoleAnimation);
		}
	}

	static Dictionary<string, tile> tilelib = new Dictionary<string, tile>();
	static Dictionary<string, tile> justRandomTiles = new Dictionary<string, tile>();
	static bool _initedTiles = false;
	public static tile getTile(string name) {
		if (!_initedTiles) {

			_initedTiles = true;
		}
		return tilelib[name];
	}

	void initTiles() {

	}

	int alignment;
	private wormhole[] wormholes = new wormhole[6];
	private GameObject myTile = null;
	player owner = player.neutralPlayer;
	public List<building> buildings = new List<building>();
	public List<unit> units = new List<unit>();
	public List<resourcePile> resources = new List<resourcePile>();

/// population
	Civilization population = null;
	int populationAmount = 0;
	void addPopulation(int amount) {
		populationAmount += amount;
		tags["populated"] = populationAmount;
	}
	//returns amount of people removed;
	int removePopulation(int amount) {
		populationAmount -= amount;
		if (populationAmount > 0) {
			return amount;
		}
		else {
			tags.Remove("populated");
			return amount - populationAmount;
		}
	}
	Vector3Int _coordinate = new Vector3Int(100, 100, 100);
	public void SetCoordinate(Vector3Int value) {
		_coordinate = value;
		Vector3 worldPos = gameManager.instance.tilemap.GetCellCenterWorld(value);
		gameObject.transform.position = worldPos;
	}
	public Vector3Int GetCoordinate(){
		return _coordinate;
	}
	string _tileName;

	public string TileName { 
	get => _tileName;
	set {
		if(_tileName != null)
			tags.Remove(_tileName);
			tags.Add(value, 1);
			_tileName = value;
		} 
	}

	public Civilization Population {
	get => population;
	set {
			if (population != null)
				tags.Remove(population.name);
			population = value;
			tags[value.name] = 1; } 
	}


	List<building> buildingsBeingDestroyed = new List<building>();
	List<unit> unitsBeingDestroyed = new List<unit>();
	List<resourcePile> resourcesUsedUp = new List<resourcePile>();

	//returns dict containing array, that shows prices of goods for this tile.
	//ex if food demand is 12 and produced is 4 this returns food:{5,5,4,4,4,3,3,3,2}
	Dictionary<ResourceType, List<int>> calculatedProjectedPrices() {
		Dictionary<ResourceType, List<int>> res = new Dictionary<ResourceType, List<int>>();
		List<int> prices = new List<int>();
		ResourceType t = population.consumedResource;
		int needed = (int)Math.Ceiling(population.consumedAmount * populationAmount);
		int sum = 0;
		foreach (building b in buildings) {
			if (b.owner == owner && b.produced == t)
				sum += b.amountProduced;
		}
		needed = needed - sum;
		for (int i = 0; i < needed; i++) {
			if (needed - 6 > 0) {
				prices.Add(resourcePile.baseCosts[t] + 3);
			}
			else if (needed - 3 > 0) {
				prices.Add(resourcePile.baseCosts[t] + 2);
			}
			else {
				prices.Add(resourcePile.baseCosts[t] + 1);
			}
		}
		prices.Add(resourcePile.baseCosts[t]);
		res[t] = prices;


		foreach (KeyValuePair<ResourceType, double> v in population.demand) {
			prices = new List<int>();
			t = v.Key;
			needed = (int)Math.Ceiling(v.Value * populationAmount);
			sum = 0;
			foreach (building b in buildings) {
				if (b.owner == owner && b.produced == t)
					sum += b.amountProduced;
			}
			needed = needed - sum;
			for (int i = 0; i < needed; i++) {
				if (needed - 6 > 0) {
					prices.Add(resourcePile.baseCosts[t] + 3);
				}
				else if (needed - 3 > 0) {
					prices.Add(resourcePile.baseCosts[t] + 2);
				}
				else {
					prices.Add(resourcePile.baseCosts[t] + 1);
				}
			}
			prices.Add(resourcePile.baseCosts[t]);
			res[t] = prices;
		}

		return res;
	}





	//int checkPopulationChange() {

	//	List<int> deaths = checkStarvation();
	//	foreach(resourcePile p in resourcesUsedUp){
	//		resources.Remove(p);
	//	}
	//	if(deaths.Count > 0){
	//	//ask for donations first player gets offered cheapest, and so on 
	//	}
	//	else{
	//		int surplus = 0;
	//		Dictionary<ResourceType, List<resourcePile>> res = new Dictionary<ResourceType, List<resourcePile>>();
	//		foreach (KeyValuePair<ResourceType, float> v in population.consumption) {
	//			int surplusNeeded = Mathf.RoundToInt((int)Math.Ceiling((double)2 * v.Value));
	//			res[v.Key] = new List<resourcePile>();
	//			foreach (resourcePile p in resources) {
	//				if (p.owner == owner && p.resourceType == v.Key) {
	//					res[v.Key].Add(p);
	//					surplus += p.amount;
	//					if (surplus > surplusNeeded) break;
	//				}
	//			}
	//			if (surplus < surplusNeeded) return 0;
	//		}

	//	}


	//	Dictionary<ResourceType, int> demand = new Dictionary<ResourceType, int>();
	//	foreach (KeyValuePair<ResourceType, double> v in population.demand) {
	//		demand[v.Key] = Mathf.RoundToInt(v.Value * populationAmount);
	//	}
	//	List<Tuple<int, int>> deaths = new List<Tuple<int, int>>(); //deaths caused and how much resource is needed to avoid dying
	//	foreach (KeyValuePair<ResourceType, float> v in population.consumption) {
	//		int sum = Mathf.RoundToInt(v.Value * populationAmount);
	//		int left = sum;
	//		int surplus = 0;
	//		foreach (resourcePile p in resources) {
	//			if (p.owner == owner && p.resourceType == v.Key) {
	//				if (left > 0)
	//					left = p.spend(left);
	//				if (p.amount == 0) {
	//					resourcesUsedUp.Add(p);
	//				}
	//				else {
	//					surplus += p.amount;
	//				}
	//			}
	//		}
	//		if (left > 0) {
	//			deaths.Add(new Tuple<int, int>((int)Math.Ceiling((double)left / v.Value), left % (int)Math.Ceiling(v.Value)));
	//		}

	//	}
	//	return deaths;
	//}
	//returns list of deaths and cost to avoid them
	List<int> checkStarvation() { //returns list of deaths and cost to avoid them
		int needed = (int)Math.Ceiling(population.consumedAmount * populationAmount);
		List<int> deaths = new List<int>(); //deaths caused followed by how much resource is needed to avoid each death
		int left = needed;
		int surplus = 0;
		foreach (resourcePile p in resources) {
			if (p.owner == owner && p.resourceType == population.consumedResource) {
				if (left > 0)
					left = p.spend(left);
				if (p.amount == 0) {
					resourcesUsedUp.Add(p);
				}
				else {
					surplus += p.amount;
				}
			}
		}
		if (left > 0) {
			deaths.Add((int)Math.Ceiling(left / population.consumedAmount));
			int cheapest = (int)Math.Ceiling(left % population.consumedAmount);
			if (cheapest != 0) {
				deaths.Add(cheapest);
				left -= cheapest;
			}
			while (left > 0) {
				int next = (int)Math.Ceiling(population.consumedAmount);
				left -= next;
				deaths.Add(next);
			}
		}
		return deaths;
	}

	private void loadData(bool[] wormholeExists, List<resourcePile> resources, List<unit> units) {
		for (int i = 0; i < 6; i++) {
			if (wormholeExists[i])
				this.wormholes[i].makeReal();
		}
		foreach (resourcePile r in resources) {
			this.resources.Add(r);
		}
		foreach (unit u in units) {
			this.units.Add(u);
		}
	}
	bool hasBeenExplored() {
		return myTile != null;
	}


	void setTexture(){
		GameObject maskedGo = transform.Find("hexImage").gameObject;
		SpriteRenderer rend = maskedGo.GetComponent<SpriteRenderer>();
		rend.sprite = ModelLibrary.getSprite(TileName);
	}
	//tiles are made on moved to coordinate 100,100,100 to await discovery and to be moved to their proper cooordinate
	public static tile makeTile(Civilization civ, JToken tile) {
		GameObject father = new GameObject();
		GameObject GO = GameObject.Instantiate(tilePrefab);
		
		tile t = GO.GetComponent<tile>();
		t.TileName = tile["name"].ToString(); //no duplicates allowed
		t.transform.SetParent(father.transform, false);
		t.population = civ;
		t.addPopulation(int.Parse(tile["population"].ToString()));
		JArray initialResources = tile["initialResources"] as JArray;
		
		for (int j = 0; j < initialResources.Count; j += 2) {
			resourcePile rp = resourcePile.makeResourcePile(
			(ResourceType)Enum.Parse(typeof(ResourceType), initialResources[j].ToString()),
			int.Parse(initialResources[j + 1].ToString()),
			civ.civLeader,
			t);
			t.resources.Add(rp);
		}
		JArray initialBuildings = tile["initialBuildings"] as JArray;
		for (int j = 0; j < initialBuildings.Count; j += 2) {
			building rp = building.makeBuilding(
			civ.civLeader,
			t,
			(buildingType)Enum.Parse(typeof(buildingType), initialBuildings[j].ToString()),
			(buildingSize)Enum.Parse(typeof(buildingSize), initialBuildings[j+1].ToString())		
			);
			
		}

		JArray initialUnits = tile["initialUnits"] as JArray;
		for (int j = 0; j < initialUnits.Count; j ++) {
			unit rp = unit.makeUnit(
			civ.civLeader,
			t,
			(unitType)Enum.Parse(typeof(unitType), initialUnits[j].ToString())
			//,(unitSize)Enum.Parse(typeof(Unitsize), initialUnits[j + 1].ToString())

			);
			civ.civLeader.myUnits.Add(rp);
		}

		t.setTexture();
		return t;
	}
	// Start is called before the first frame update
	void Start()
    {
		SetCoordinate(new Vector3Int(100,100,100));
		addTag("tile");
		addTag("selectable");
	}

    // Update is called once per frame
    void Update()
    {
		animateTile();
    }

	void onExploring(){
		tags.Remove("beingShowedToPlayer");
		
	}


	void onReturnToDrawPile(){
		tags.Remove("beingShowedToPlayer");
	}

	void whenPlayerDrawsTile() {
		tags.Add("beingShowedToPlayer",1);
	}

	void animateTile(){

	}
}

/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;

public class graphics 
{
	civilization civ;
	Dictionary<string, GameObject> ships = new Dictionary<string, GameObject>();
	Dictionary<string, GameObject> buildings = new Dictionary<string, GameObject>();
	Texture raceGraphics;
	Texture symbol;


}
	public class civilization{
	string name;

	public Dictionary<ResourceType, float> demand = new Dictionary<ResourceType, float>();
	public Dictionary<ResourceType, float> consumption = new Dictionary<ResourceType, float>();
	List<string> tiles = new List<string>();
	graphics graphics;

	JArray CivilizationsJson() {
		JArray civs = new JArray();
		JArray tiles = new JArray();
		///////////////////////////////////////////////////////////////////////////////////////////
		//RACE								ROMOTO
		///////////////////////////////////////////////////////////////////////////////////////////
		JObject romoto = new JObject();
		romoto["name"] = "Romoto";
		romoto["consumption"] = new JArray("materia", "2");
		romoto["consumption"] = new JArray("science", "0.2");
		romoto["graphics"] = "romoto";
		romoto["tiles"] = new JArray("Root", "Romoto Colony", "neutral");

		JArray tmp = new JArray();
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
		Haren["initialBuilding"] = tmp;
		tmp.Add(new JArray("materia", "small", "Sandpeople", 1)); //last one is initial amount
		tmp.Add(new JArray("materia", "Large", "Sandpeople", 1));
		tmp.Add(new JArray("science", "small", "Sandpeople", 1));

		tmp = new JArray();
		Haren["ships"] = tmp;
		tmp.Add(new JArray("large", "Sandpeople", 2));
		//tmp.Add(new JArray("small", "Sandpeople", 2));
		tiles.Add(Haren);

		///////////////////////////////////////////////////////////////////////////////////////////
		//RACE								BLOBS
		///////////////////////////////////////////////////////////////////////////////////////////
		JObject blobs = new JObject();
		blobs["name"] = "Blobs";
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
		Dulcis["ships"] = tmp;
		tmp.Add(new JArray("large", "Ancient", 1));
		tmp.Add(new JArray("medium", "Ancient", 3));
		tmp.Add(new JArray("small", "Ancient", 2));
		tiles.Add(Dulcis);
		///////////////////////////////////////////////////////////////////////////////////////////
		//RACE								SHADOWS (nosedu)
		///////////////////////////////////////////////////////////////////////////////////////////
		JObject shadows = new JObject();
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
		return civs;
	}
}
public class tile : MonoBehaviour {

	JObject tileRoot() {
		JObject Root = new JObject();
		Root["name"] = "Root";
		Root["ownner"] = "Romoto";
		Root["texture"] = "romotoHomeworld";
		Root["wormholes"] = new JArray(1, 2, 3, 4, 5, 6);
		Root["population"] = 5;
		Root["deposits"] = new JArray();

		Root["deposits"][0] = new JArray("materia", "3");
		Root["deposits"][2] = new JArray("food", "3");

		Root["initialResources"] = new JArray();
		Root["initialResources"][0] = new JArray("materia", "3");
		Root["initialResources"][2] = new JArray("science", "2");

		Root["initialBuilding"][0] = new JArray("materia", "small", "Romoto", 1); //last one is initial amount
		Root["initialBuilding"][1] = new JArray("materia", "Large", "Romoto", 1);
		Root["initialBuilding"][2] = new JArray("science", "small", "Romoto", 1);

		Root["ships"][0] = new JArray("large", "Romoto", 1);
		Root["ships"][1] = new JArray("small", "Romoto", 2);
		return Root;
	}

	JObject sampleJson() {
		JObject romotoOutpost = new JObject();
		romotoOutpost["name"] = "Romoto Outpost";
		romotoOutpost["ownner"] = "Romoto";
		romotoOutpost["texture"] = "romotoOutpost";
		romotoOutpost["wormholes"] = new JArray(1, 2, 4, 6);
		romotoOutpost["population"] = 3;
		romotoOutpost["deposits"] = new JArray();
		romotoOutpost["deposits"][0] = new JArray("materia", "3", "neutral");
		romotoOutpost["deposits"][2] = new JArray("science", "2", "Romoto");

		romotoOutpost["initialResources"] = new JArray();
		romotoOutpost["initialResources"][0] = new JArray("materia", "3", "neutral");
		romotoOutpost["initialResources"][2] = new JArray("science", "2", "Romoto");

		romotoOutpost["initialBuilding"][0] = new JArray("materia", "small", "Romoto", 2); //last one is initial amount
		romotoOutpost["initialBuilding"][2] = new JArray("science", "small", "Romoto", 1);

		romotoOutpost["ships"][0] = new JArray("large", "Romoto", 1);
		return romotoOutpost;
	}

	JObject samplesJson() {
		JObject sampleTile = new JObject();
		sampleTile["name"] = "Root";
		sampleTile["ownner"] = "Romoto";
		sampleTile["texture"] = "romotoHomeworld";
		sampleTile["wormholes"] = new JArray(1, 2, 3, 4, 5, 6);
		sampleTile["population"] = 6;
		sampleTile["deposits"] = new JArray();
		sampleTile["deposits"][0] = new JArray("materia", "3", "neutral");
		sampleTile["deposits"][2] = new JArray("science", "2", "Romoto");

		sampleTile["initialResources"] = new JArray();
		sampleTile["initialResources"][0] = new JArray("materia", "3", "neutral");
		sampleTile["initialResources"][2] = new JArray("science", "2", "Romoto");

		sampleTile["initialBuilding"][0] = new JArray("materia", "small", "Romoto", 1); //last one is initial amount
		sampleTile["initialBuilding"][1] = new JArray("materia", "Large", "Romoto", 1);
		sampleTile["initialBuilding"][2] = new JArray("science", "small", "Romoto", 1);

		sampleTile["ships"][0] = new JArray("large", "Romoto", 1);
		sampleTile["ships"][1] = new JArray("small", "Romoto", 2);
		return sampleTile;
	}
	class wormhole {
		public static GameObject wormHoleAnimation;
		bool exists = false;
		GameObject sprite = null;

		public void makeReal() {
			exists = true;
			sprite = GameObject.Instantiate(wormHoleAnimation);
		}
	}

	int alignment;
	private wormhole[] wormholes = new wormhole[6];
	private GameObject myTile = null;
	player owner = player.neutralPlayer;
	List<building> buildings = new List<building>();
	List<unit> units = new List<unit>();
	List<resourcePile> resources = new List<resourcePile>();
	civilization population = null;
	int populationAmount = 0;


	List<building> buildingsBeingDestroyed = new List<building>();
	List<unit> unitsBeingDestroyed = new List<unit>();
	List<resourcePile> resourcesUsedUp = new List<resourcePile>();

	Dictionary<ResourceType, int> calculateDemand() {
		Dictionary<ResourceType, int> res = new Dictionary<ResourceType, int>();

		foreach (KeyValuePair<ResourceType, float> v in population.demand) {
			res[v.Key] = Mathf.RoundToInt(v.Value * populationAmount);
		}

		return res;
	}
	int checkPopulationChange() {

		List<Tuple<int, int>> deaths = checkStarvation();
		foreach (resourcePile p in resourcesUsedUp) {
			resources.Remove(p);
		}
		if (deaths.Count > 0) {
			//ask for donations first player gets offered cheapest, and so on 
		}
		else {
			int surplus = 0;
			Dictionary<ResourceType, List<resourcePile>> res = new Dictionary<ResourceType, List<resourcePile>>();
			foreach (KeyValuePair<ResourceType, float> v in population.consumption) {
				int surplusNeeded = Mathf.RoundToInt((int)Math.Ceiling((double)2 * v.Value));
				res[v.Key] = new List<resourcePile>();
				foreach (resourcePile p in resources) {
					if (p.owner == owner && p.resourceType == v.Key) {
						res[v.Key].Add(p);
						surplus += p.amount;
						if (surplus > surplusNeeded) break;
					}
				}
				if (surplus < surplusNeeded) return 0;
			}

		}

		Dictionary<ResourceType, int> demand = new Dictionary<ResourceType, int>();
		foreach (KeyValuePair<ResourceType, float> v in population.demand) {
			demand[v.Key] = Mathf.RoundToInt(v.Value * populationAmount);
		}
		List<Tuple<int, int>> deaths = new List<Tuple<int, int>>(); //deaths caused and how much resource is needed to avoid dying
		foreach (KeyValuePair<ResourceType, float> v in population.consumption) {
			int sum = Mathf.RoundToInt(v.Value * populationAmount);
			int left = sum;
			int surplus = 0;
			foreach (resourcePile p in resources) {
				if (p.owner == owner && p.resourceType == v.Key) {
					if (left > 0)
						left = p.spend(left);
					if (p.amount == 0) {
						resourcesUsedUp.Add(p);
					}
					else {
						surplus += p.amount;
					}
				}
			}
			if (left > 0) {
				deaths.Add(new Tuple<int, int>((int)Math.Ceiling((double)left / v.Value), left % (int)Math.Ceiling(v.Value)));
			}

		}
		return deaths;
	}

	List<Tuple<int, int>> checkStarvation() {
		Dictionary<ResourceType, int> demand = new Dictionary<ResourceType, int>();
		foreach (KeyValuePair<ResourceType, float> v in population.demand) {
			demand[v.Key] = Mathf.RoundToInt(v.Value * populationAmount);
		}
		List<Tuple<int, int>> deaths = new List<Tuple<int, int>>(); //deaths caused and how much resource is needed to avoid dying
		foreach (KeyValuePair<ResourceType, float> v in population.consumption) {
			int sum = Mathf.RoundToInt(v.Value * populationAmount);
			int left = sum;
			int surplus = 0;
			foreach (resourcePile p in resources) {
				if (p.owner == owner && p.resourceType == v.Key) {
					if (left > 0)
						left = p.spend(left);
					if (p.amount == 0) {
						resourcesUsedUp.Add(p);
					}
					else {
						surplus += p.amount;
					}
				}
			}
			if (left > 0) {
				deaths.Add(new Tuple<int, int>((int)Math.Ceiling((double)left / v.Value), left % (int)Math.Ceiling(v.Value)));
			}

		}
		return deaths;
	}

	private void loadData(bool[] wormholeExists, List<resourcePile> resources, List<unit> units) {
		for (int i = 0; i < 6; i++) {
			if (wormholeExists[i])
				this.wormholes[i].makeReal();
		}
		foreach (resourcePile r in resources) {
			this.resources.Add(r);
		}
		foreach (unit u in units) {
			this.units.Add(u);
		}
	}
	bool hasBeenExplored() {
		return myTile != null;
	}



	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		animateTile();
	}



	void animateTile() {

	}
}






 */