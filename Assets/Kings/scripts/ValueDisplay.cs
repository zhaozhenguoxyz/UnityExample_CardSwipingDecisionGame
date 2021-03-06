﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ValueDisplay : MonoBehaviour {

	[Tooltip("Reference to an value script, which contains the compare information for enabling/disabling the gameobject.")]
	//value script is automatically linked later
	ValueScript vs;

	[Tooltip("Define which kind of value you want to display.")]
	public ValueDefinitions.값정의 valueTyp;

	[Tooltip("Define the text display for the current value.")]
	public Text currentValueText;
	[Tooltip("Define the text display for the minimal value of the script within this game.")]
	public Text minValueText;
	[Tooltip("Define the text display for the maximal value of the script within this game.")]
	public Text maxValueText;


	[Tooltip("Define the format for displaying the value. \nE.g. 0 or # for only whole numbers.\nE.g. #.00 to show two following digits.")]
	public string formatter = "0";
	[Tooltip("Define a multiplier for displaying of the value. This does't change the original value.")]
	public float displayMultiplier = 1f;

	public void showMinMaxValue(){

		vs = ValueManager.나자신.첫번째피팅값가져오기 (valueTyp);

		if (vs != null) {
			if (maxValueText != null) {
				maxValueText.text = (vs.최대값얻기 ()*displayMultiplier).ToString (formatter);
			}
			if (minValueText != null) {
				minValueText.text = (vs.최소값얻기 ()*displayMultiplier).ToString (formatter);
			}
			if (currentValueText != null) {
				currentValueText.text = (vs.플레이어프랩스데이터*displayMultiplier).ToString (formatter);
			}
		}
	}

	void Start(){
		showMinMaxValue ();
	}
}
