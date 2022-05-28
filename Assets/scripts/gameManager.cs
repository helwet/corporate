using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class gameManager : MonoBehaviour
{
	enum gamePhases {
	 PRODUCE, ELECTION, ACTIONS, PLAYACTIONS, COMBAT, TRADE, CLEANUP  
	}
	public static gameManager instance;
	List<player> playerOrder = new List<player>();
	public Tilemap tilemap;

	void produce(){
		//for each building produce();
		//for each player draw, reduce bad blood, produce baseline;
		//for each reduce bad blood
	}

	Dictionary<player, int> bids = new Dictionary<player, int>();
	void initElections(){
		//start timer for players to set bids 
		//big red timer center screen and dialog with buttons, confirm bid button
		initElections(playerOrder);
	}

	void initElections(List<player> bidders) {
		//start timer for players to set bids 
		//big red timer center screen and dialog with buttons, confirm bid button
	}

	void processBids(){
		var bidList = this.bids.ToList();
		bidList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

		foreach(KeyValuePair<player,int> v in bidList){
			v.Key.pay(ResourceType.money, v.Value);
		}

		List<player> newPlayerOrder = new List<player>();
		
		
		if (bidList[0].Value == bidList[1].Value){
			List<player> bidders = new List<player>();
			bidders.Add(bidList[0].Key);
			for (int i = 1; i < bidList.Count; i++){
				if(bidList[i].Value == bidList[0].Value){
					bidders.Add(bidList[i].Key);
				}
			}
			initElections(bidders);
		}
	}

	void anounceElectionResults(){

	}


	void startActions(){

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
}
