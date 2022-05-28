using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class graphics {

	Dictionary<string, GameObject> ships = new Dictionary<string, GameObject>();
	Dictionary<string, GameObject> buildings = new Dictionary<string, GameObject>();
	Texture2D icon;
	Texture2D symbol;
	string name;

	public graphics(string name) {
		this.name = name;
		icon = (Texture2D)Resources.Load("Resources/civs/" + name + "Icon");
		if (icon == null) {
			icon = (Texture2D)Resources.Load("Resources/placeholder");
		}
		symbol = (Texture2D)Resources.Load("Resources/civs/" + name + "Symbol");
		if (symbol == null) {
			symbol = (Texture2D)Resources.Load("Resources/placeholder");
		}
	}
}

public class ModelLibrary : MonoBehaviour
{
	static Dictionary<string, GameObject> buildingsModels = new Dictionary<string, GameObject>();
	static Dictionary<string, GameObject> unitModels = new Dictionary<string, GameObject>();

	static ModelLibrary instance;
	//returns civs shipmodel or placeholder if not found
	public static GameObject getUnitModel(Civilization civ, unitType type) {
		string s = Enum.GetName(typeof(unitType), type);

		string key = "Ship" + civ.name + char.ToUpper(s[0]) + s.Substring(1);
		if(unitModels.ContainsKey(key))
			return unitModels[key];

		key = "ShipPlaceholder" + char.ToUpper(s[0]) + s.Substring(1);
		return unitModels[key];
	
	}
	//returns civs shipmodel or placeholder if not found
	public static GameObject getBuildingModel(Civilization civ, buildingType type, buildingSize size) {
		string s = Enum.GetName(typeof(buildingType), type);

		string key = "Building" + civ.name + char.ToUpper(s[0]) + s.Substring(1);
		s = Enum.GetName(typeof(buildingSize), size);

		key = key + char.ToUpper(s[0]) + s.Substring(1);
		if (buildingsModels.ContainsKey(key))
			return buildingsModels[key];


		s = Enum.GetName(typeof(buildingType), type);
		key = "BuildingPlaceholder" + char.ToUpper(s[0]) + s.Substring(1);
		s = Enum.GetName(typeof(buildingSize), size);
		key = key + char.ToUpper(s[0]) + s.Substring(1);
		return buildingsModels[key];

	}

	public static void setShipModel(Civilization civ, buildingType type, buildingSize size, GameObject prefab) {
		string s = Enum.GetName(typeof(buildingType), type);
		string key = "";
		if (civ == null)
			key = "ShipPlaceholder" + char.ToUpper(s[0]) + s.Substring(1);
		else
			key = "Ship" + civ.name + char.ToUpper(s[0]) + s.Substring(1);
		unitModels[key] = prefab;
	}

	public static void setBuildingModel(Civilization civ, buildingType type,buildingSize size, GameObject prefab) {
		string s = Enum.GetName(typeof(buildingType), type);
		string key = "";
		if (civ == null)
			key = "BuildingPlaceholder" + char.ToUpper(s[0]) + s.Substring(1);
		else
			key = "Building" + civ.name + char.ToUpper(s[0]) + s.Substring(1);
		s = Enum.GetName(typeof(buildingSize), size);

		key = key + char.ToUpper(s[0]) + s.Substring(1);
		buildingsModels[key] = prefab;
	}
	// Start is called before the first frame update
	void Start()
    {
		instance = this;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
	Dictionary<string, Vector3> adjustment = new Dictionary<string, Vector3>();
	public static Sprite getSprite(string tileName) {

		throw new NotImplementedException();
	}
}
