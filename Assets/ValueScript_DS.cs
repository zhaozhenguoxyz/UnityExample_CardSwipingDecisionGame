﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임에서 사용되는 모든 가치값을 정의해놓은 열거형.
/// 유니티에서 열거형 변수는 인스펙터에서 드롭 다운 메뉴를 표시되기때문에 프로그래머가 인스펙터에서 목록의 요소를 쉽게 선택해서 지정할 수 있게 하기 위해 본 게임에서 사용되는 모든 값들을 열거형으로 정의해놨다.
/// </summary>
public enum 값정의
{
    이름, // 이름
    서브네임, // 성
    성별, // 성별
    국가, // 나라
    통치기간, // 통치기간
    군대, // 군대수치
    국민, // 국민수치
    종교, // 종교수치
    재물, // 돈
    권위, // 권위
    지성, // 지성
    카리스마, // 카리스마
    행운, // 행운
    독창성, // 독창성
    look, // 이거 뭔지 잘 모르겠음.
    건강, // 건강
    결혼, // 결혼
    기혼, // 기혼
    광고준비, // 광고준비
    게임플레이중 // 게임플레이중
}

/// <summary>
/// 게임에서 사용되는 수치들을 계산하고 여러가지 처리등을 하기 위한 클래스.
/// 이 스크립트는 게임씬내에 빈 오브젝트들을 만들어서 연결해놓는다.  
/// Play -> Values -> adreadyValue
/// Play -> Values -> Army
/// Play -> Values -> AuthorityValue
/// Play -> Values -> CharimaValue
/// Play -> Values -> Country
/// Play -> Values -> CreativityValue
/// Play -> Values -> GamesPlayedValue
/// Play -> Values -> Gender
/// Play -> Values -> HealthValue 
/// Play -> Values -> IntelligenceValue
/// Play -> Values -> LookValue
/// Play -> Values -> LuckValue
/// Play -> Values -> MarriageValue
/// Play -> Values -> MarriedValue
/// Play -> Values -> Money
/// Play -> Values -> Name
/// Play -> Values -> People
/// Play -> Values -> Religion
/// Play -> Values -> Surname
/// Play -> Values -> Years
/// </summary>
public class ValueScript_DS : MonoBehaviour {

    /// <summary>
    /// 현재 이 스크립트가 붙어 있는 게임오브젝트가 게임에서 어떤 것을 담당하는지 유형인지 인스펙터 드롭다운 목록에서 선택하기 위해 열거형 변수.
    /// 가령 내가 군대수치와 관련된 작동들을 할려고 하면, 게임씬에 빈 오브젝트로 만들어서 이름을 Army로 짓고, 이 클래스를 연결한다.
    /// 즉 Play -> Values -> Army이면 인스펙터 드롭다운 메뉴에서 '군대'를 선택해주어서 내가 붙어 있는 오브젝트의 성격을 규정해준다.
    /// </summary>
    [Tooltip("옆에 목록에서 내가 어떤 성격의 오브젝트인지 선택하세요.")]
    public 값정의 내역활;

    /// <summary>
    /// 플레이어프랩스에 키값으로 사용하기 위한 스트링타입 변수.
    /// 사용자가 인스펙터에서 지정한 내가 붙어 있는 객체가 무엇인지(이름, 성별, 군인수 등등)지정한 값을 문자열로 저장하고 있는 변수.
    /// 가령 군인수치를 담당하는 객체이면 프로그래머가 alueType 변수에 '군인'열거요소로 값을 할당했을 것이다. 
    /// 이 값을 본 변수에 문자(텍스트)로 저장하고 있으며, 이 값을 플레이어프랩스에 키값으로 사용하기 위해 따로 변수를 만들어둔것이다.
    /// </summary>
    private string 플레이어프랩스키값 = "빈값";

    /// <summary>
    /// 플레이어프랩스에 내 성격의 데이터가 이미 있을 경우, 해당 데이터를 가져와서 저장하는 변수
    /// </summary>
    [ReadOnlyInspector] /// 변수를 인스펙터에서 읽기 전용으로 만들기위한 프로그래머가 만든 애트리뷰트.
    public float 플레이어프랩스데이터 = 0f;
    
    /// <summary>
    /// 최소값과 최대값의 범위를 지정한 클래스. 
    /// 여기서는 일단 대충 기본값으로 초기화하고, 
    /// 실제로 부여하는 값은 인스펙터에서 노출된 변수에서 프로그래머가 해당 오브젝트의 성격에 따라 최소값과 최대값을 지정한다.
    /// </summary>
	[System.Serializable]
    public class 값범위
    {
        /// <summary>
        /// 프로그래머가 설정하는 밸류값이 벗어나면 안되는 범위의 최소값
        /// </summary>
		[Tooltip("가능한 최소값을 넣어주세요")]
        public float 최소값 = 0f;

        /// <summary>
        /// 프로그래머가 설정하는 밸류값이 벗어나면 안되는 범위의 최대값
        /// </summary>
		[Tooltip("가능한 최대값을 넣어주세요")]
        public float 최대값 = 100f;

        /// <summary>
        /// 랜덤으로 값을 뽑기할때 랜덤범위의 최소값.
        /// </summary>
		[Tooltip("랜덤으로 값을 할당하는 경우 허용 가능한 최소값을 입력하세요.")]
        public float 랜덤최소값 = 0f;

        /// <summary>
        /// 랜점으로 값을 뽑기할때 랜덤범위의 최대값.
        /// </summary>
		[Tooltip("랜덤으로 값을 할당하는 경우 허용 가능한 최대값을 입력하세요.")]
        public float 랜덤최대값 = 100f;
    }
    
    /// <summary>
    /// 최소값과 최대값의 범위. 내가 붙어 있는 오브젝트의 성격에 따라 인스펙터에서 각각의 값의 범위를 지정한다.
    /// </summary>
    public 값범위 범위;


    private void Awake()
    {
        /// 플레이어프랩스에 있는 데이터와 비교하기 위해(데이터가 있으면 불러오고, 없으면 저장하기 위해), 우선 내가 붙어 있는 오브젝트가 어떤 역할을 하는지 프로그래머가 인스펙터에서 할당한 값을 스트링타입으로 넣어준다. 
        플레이어프랩스키값 = 내역활.ToString();

        ///
        플레이어프랩스저장또는불러오기();

    }






    /// <summary>
    /// 내가 붙어 있는 오브젝트 성격을 지정한 값이 기존 플레이어프랩스에 저장되어 있는 값인지 검사해서 없으면 저장하고 있으면 불러온다.
    /// </summary>
    private void 플레이어프랩스저장또는불러오기()
    {
        /// 현재 내가 붙어 있는 오브젝트이 성격을 지정한 값이 기존 플레이어프랩스에 이미 저장되어 있는 값인지 검사해서, 이미 저장되 있는 키값일 경우 
		if (SecurePlayerPrefs.HasKey(플레이어프랩스키값))
        {
            /// 플레이어프랩스에 저장되어 있는 키값을 가져와서 저장한다.
			플레이어프랩스데이터 = SecurePlayerPrefs.GetFloat(플레이어프랩스키값);
        }
        else /// 저장되 있는 키값이 아닌 경우에는
        {
            /// 내 정체를 랜덤으로 값을 지정하는 메서드. 값을 지정할때 최소&최대값을 넘지 못하도록 하고 있으며, 최소&최대값일때 실행할 유니티이벤트를 실행한다. 그리고 플레이어프랩스에 데이터를 저장한다.
			랜덤값세팅();
        }

    }


    /// <summary>
    /// 내 정체를 랜덤으로 값을 지정하는 메서드. 값을 지정할때 최소&최대값을 넘지 못하도록 하고 있으며, 최소&최대값일때 실행할 유니티이벤트를 실행한다. 그리고 플레이어프랩스에 데이터를 저장한다.
    /// </summary>
    /// <returns></returns>
    public float 랜덤값세팅()
    {
        /// 랜덤으로 지정한 값을 배정하고
        플레이어프랩스데이터 = Random.Range(범위.랜덤최소값, 범위.랜덤최대값);

        /// 내 정체 밸류값이 내가 설정한 범위를 넘지 못하도록 하고, 설정한 범위를 넘을 경우 최소값 또는 최대값을 할당한다.
        /// 그리고 최소값 & 최대값일때 실행할 유니티이벤트를 실행한다
        설정값으로실행();

        /// 플레이어프랩스에 데이터 저장
		//saveValue();

        /// 랜덤으로 지정한 값을 반환
        return 플레이어프랩스데이터;
    }

    /// <summary>
    /// 내 정체 밸류값이 내가 설정한 범위를 넘지 못하도록 하고, 설정한 범위를 넘을 경우 최소값 또는 최대값을 할당한다. 그리고 최소값 & 최대값일때 실행할 유니티이벤트를 실행한다.
    /// </summary>
    public void 설정값으로실행()
    {
        /// 밸류값이 내가 설정한 범위보다 작은 경우
		if (플레이어프랩스데이터 < 범위.최소값)
        {
            /// 밸류값에 내가 설정한 범위 최소값을 할당한다.
			플레이어프랩스데이터 = 범위.최소값;
            /// 최소값일때 작동할 유니티이벤트를 실행한다.
			//events.OnMin.Invoke();
        }
        /// 만약 밸류값이 내가 설정한 범위보다 큰 경우
		if (플레이어프랩스데이터 > 범위.최소값)
        {
            /// 밸류값에 내가 설정한 범위 최대값을 할당한다.
			플레이어프랩스데이터 = 범위.최대값;
            /// 최대값일때 작동할 유니티이벤트를 실행한다.
			//events.OnMax.Invoke();
        }
    }

    /// <summary>
    /// 유니티 이벤트를 상속받은 클래스.
    /// </summary>
    [System.Serializable] /// 클래스를 인스펙터에 노출시키기 위해 직렬화해준다.
    public class 유니티이벤트 : UnityEngine.Events.UnityEvent { }

    /// <summary>
    /// 상황에 따른 유니티 이벤트를 인스펙터에서 연결시키기 위한 클래스.
    /// </summary>
    [System.Serializable] /// 클래스를 인스펙터에 노출시키기 위해 직렬화해준다.
    public class 실행이벤트
    {
        /// <summary>
        /// 증가할때 실행할 유니티 이벤트를 연결하는 변수
        /// </summary>
        public 유니티이벤트 증가할때;

        /// <summary>
        /// 감소할때 실행할 유니티 이벤트를 연결하는 변수
        /// </summary>
        public 유니티이벤트 감소할때;

        /// <summary>
        /// 최대값일때 실행할 유니티 이벤트를 연결하는 변수
        /// </summary>
        public 유니티이벤트 최대값일때;

        /// <summary>
        /// 최소일때 실행할 유니티 이벤트를 연결하는 변수
        /// </summary>
        public 유니티이벤트 최소값일때;
    }

    /// <summary>
    /// 인스펙터에서 유니티이벤트를 사용하기 위해 노출하는 변수
    /// </summary>
    [Tooltip("이벤트는 값 변경시 또는 값이 한계 중 하나에 있을때 트리거됩니다.????")]
    public 실행이벤트 실행할이벤트;






}
