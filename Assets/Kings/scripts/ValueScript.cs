﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


/// <summary>
/// 게임에서 사용되는 수치들을 계산하고 여러가지 처리등을 하기 위한 클래스.
/// 이 스크립트는 게임씬내에 빈 오브젝트들을 만들어서 연결해놓는다.  
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
    /// 현재 내가 붙어 있는 게임오브젝트가 게임에서 어떤 것을 담당하는지 유형인지 인스펙터 드롭다운 목록에서 선택해서 지정한다.
    /// 가령 내가 군대수치와 관련된 작동들을 할려고 하면, 게임씬에 빈 오브젝트로 만들어서 이름을 Army로 짓고, 이 클래스를 연결한다.
    /// 즉 Game -> Values -> Army이면 인스펙터 드롭다운 메뉴에서 Army를 선택해주어서 내가 붙어 있는 오브젝트의 성격을 규정해준다.
    /// </summary>
	[Tooltip("내가 붙어 있는 게임오브젝트가 어떤 유형인지 옆에 드롭박스 목록에서 선택하세요.")]
	public valueDefinitions.values valueType;

    /// <summary>
    /// 플레이어프랩스에 키값으로 사용되는 텍스트를 가지고 있는 변수.
    /// 사용자가 인스펙터에서 지정한 내가 붙어 있는 객체가 무엇인지(이름, 성별, 군인수 등등)지정한 값을 문자열로 저장하고 있는 변수.
    /// 가령 군인수치를 담당하는 객체이면 프로그래머가 valueType 변수에 'Army'열거요소로 값을 할당했을 것이다. 
    /// 이 값을 본 변수에 문자(텍스트)로 저장하고 있으며, 이 값을 플레이어프랩스에 키값으로 사용하기 위해 따로 변수를 만들어둔것이다.
    /// </summary>
	private string identifier = "valuename";

    /// <summary>
    /// 내 정체. 플레이어프랩스에 저장되어 있는 내가 붙어 있는 객체가 무엇인지(이름, 성별등등)을 가져와서 저장하는 변수
    /// </summary>
	[ReadOnlyInspector] /// 변수를 인스펙터에서 읽기 전용으로 만들기위한 프로그래머가 만든 애트리뷰트.
    public float value = 0f;

    /// <summary>
    /// 디버그 값 변경 ??????
    /// </summary>
	public bool debugValueChanges = false;

	void Awake()
    {
        /// 프로그래머가 인스펙터에서 내가 붙어 있는 객체가 무엇인지 지정한 값을 문자로 저장한다. (이름, 성별, 나라, 돈 등등)
        /// 가령 인스펙터에서 내가 붙어 있는 객체의 역할이 군인수치를 관리하는 객체라면 'Army'로 지정했을 것이고, 이때 식별자로 사용하는 변수에 'Army'텍스트 문자를 따로 저장한다.
		identifier = valueType.ToString ();

        /// 플레이어프랩스에 값을 불러오거나 저장한다.
		loadValue();
	}

	void Start()
    {
        /// todo. 작업1
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
		[Tooltip("값을 채우기 막대로 표시하는 경우에는 스크롤 막대 또는 슬라이더를 사용할 수 있습니다. 여기에서 환경 설정을 정의하십시오.")]
		public Scrollbar uiScrollbar;
		[Tooltip("값을 채우기 막대로 표시하는 경우에는 스크롤 막대 또는 슬라이더를 사용할 수 있습니다. 여기에서 환경 설정을 정의하십시오.")]
		public Slider uiSlider;
		[Tooltip("값이 변경되면 바를 채우는 속도입니다.")]
		[Range(0.1f,100f)]public float lerpSpeed = 10f;
		[Tooltip("값을 표시하기 위한 형식을 정의하십시오. \n예: 0 또는 # 을 입력하십시오. \n예 : #.00을 입력하면 두자리 숫자가 표시됩니다.")]
		public string formatter = "0.##";
		[Tooltip("실제 lerped/filling 값.")]
		[ReadOnlyInspector]public float lerpedValue = 0f;
		[Tooltip("값이 텍스트로 표시되면 여기에서 텍스트 필드를 정의하십시오.")]
		public Text textValue;

		[Tooltip("값 관리자는 'showActualization'에 따라 값 변경을 사용자에게 표시할 수 있습니다.")]
		public bool showActualization = true;
		[Tooltip("값 관리자는 미니터처스프라이트로 값의 변경을 표시할 수 있습니다.")]
		public Sprite miniatureSprite;
	}
	[Tooltip("각 값 스크립트에 대해 값을 표시하는 다른 방법이 있을 수 있습니다. 여기에 세부 정보를 정의하십시오.")]
	public uiConfig UserInterface;




	void Update(){
		//UserInterface.lerpedValue = Mathf.Lerp (UserInterface.lerpedValue, value, UserInterface.lerpSpeed * Time.deltaTime);

		UserInterface.lerpedValue = MathExtension.linearInterpolate (UserInterface.lerpedValue, value, UserInterface.lerpSpeed * Time.deltaTime);

		actualizeUI (UserInterface.lerpedValue);
	}


    /// <summary>
    /// 최소값과 최대값의 범위를 지정한 클래스. 
    /// 여기서는 일단 대충 기본값으로 초기화하고, 
    /// 실제로 부여하는 값은 인스펙터에서 노출된 변수에서 프로그래머가 해당 오브젝트의 성격에 따라 최소값과 최대값을 지정한다.
    /// </summary>
	[System.Serializable]
	public class valueLimits
	{
        /// <summary>
        /// 프로그래머가 설정하는 밸류값이 벗어나면 안되는 범위의 최소값
        /// </summary>
		[Tooltip("가능한 최소값은 무엇입니까?")]
		public float min = 0f;

        /// <summary>
        /// 프로그래머가 설정하는 밸류값이 벗어나면 안되는 범위의 최대값
        /// </summary>
		[Tooltip("가능한 최대값은 무엇입니까?")]
		public float max = 100f;

        /// <summary>
        /// 랜덤으로 값을 뽑기할때 랜덤범위의 최소값.
        /// </summary>
		[Tooltip("랜덤으로 값을 할당하는 경우 허용 가능한 최소값을 입력하세요.")]
		public float randomMin = 0f;

        /// <summary>
        /// 랜점으로 값을 뽑기할때 랜덤범위의 최대값.
        /// </summary>
		[Tooltip("랜덤으로 값을 할당하는 경우 허용 가능한 최대값을 입력하세요.")]
		public float randomMax = 100f;
	}

    /// <summary>
    /// 최소값과 최대값의 범위. 내가 붙어 있는 오브젝트의 성격에 따라 인스펙터에서 각각의 값의 범위를 지정한다.
    /// </summary>
	public valueLimits limits;

    /// <summary>
    /// 내 정체 밸류값이 내가 설정한 범위를 넘지 못하도록 하고, 설정한 범위를 넘을 경우 최소값 또는 최대값을 할당한다. 그리고 최소값 & 최대값일때 실행할 유니티이벤트를 실행한다.
    /// </summary>
	public void limitValue()
    {
        /// 밸류값이 내가 설정한 범위보다 작은 경우
		if (value < limits.min)
        {
            /// 밸류값에 내가 설정한 범위 최소값을 할당한다.
			value = limits.min;
            /// 최소값일때 작동할 유니티이벤트를 실행한다.
			events.OnMin.Invoke();
		}
        /// 만약 밸류값이 내가 설정한 범위보다 큰 경우
		if (value > limits.max)
        {
            /// 밸류값에 내가 설정한 범위 최대값을 할당한다.
			value = limits.max;
            /// 최대값일때 작동할 유니티이벤트를 실행한다.
			events.OnMax.Invoke();
		}
	}

	/// <summary>
    /// 값을 늘리거나 줄인다.
    /// </summary>
    /// <param name="increment"></param>
    /// <returns></returns>
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


    /// <summary>
    /// 내 정체를 랜덤으로 값을 지정하는 메서드. 값을 지정할때 최소&최대값을 넘지 못하도록 하고 있으며, 최소&최대값일때 실행할 유니티이벤트를 실행한다. 그리고 플레이어프랩스에 데이터를 저장한다.
    /// </summary>
    /// <returns></returns>
    public float setValueRandom()
    {
        /// 랜덤으로 지정한 값을 배정하고
        value = Random.Range (limits.randomMin, limits.randomMax);
        
        /// 내 정체 밸류값이 내가 설정한 범위를 넘지 못하도록 하고, 설정한 범위를 넘을 경우 최소값 또는 최대값을 할당한다.
        /// 그리고 최소값 & 최대값일때 실행할 유니티이벤트를 실행한다
        limitValue();

        /// 플레이어프랩스에 데이터 저장
		saveValue ();

        /// 랜덤으로 지정한 값을 반환
        return value;
	}

	public void newGameStart()
    {
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

    /// <summary>
    /// 내가 붙어 있는 오브젝트 성격을 지정한 값이 기존 플레이어프랩스에 저장되어 있는 값인지 검사해서 없으면 저장하고 있으면 불러온다.
    /// </summary>
	void loadValue()
    {
        /// 현재 내가 붙어 있는 오브젝트이 성격을 지정한 값이 기존 플레이어프랩스에 이미 저장되어 있는 값인지 검사해서, 이미 저장되 있는 키값일 경우 
		if (SecurePlayerPrefs.HasKey (identifier))
        {
            /// 플레이어프랩스에 저장되어 있는 키값을 가져와서 
			value = SecurePlayerPrefs.GetFloat (identifier);
		}
        else /// 저장되 있는 키값이 아닌 경우에는
        {
            /// 내 정체를 랜덤으로 값을 지정하는 메서드. 값을 지정할때 최소&최대값을 넘지 못하도록 하고 있으며, 최소&최대값일때 실행할 유니티이벤트를 실행한다. 그리고 플레이어프랩스에 데이터를 저장한다.
			setValueRandom();
		}
	}

    /// <summary>
    /// 플레이어프랩스에 데이터를 저장.
    /// </summary>
	void saveValue()
    {
        /// 플레이어프랩스에 데이터를 저장한다.
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

    /// <summary>
    /// 유니티 이벤트.
    /// </summary>
	[System.Serializable] /// 유니티 이벤트를 인스펙터에 노출시키기 위해 직렬화해준다.
    public class mEvent : UnityEvent {}

    /// <summary>
    /// 유니티 이벤트들의 모음 클래스.
    /// </summary>
	[System.Serializable] /// 클래스를 인스펙터에 노출시키기 위해 직렬화해준다.
	public class valueEvents
    {
        /// <summary>
        /// 증가할때 실행할 유니티이벤트를 연결하는 변수
        /// </summary>
		public mEvent OnIncrease;

        /// <summary>
        /// 감소할때 실행할 유니티이벤트를 연결하는 변수
        /// </summary>
        public mEvent OnDecrease;

        /// <summary>
        ///  최대일때 실행할 유니티이벤트를 연결하는 변수
        /// </summary>
		public mEvent OnMax;

        /// <summary>
        /// 최소일때 실행할 유니티이벤트를 연결하는 변수
        /// </summary>
		public mEvent OnMin;
	}

    /// <summary>
    /// 인스펙터에서 유니티이벤트를 사용하기 위해 노출하는 변수.
    /// </summary>
	[Tooltip("이벤트는 값 변경시 또는 값이 한계 중 하나에 있을때 트리거됩니다.")]
	public valueEvents events;


}
