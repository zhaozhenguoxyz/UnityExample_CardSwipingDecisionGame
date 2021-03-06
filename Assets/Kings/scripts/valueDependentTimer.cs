﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ValueDependentTimer : MonoBehaviour {


	public EventScript.컨디션[] conditionsToTest;
	[System.Serializable] public class mEvent : UnityEvent {}

	[Tooltip("'deltaTimeCycle' defines which timecycle the value is tested and also the events invoked.")]
	public float deltaTimeCycle = 1f;
	[Tooltip("'onlyCountWhileGameActive' can block the counting, while the card is not movable (e. g. while inside the menue or the card spawns). When Gameovercards etc. are spawned, the game is still active.")]
	public bool onlyCountWhileGameActive = false;

	public mEvent OnConditionsTrue;
	public mEvent OnConditionsFalse;

	void Start(){
		
		StartCoroutine (cyclicTestValue ());
	}

	IEnumerator cyclicTestValue(){
		yield return null;
		//wait at least one frame bevore testing
		while (true) {


			while (CardStack.instance.getCardMoveEnabled () == false && onlyCountWhileGameActive == true) {
				yield return null;
			}

			if (ValueManager.나자신.AreConditinsForResultMet (conditionsToTest)) {
				OnConditionsTrue.Invoke ();
			}
			else
			{
				OnConditionsFalse.Invoke ();
			}

			yield return new WaitForSeconds (deltaTimeCycle);
		}
	}



}
