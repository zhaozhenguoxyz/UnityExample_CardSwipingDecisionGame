﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// GlobalMessageEventManager는 (애니메이터의 트리거와 같은) 문자열 메시지에 대한 수신자를 수집하고 메시지를 그들에게 배포합니다.
/// 메시지에 따라 수신기는 단일 이벤트를 호출합니다.
/// Manger는 수신기가 있거나 현장에있을 경우 자동으로 "DontDestroyOnLoad"- 게임 개체로 생성됩니다. 게임 객체에 수동으로 추가해서는 안됩니다.
/// 이 구조 때문에 메시지는 열린 다중 장면에서 전송 될 수 있습니다.
/// </summary>
public class GlobalMessageEventManager : MonoBehaviour {

	public static GlobalMessageEventManager instance;

	public List<GlobalMessageEventReceiver> receivers;

	void Awake(){
		buildAwake ();
	}

    /// <summary>
    /// 내가 붙어 있는 'GlobaMessageEventManager'를 싱글톤 패턴으로 사용하기 위해 객체를 하나만 만들기 위한 메서드.
    /// 아직 매니저가 없다면 gameObject를 'DontDestroyOnLoad'로 표시하고 receiver-list를 생성하십시오. 그렇지 않으면 자체 파괴하십시오.
    /// </summary>
    public void buildAwake()
    {
		if (instance == null)
        {
			instance = this;
			GameObject.DontDestroyOnLoad (gameObject);
			receivers = new List<GlobalMessageEventReceiver> ();
		}
        else
        {
			if (instance != this)
            {
				Destroy (gameObject);
			}
		}
	}

    /// <summary>
    /// 수신자가 호출 : 메시지 수신 등록
    /// </summary>
    /// <param name="recv"></param>
    public static void registerMessageReceiver(GlobalMessageEventReceiver recv)
    {
		if (instance != null) {
			instance.receivers.Add (recv);
		}
	}

    /// <summary>
    /// 수신자가 호출 (OnDestro ()) : 메시지 수신을 위해 등록 취소
    /// </summary>
    /// <param name="recv"></param>
    public static void unregisterMessageReceiver(GlobalMessageEventReceiver recv){
		if (instance != null) {
			instance.receivers.Remove (recv);
		}
	}

    /// <summary>
    /// 송신기 스크립트에 의해 호출 : 등록 된 모든 수신기에 메시지 보내기
    /// </summary>
    /// <param name="message"></param>
    public static void sendToReceivers(string message){
		if (instance != null) {
			//Debug.Log ("Manager, sending:'" + message + "'");
			foreach (GlobalMessageEventReceiver recv in instance.receivers) {
				recv.globalMessage (message);
			}
		} else {
			Debug.Log ("경고 : GlobalMessageEventManager가 누락되었습니다. 수신자가 있는 경우 생성이 자동으로 이루어져야 합니다.");
		}
	}

}
