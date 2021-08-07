namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GameCreator;

	[AddComponentMenu("")]
	public class ActivateVerb : IAction
	{
		public int example = 0;
		public GameObject verbs;	
        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
			//Get globally selected verb, find the selected object
			int verbCount = this.verbs.GetComponent<Variables.ListVariables>().iterator;
			object selectedItemObj = Variables.VariablesManager.GetGlobal("selectedItem");
			GameObject selectedItemGameObj = (GameObject)selectedItemObj;
			//If selected object then look for verbs
			//Verb object must be child of selectedObject in position 0
			if (selectedItemGameObj) {
			
				GameObject verbs = selectedItemGameObj.transform.GetChild(0).gameObject;
				GameObject selectedVerb = verbs.transform.GetChild(verbCount).gameObject;
				Actions actionVerb = selectedVerb.GetComponent<Actions>();

				if (actionVerb)
				{
					actionVerb.Execute();
				}
			}
	
			return true;
        }

		#if UNITY_EDITOR
        public static new string NAME = "Custom/ActivateVerb";
        private GameObject selectedItemGameObj;
#endif
    }
}
 