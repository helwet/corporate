using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour{

	protected Dictionary<string, double> tags = new Dictionary<string, double>();
	public bool containsTag(string s) {
		return tags.ContainsKey(s);
	}
	public void removeTag(string tag) {
		tags.Remove(tag);
	}
	public void addTag(string tag) {
		tags.Add(tag, 1);
	}
	public player owner { get; protected set; }

	public bool validSelection(List<string> requiredTags){
		foreach (string s in requiredTags) {
			if (s[0] == '!') { //if looking not to have that tag
				string search = s.Substring(1);
				if (tags.ContainsKey(search)) {
					return false;
				}
			}
			else {
				if (tags.ContainsKey(s) == false)
					return false;
			}
		}
		return true;
	}

	public bool friendly(player p){
		return owner.friendly(p);
	}

	public void highlight(){
		
	}


	void Start() {
		this.gameObject.tag = "selectable";
	}

}
public class entityOnTile : Selectable {
	static private int entityID__ = 0;
	readonly int entityID = entityID__++;
	protected tile tileOn = null;
	protected GameObject model;

	// Start is called before the first frame update
	void Start()
    {
		addTag("entityOnTile");
		addTag("selectable");
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
/*
public class card : entityOnTile {
	bool isInHand() {
		if (tileOn != tile.owner != owner && tile.owner != player.neutral) {

		}
	}
}*/
