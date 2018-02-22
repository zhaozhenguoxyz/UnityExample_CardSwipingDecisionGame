﻿//#define USE_I2_LOCALIZATION

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Text;
using System;

#if USE_I2_LOCALIZATION
using I2.Loc;
#endif

public class TranslationManager : MonoBehaviour {

    /// <summary>
    /// 'translateIfAvail' 메서드는 번역을 구현하는데 사용할 수 있습니다.
    /// 겡미에 새 텍스트가 표시되면 먼저 이 기능을 통해 번역이 테스트됩니다.
    /// 언어가 변경되면 텍스트가 장면을 다시 불러올 때까지 업데이트 되지 않는다는 단점이 있습니다.
    /// </summary>
    /// <param name="term"></param>
    /// <returns></returns>
    public static string translateIfAvail(string term){
		string retString;

		//Example of the use of I2 localization.
		//To enable I2 localization, uncomment the line '#define USE_I2_LOCALIZATION'
		//on the beginning of the script.
		#if USE_I2_LOCALIZATION
		retString = I2.Loc.LocalizationManager.GetTranslation(  term  );
		if(string.IsNullOrEmpty(retString))
		{
		//Debug.LogWarning("Missing translation for term: '" + term +"' , using term as translation result." );
		return term;
		}else{
		return retString;
		}
		#else
		return term;
		#endif
	}

	/*
	 * Following code is for collecting most of the translatable terms or texts.
	 * Each script with translatable content is derivated from 'TranslatableContent'.
	 * On Start it registers at this manager by calling 'TranslationManager.instance.registerTranslateableContentScript (this);'
	 * Each of this scripts has to implement 'public override List<string> getTranslatableTerms ()' and returns 
	 * its list of strings. 
	 * The strings are collected, duplicates are removed and saved to 'TermList.txt'
	 */

	public static TranslationManager instance;
	[HideInInspector] public string saveState = "";
	void Awake(){
		if (instance != null) {
			Debug.LogError ("Only one 'TranslationManager' per scene allowed."); 
		}
		instance = this;
		translateableContents = new List<TranslatableContent>();
		translateableContents.Clear ();
	}

	[ReadOnlyInspector]public List<TranslatableContent> translateableContents;

	public void clearTranslatableContentScripts(){
		translateableContents.Clear ();
	}

	public void registerTranslateableContentScript(TranslatableContent tc){
		translateableContents.Add (tc);
	}

	public void generateTermList(){
		List <string> allCollectableTexts = new List<string>();

		foreach (TranslatableContent tc in translateableContents) {
			List<string> termList = tc.getTranslatableTerms ();
			if (termList != null) {
				foreach (string s in termList) {
					allCollectableTexts.Add (s);
				}
			} else {
				//Debug.LogWarning ("Not terms for '"+tc.ToString()+"' on '"+tc.gameObject.ToString()+"'.");
			}
		}

		//sort out the duplicates
		allCollectableTexts = allCollectableTexts.Distinct().ToList();
		//save it
		saveListToFile (allCollectableTexts);

	}


	void saveListToFile(List<string> rowData){

		// 파일 내용 만들기
		StringBuilder sb = new StringBuilder();

        for (int i = 0; i < rowData.Count; i++) {
			sb.AppendLine (rowData [i] );
		}

		//create path
		string directoryPath = Application.dataPath +"/Kings";
		string filePath = directoryPath+ "/TermList.txt";

		if(!Directory.Exists(directoryPath))
		{    
			//if it doesn't exist, create it
			Directory.CreateDirectory(directoryPath);
		}

		//save the content as file. 
		StreamWriter outStream = System.IO.File.CreateText(filePath);
		outStream.WriteLine(sb);
		outStream.Close();

		saveState = rowData.Count.ToString() + " entries saved to 'Kings/TermList.txt' at " + DateTime.Now.ToShortTimeString() +".";
	}


}
