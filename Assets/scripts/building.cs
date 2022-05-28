using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum buildingType {
	HQ, mine, farm, finance, research, defensePlatform, starport
}
public enum buildingSize {
	small, medium, large, megastructure
}

public struct cost{
	public int amount;
	public ResourceType type;
}

public class building : entityOnTile
{
	public static building makeBuilding(player owner,tile t, buildingType type, buildingSize size){
		GameObject father = new GameObject();
		building b = father.AddComponent<building>();

		t.buildings.Add(b);
		b.tileOn = t;
		b.owner = owner;
		owner.myBuildings.Add(b);
		b.model = GameObject.Instantiate(ModelLibrary.getBuildingModel(owner.civ, type, size));
		b.model.transform.SetParent(b.gameObject.transform,false);

		b.size = size;
		b.amountProduced = owner.producedAmounts[type][size];
		b.storageSpace = b.amountProduced * 2;
		if (type == buildingType.HQ || type == buildingType.mine) {
			b.starport = true;
		}
		switch (type) {
			case buildingType.mine:
				b.produced = ResourceType.materia;
				break;
			case buildingType.farm:
				b.produced = ResourceType.materia;
				break;
			case buildingType.research:
				b.produced = ResourceType.science;
				break;
			case buildingType.finance:
				b.produced = ResourceType.money;
				break;

		}

		b.setColour();
		return b;
	}

	cost getUpgradeCost(buildingSize tier) {
		int amount = owner.buildAndUpgradeCosts[type][size][(int)tier];
		return new cost{ amount= amount, type = ResourceType.materia};
	}

	void upgrade(){
		if (size == buildingSize.large) {
			return;
		}
		else{
			size += 1;
			amountProduced = owner.producedAmounts[type][size];
			model.SetActive(false);
			model = GameObject.Instantiate(ModelLibrary.getBuildingModel(owner.civ, type, size));
			model.transform.SetParent(gameObject.transform);
			setColour();
		}
	}

	internal GameObject showModel() {
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(true);
		}
		return transform.GetChild(0).gameObject;
	}

	resourcePile storage;
	int storageSpace;

	public ResourceType produced { get; protected set; } = ResourceType.NOTVALID;
	public int amountProduced { get; protected set; } = 0;
	bool starport = false;
	public buildingSize size { get; protected set; }
	public buildingType type { get; protected set; }
	List<tradeRoute> tradeRoutes;
	public class tradeRoute{
		Vector3 target;
		building source;
		int countResource;
		
	}

	void produce() //produces resources, creates resource piles
	{

	}

    // Start is called before the first frame update
    void Start()
    {
		model = this.transform.GetChild(0).gameObject;
		setColour();
    }
	Color myTint;
	Renderer rend;
	
	//sets the tint of the building to right one
	private void setColour() {
		rend = model.GetComponent<Renderer>();
		myTint = owner.playerTint;
		rend.material.SetColor("_Color", myTint);

	}

	entityOnTile worker;
	public void setWorking(entityOnTile worker){
		
	}

	public void displayBuildingIsOnline(){
		Vector3 v = Vector3.zero;
		v.x = 180;
		model.transform.Rotate(v, Space.World); 
	}
	
	public void hideModel(){
		model.SetActive(false);
	}

	// Update is called once per frame
	void Update()
    {
        
    }

}

/*
 * 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class MovementController : MonoBehaviour
{
    //Stores input from the PlayerInput
    private Vector2 movementInput;

    private Vector3 direction;

    public Tilemap fogOfWar;

    bool hasMoved;
    void Update()
    {
        if(movementInput.x == 0)
        {
            hasMoved = false;
        }
        else if (movementInput.x != 0 && !hasMoved)
        {
            hasMoved = true;

            GetMovementDirection();
        }

    }

    public void GetMovementDirection()
    {
        if (movementInput.x < 0)
        {
            if (movementInput.y > 0)
            {
                direction = new Vector3(-0.5f, 0.5f);
            }
            else if (movementInput.y < 0)
            {
                direction = new Vector3(-0.5f, -0.5f);
            }
            else
            {
                direction = new Vector3(-1, 0, 0);
            }
            transform.position += direction;
            UpdateFogOfWar();
        }
        else if (movementInput.x > 0)
        {
            if (movementInput.y > 0)
            {
                direction = new Vector3(0.5f, 0.5f);
            }
            else if (movementInput.y < 0)
            {
                direction = new Vector3(0.5f, -0.5f);
            }
            else
            {
                direction = new Vector3(1, 0, 0);
            }

            transform.position += direction;
            UpdateFogOfWar();
        }
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.position -= direction;
    }

    public int vision = 1;

    void UpdateFogOfWar()
    {
        Vector3Int currentPlayerTile = fogOfWar.WorldToCell(transform.position);

        //Clear the surrounding tiles
        for(int x=-vision; x<= vision; x++)
        {
            for(int y=-vision; y<= vision; y++)
            {
                fogOfWar.SetTile(currentPlayerTile + new Vector3Int(x, y, 0), null);
            }

        }

    }
}
*/
