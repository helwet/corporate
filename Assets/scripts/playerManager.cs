using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour
{
	static public List<player> allPlayers = new List<player>();
	static public List<player> civs = new List<player>();
	public tile origo;

	public player neutralPlayer;

	public static player makeCivilizationLeader(Civilization c){
		player civLead = new player(c.empireName, false, c.tintColor);
		civs.Add(civLead);
		allPlayers.Add(civLead);
		return civLead;
	}
	// Start is called before the first frame update
	void Start()
    {
		//allPlayers[0].makeIntoComputerPlayer(0, origo);
		neutralPlayer = new player("neutral", false,Color.gray);
		allPlayers.Add(neutralPlayer);
		allPlayers.Add(new player("player1", true, Color.yellow));
		allPlayers.Add(new player("player2", true, Color.cyan));
		allPlayers.Add(new player("player3", true, Color.green));
		allPlayers.Add(new player("player4", true, Color.red));
	}

    // Update is called once per frame
    void Update()
    {
        
    }

}
