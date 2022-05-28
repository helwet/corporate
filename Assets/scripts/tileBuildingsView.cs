using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using System.Linq;

public class tileBuildingsView : MonoBehaviour
{
	//public List<building> buildings;
	SortedDictionary<player, SortedDictionary<buildingType, SortedDictionary<buildingSize, List<building>>>> buildings = new SortedDictionary<player, SortedDictionary<buildingType, SortedDictionary<buildingSize, List<building>>>>();


	public int timetoexpand = 15, timetowiden = 20;
	public float sizeOfRow = 1.5f;
	public float sizeOfColumn = 3f;
	public GameObject cornerleft , cornerright;

	List<int> goals;
    // Start is called before the first frame update
    void Start()
    {
		//var grouped = groupedBuildings.GroupBy<>
    }

	void addBuilding(building b){
		if(buildings.ContainsKey(b.owner)){
			if (buildings[b.owner].ContainsKey(b.type) == false) {
				buildings[b.owner][b.type] = new SortedDictionary<buildingSize, List<building>>();
				buildings[b.owner][b.type][b.size] = new List<building>();
			}
			else if(buildings[b.owner][b.type].ContainsKey(b.size) == false) {
				buildings[b.owner][b.type][b.size] = new List<building>();		
			}
		}
		else{
			buildings[b.owner] = new SortedDictionary<buildingType, SortedDictionary<buildingSize, List<building>>>();
			buildings[b.owner][b.type] = new SortedDictionary<buildingSize, List<building>>();
			buildings[b.owner][b.type][b.size] = new List<building>();
		}
		buildings[b.owner][b.type][b.size].Add(b);
	}

    // Update is called once per frame
    void Update()
    {
		int playerCount = buildings.Count;
		float targetX = 0 ;
		float targetY = 0 ;
		Vector3 target;
		foreach (var playerDict in buildings) {
			player current = playerDict.Key;
			targetX += sizeOfRow;
			foreach (var typeDict in playerDict.Value) {
				buildingType t = typeDict.Key;
				targetY += sizeOfColumn;
				foreach (var sizeDict in typeDict.Value) {
					buildingSize size = sizeDict.Key;
					foreach (building b in sizeDict.Value) {
						b.showModel();
						var seq = LeanTween.sequence();

						target = b.gameObject.transform.position;
						target.x += targetX;
						LTDescr lt = LeanTween.move(b.gameObject, target, timetoexpand).setEaseInBack();
						lt = LeanTween.alpha(b.gameObject, 1, timetowiden);
						//.move(b.gameObject, target, timetoexpand).setEaseInBack();

						target = b.gameObject.transform.position;
						target.x += targetX;
						target.y += targetY;
						seq.append(lt);
						lt = LeanTween.move(b.gameObject, target, timetowiden).setEaseInBack();
					}
				}
			}
		}
		
		//Vector3.MoveTowards(transform.position,target, target.x - 
		//LTDescr lt = LeanTween.move(b.gameObject, target).setOnUpdate((float val) => {
			//Vector3 vec = obj1.localPosition;
			//vec.x = obj1val * lineDrawScale;
			///vec.y = val * lineDrawScale;
	}
	bool locked = false;
	enum mode{
		minimized, expanding_up, expanding_side,open,locked
	}

	mode m;
	void display(){
		if(buildings.Count == 0){

		}
		else{
			switch (m) {
				case mode.minimized:
					break;
				case mode.expanding_up:
					break;
				case mode.expanding_side:
					break;
				case mode.open:
					break;
				case mode.locked:
					break;
				default:
					break;
			}
		}
	}
}
