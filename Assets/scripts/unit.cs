using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum unitType { freighter, figter, capitalship, battleship, ancientMothership, ancientDestroyer };
public class unit : entityOnTile
{


	public static unit makeUnit(player owner, tile t, unitType type) {
		GameObject father = new GameObject();
		unit u = father.AddComponent<unit>();
		t.units.Add(u);
		u.tileOn = t;
		u.owner = owner;
		u.model = GameObject.Instantiate(ModelLibrary.getUnitModel(owner.civ, type));
		u.model.transform.SetParent(u.gameObject.transform, false);

		//u.size = size;

		u.setColour();
		u.setStats();
		return u;
	}
	Renderer rend;
	Color myTint;
	//sets the tint of the building to right one
	private void setColour() {
		rend = model.GetComponent<Renderer>();
		myTint = owner.playerTint;
		rend.material.SetColor("_Color", myTint);

	}
	void setStats(){


		if (type == unitType.freighter){
			DamageMin = 0;
			damageMax = 1;
			health = 3;
			capacity = 2;
		}

		if(type == unitType.figter){
			DamageMin = 2;
			damageMax = 3;
			health = 3;
			capacity = 2;
		}

		if(type == unitType.battleship){
			DamageMin = 3;
			damageMax = 4;
			health = 5;
			capacity = 3;
		}

		if(type == unitType.capitalship){
			DamageMin = 5;
			damageMax = 5;
			health = 8;
			capacity = 5;
		}


		if(type == unitType.ancientMothership){
			DamageMin = 5;
			damageMax = 5;
			health = 20;
			capacity = 5;
		}
		/*
			unit u = unitStats[unitType.freighter] = new unit();
			u.damageMin = 0;
			u.damageMax = 1;
			u.health = 3;
			u.capacity = 2;


			u = unitStats[unitType.figter] = new unit();
			u.damageMin = 2;
			u.damageMax = 3;
			u.health = 3;
			u.capacity = 2;


			u = unitStats[unitType.battleship] = new unit();
			u.damageMin = 3;
			u.damageMax = 4;
			u.health = 5;
			u.capacity = 3;

			u = unitStats[unitType.capitalship] = new unit();
			u.damageMin = 5;
			u.damageMax = 5;
			u.health = 8;
			u.capacity = 5;


			u = unitStats[unitType.ancientMothership] = new unit();
			u.damageMin = 5;
			u.damageMax = 5;
			u.health = 20;
			u.capacity = 5;
	*/
	}
	public static Dictionary<unitType, unit> unitStats = new Dictionary<unitType, unit>();

	unitType type;
	int _damageMin;
	int _damageMax;
	int _capacity;
	int _victoryPoints = 0;
	int _reputationPenalty = 1;

	int _health;
	int _healthMax;

	public int DamageMin {
		get => _damageMin; set {
			_damageMin = value;
		}
	}
	public int DamageMax {
		get => _damageMax; set {
			_damageMax = value;
		}
	}
	public int Capacity {
		get => _capacity; set {
			_capacity = value;
		}
	}
	public int VictoryPoints { get => _victoryPoints; set => _victoryPoints = value; }
	public int ReputationPenalty { get => _reputationPenalty; set => _reputationPenalty = value; }
	public int Health {
		get => _health; set {
			_health = value;
			tags["health"] = value;
		}
	}
	public int HealthMax {
		get => _healthMax; set {
			_healthMax = value;
		}
	}


	// Start is called before the first frame update
	void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
