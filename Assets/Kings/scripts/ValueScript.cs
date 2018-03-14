﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


/// <summary>
/// todo. 스크립트를 연결하기 위한 빈 오브젝트들.  
/// Game -> Values -> adreadyValue
/// Game -> Values -> Army
/// Game -> Values -> AuthorityValue
/// Game -> Values -> CharimaValue
/// Game -> Values -> Country
/// Game -> Values -> CreativityValue
/// Game -> Values -> GamesPlayedValue
/// Game -> Values -> Gender
/// Game -> Values -> HealthValue 
/// Game -> Values -> IntelligenceValue
/// Game -> Values -> LookValue
/// Game -> Values -> LuckValue
/// Game -> Values -> MarriageValue
/// Game -> Values -> MarriedValue
/// Game -> Values -> Money
/// Game -> Values -> Name
/// Game -> Values -> People
/// Game -> Values -> Religion
/// Game -> Values -> Surname
/// Game -> Values -> Years
/// </summary>
public class ValueScript : MonoBehaviour {

    /// <summary>
    /// 현재 내가 붙어 있는 게임오브젝트가 게임에서 어떤 값의 유형인지 인스펙터 드롭다운 목록에서 선택해서 지정한다.
    /// 가령 내가 붙어 있는 게임오브젝트가 Game -> Values -> Name 이면 인스펙터 드롭다운 메뉴에서 Name를 선택한다.
    /// </summary>
	[Tooltip("여기에 내가 붙어 있는 게임오브젝트가 어떤 유형인지 옆에 드롭박스 목록에서 선택하세요.")]
	public valueDefinitions.values valueType;

	private string identifier = "valuename";

	[ReadOnlyInspector] /// 변수를 인스펙터에서 읽기 전용으로 만들기위한 프로그래머가 만든 애트리뷰트.
    public float value = 0f;

	public bool debugValueChanges = false;

	void Awake()
    {
        /// 인스펙터에서 지정한 밸류타입을 할당하고.
		identifier = valueType.ToString ();

		loadValue();
	}

	void Start()
    {
		valueManager.instance.registerValueScript (this);
	}

	public void actualizeUI(float uiValue){
		if (UserInterface.uiScrollbar != null) {
			UserInterface.uiScrollbar.size = uiValue / limits.max;
		}
		if (UserInterface.textValue != null) {
			UserInterface.textValue.text = uiValue.ToString(UserInterface.formatter);
		}
		if (UserInterface.uiSlider != null) {
			UserInterface.uiSlider.value = uiValue / limits.max;
		}
	}


	[System.Serializable]
	public class uiConfig{
		[Tooltip("If the value shall be displayed as a filling bar it can be a scrollbar or a slider. Define your preference here.")]
		public Scrollbar uiScrollbar;
		[Tooltip("If the value shall be displayed as a filling bar it can be a scrollbar or a slider. Define your preference here.")]
		public Slider uiSlider;
		[Tooltip("The speed of filling the bar, if the value changes.")]
		[Range(0.1f,100f)]public float lerpSpeed = 10f;
		[Tooltip("Define the format for displaying the value. \nE.g. 0 or # for only whole numbers.\nE.g. #.00 to show two following digits.")]
		public string formatter = "0.##";
		[Tooltip("The actual lerped/filling value.")]
		[ReadOnlyInspector]public float lerpedValue = 0f;
		[Tooltip("If the value is displayed as text, define the text field here.")]
		public Text textValue;

		[Tooltip("The value manager can show a change of the value to the user depending on 'showActualization'")]
		public bool showActualization = true;
		[Tooltip("The value manager can show a change of the value with this miniature sprite.")]
		public Sprite miniatureSprite;
	}
	[Tooltip("For each value script there can be different ways to display the value. Define the details here.")]
	public uiConfig UserInterface;




	void Update(){
		//UserInterface.lerpedValue = Mathf.Lerp (UserInterface.lerpedValue, value, UserInterface.lerpSpeed * Time.deltaTime);

		UserInterface.lerpedValue = MathExtension.linearInterpolate (UserInterface.lerpedValue, value, UserInterface.lerpSpeed * Time.deltaTime);

		actualizeUI (UserInterface.lerpedValue);
	}


	[System.Serializable]
	public class valueLimits
	{
		[Tooltip("What is the minimum possible value?")]
		public float min = 0f;
		[Tooltip("What is the maximum possible value?")]
		public float max = 100f;
		[Tooltip("For initialization with random values: What is the minimum random value.")]
		public float randomMin = 0f;
		[Tooltip("For initialization with random values: What is the maximum random value.")]
		public float randomMax = 100f;
	}

	public valueLimits limits;

	public void limitValue(){
		if (value < limits.min) {
			value = limits.min;
			events.OnMin.Invoke();
		}
		if (value > limits.max) {
			value = limits.max;
			events.OnMax.Invoke();
		}
	}

	/*
	 * Increas or decrease a value. 
	 */
	public float addValue (float increment){

		if (debugValueChanges == true) {
			Debug.Log ("Value '" + valueType.ToString () + "': " + value.ToString () + " increment by " + increment.ToString ());
		}

		value += increment;
		limitValue ();
		if (increment >= 0f) {
			events.OnIncrease.Invoke ();
		} else {
			events.OnDecrease.Invoke ();
		}
		saveValue ();

		if (debugValueChanges == true) {
			Debug.Log ("Value '" + valueType.ToString () + "' is now " + value.ToString () + " (after limiter)");
		}

		return value;
	}

	/*
	 * Set a value to an defined value.
	 */
	public float setValue(float newValue){
		value = newValue;
		limitValue ();
		saveValue ();
		return value;
	}

	[Tooltip("'keepValue' blocks the randomization of the value on a new game start. On the first startup of the game, the value is randomized between 'Limits.RandomMin' and 'Limits.RandomMax' (Accessable from Inspector).")]
	public bool keepValue = false;

	/*
	 * Randomize a value within the defined range. Helpfull for initialization of a new game.
	 */

	public float setValueRandom(){
			value = Random.Range (limits.randomMin, limits.randomMax);
			limitValue ();
			saveValue ();
		return value;
	}

	public void newGameStart(){
		if (keepValue == false) {
			setValueRandom ();
		}
	}


	public float multiplyValue (float multiplier){
		value *= multiplier;
		limitValue ();
		if (multiplier >= 1f) {
			events.OnIncrease.Invoke();
		}else {
			events.OnDecrease.Invoke ();
		}
		saveValue ();
		return value;
	}

	void loadValue()
    {
		if (SecurePlayerPrefs.HasKey (identifier)) {
			value = SecurePlayerPrefs.GetFloat (identifier);
		} else {
			setValueRandom ();
		}
	}

	void saveValue(){
		SecurePlayerPrefs.SetFloat (identifier, value);
	}

	public void saveMinMax(){
		float min = SecurePlayerPrefs.GetFloat (identifier+"_min");
		float max = SecurePlayerPrefs.GetFloat (identifier+"_max");

		if(SecurePlayerPrefs.HasKey(identifier+"_min")){
			if (value < min) {
				SecurePlayerPrefs.SetFloat (identifier+"_min",value);
			}	
		}else{
			SecurePlayerPrefs.SetFloat (identifier+"_min",value);
		}

		if (value > max) {
			SecurePlayerPrefs.SetFloat (identifier + "_max", value);
		}

	}

	public float getMaxValue(){
		return SecurePlayerPrefs.GetFloat (identifier+"_max");
	}

	public float getMinValue(){
		return SecurePlayerPrefs.GetFloat (identifier+"_min");
	}

	void OnDestroy(){
		saveValue ();
	}

	[System.Serializable] public class mEvent : UnityEvent {}

	[System.Serializable]
	public class valueEvents{
		public mEvent OnIncrease;
		public mEvent OnDecrease;
		public mEvent OnMax;
		public mEvent OnMin;
	}

	[Tooltip("Events are triggered on value changes or if the value is at one of its limits.")]
	public valueEvents events;


}
