using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
//basic actions = movement, build, traderoute, playspecial, actions["explore"], attack, deconstruct, trade, research
public enum ActionType {
	movement, explore, attack, build,
	playspecial, deconstruct,
	traderoute, trade,
	research
}
class actions {
	static GameObject go;
	static JArray standardActionsSetup() {
		var actions = new JArray();
		actions["movement"]["count"] = 2;
		actions["movement"]["phases"] = new JArray();
		actions["movement"]["phases"][0]["target"] = new JObject();
		actions["movement"]["phases"][0]["target"]["tags"] = new JArray("ship", "!combat");
		actions["movement"]["phases"][0]["target"]["relationship"] = "owned";
		actions["movement"]["phases"][1]["target"] = new JArray("tile","explored");
		actions["movement"]["phases"][2]["decklare_attack"] = "optional";

		actions["declare_attack"] = new JObject();
		actions["declare_attack"]["count"] = 1;
		actions["declare_attack"]["optional"] = true;
		actions["declare_attack"]["phases"] = new JArray();
		actions["declare_attack"]["phases"][0]["target"] = new JArray("player", "!friendly");


		actions["playSpecial"]["count"] = 1;
		//actions["playSpecial"]["optional"] = true;
		actions["playSpecial"]["phases"] = new JArray();
		actions["playSpecial"]["phases"][0]["target"]["tags"] = new JArray("card", "inHand");
		actions["playSpecial"]["phases"][0]["target"]["relationship"] = "owned";
		actions["playSpecial"]["phases"][1]["target"] = new JArray("tile", "explored");

		actions["triggerSpecial"]["count"] = 1;
		//actions["playSpecial"]["optional"] = true;
		actions["triggerSpecial"]["phases"] = new JArray();
		actions["triggerSpecial"]["phases"][0]["target"] = new JArray("card", "played", "owned");
		actions["triggerSpecial"]["phases"][0]["target"]["relationship"] = "owned";
		//actions["triggerSpecial"]["phases"][0]["target"]["result"]; //contains the selected go id
		actions["triggerSpecial"]["phases"][1]["pay"] = new JArray("tile", "explored");
		actions["triggerSpecial"]["phases"][1]["target"] = new JArray("tile", "explored");

		actions["explore"]["count"] = 1;
		actions["explore"]["tile_count"] = 1;
		actions["explore"]["tile_selected"] = "";
		actions["explore"]["phases"] = new JArray();
		actions["explore"]["phases"][1]["target"] = new JArray("coordinate,unexplored");
		actions["explore"]["phases"][0]["drawTiles"] = new JArray("player.explorationDraw"); //draw and show tiles
		actions["explore"]["phases"][2]["target"] = new JArray("tile", "beingShowedToPlayer"); //select from only the ones brought before camera
		actions["explore"]["phases"][2]["showSelection"] = actions["explore"]["tile_selected"]; //select the tile from options offered
		actions["explore"]["phases"][3]["target"] = new JArray("ship", "owned"); //move the ship used to actions["explore"]


		var build = new JObject();
		build["count"] = 1;
		build["discount"] = 0;
		build["phases"] = new JArray();
		build["phases"][0]["target"] = new JArray("tile", "any");
		build["phases"][1]["target"] = new JArray("building", "any"); //select building
																	  //fill up resources required field
		build["phases"][2]["target"] = new JArray("resource", "materia"); //open buy ui for resources for that tile, using required resrouces field

		var research = new JObject();
		research["count"] = 1;
		research["discount"] = 0;
		research["phases"] = new JArray();
		research["phases"][1]["target"] = new JArray("researching", "any"); //open research interface
																			//fill up resources required field
		research["phases"][2]["target"] = new JArray("resource", "materia"); //open buy ui for resources for that tile, using required resrouces field

		return actions;
	}



	void createPlayerActions() {

	}

	class selector {

		class selectionArgument {
			enum selectionType {
				unit, ship, player, resource, tile, entityOnTile, building, playedCard, card
			}
			enum relationshipType {
				friendly, hostile, neutral, owned
			}

			selectionType type;
			relationshipType relationship;
			List<string> tags;
			int amount = 1;
			player selecter = null;
			int range = 0;

			bool validateTarget(GameObject ob) {
				Selectable selected = ob.GetComponent<Selectable>();
				bool ok = false;
				switch (relationship) {
					case relationshipType.friendly:
						ok = selecter.friendly(selected.owner);
						break;
					case relationshipType.owned:
						if (selecter == selected.owner) {
							ok = true;
						}
						break;
					case relationshipType.hostile:
						ok = !selecter.friendly(selected.owner);
						break;
					case relationshipType.neutral:
						ok = !selecter.friendly(selected.owner) && !selecter.friendly(selected.owner);
						break;

					default:
						break;
				}
				if (!ok) return false;

				string selectionName = Enum.GetName(typeof(selectionType), type);
				if (selected.containsTag(selectionName)) {
					return true;
				}
				return false;
			}


			static selectionArgument movement(int amount, player p) {
				selectionArgument t = new selectionArgument();
				t.amount = amount;
				t.type = selectionType.ship;
				t.selecter = p;

				return t;
			}
			static selectionArgument building(int amount, int range, player p) {
				selectionArgument t = new selectionArgument();
				t.amount = amount;
				t.type = selectionType.building;
				t.selecter = p;
				return t;
			}
			static selectionArgument resource(int amount, player p) {
				selectionArgument t = new selectionArgument();
				t.amount = amount;
				t.type = selectionType.ship;
				t.selecter = p;
				return t;
			}
		}

		List<GameObject> movement() {
			List<GameObject> selection = new List<GameObject>();


			return selection;
		}

		List<GameObject> selectThings(List<string> str) {
			List<GameObject> selection = new List<GameObject>();
			return selection;
		}
	}
}