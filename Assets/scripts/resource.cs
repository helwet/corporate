using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType {
	NOTVALID,food, science, materia, money, reputation, karma, goods, media,anomaly
}

public class resourcePile : entityOnTile
{
	public static GameObject resourcePilePrefab;
	public static resourcePile makeResourcePile(ResourceType rt, int amount, player owner, tile t, building producer = null){
		GameObject GO = GameObject.Instantiate(resourcePilePrefab);
		resourcePile rp = GO.GetComponent<resourcePile>();
		rp.owner = owner;
		rp.amount = amount;
		rp.inUseBy = producer;
		rp.resourceType = rt;
		rp.tileOn = t;
		rp.immaterial = (t == null);
		return rp;
	}
	static public readonly Dictionary<ResourceType, int> baseCosts = new Dictionary<ResourceType, int>
	{ { ResourceType.materia, 3}, { ResourceType.goods, 6 }, { ResourceType.science, 5 }, { ResourceType.food, 2 }};

	private resourcePile() { }
	public int amount { get; protected set; } = 0;
	building inUseBy = null;
	bool immaterial;
	bool depleteable = false;
	//owner = player.neutralPlayer;
	//string resourceName = "not_a_valid_resource_type";
	public ResourceType resourceType { get; protected set; } = ResourceType.NOTVALID;
	int usedTimes = 0;
	string subtype = "bad_subtype_of_resource"; //use this track event choices and effects
												//player owner = null; //inherited from basetype

	public int canSpend(int amount) //returns if you can pay the amount
	{
		return this.amount - amount;
	}

		public int spend(int amount) //returns the amount left to spend
	{
		
		if(this.amount <= amount){
			this.amount -= amount;
			return 0;
		}
		else{
			this.amount = 0;
			return amount - this.amount;
		}
	}

	public void addResources(int amount){
		this.amount += amount;
	}

    // Start is called before the first frame update
    void Start()
    {
		addTag("resource");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
