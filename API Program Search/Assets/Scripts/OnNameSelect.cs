using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class to define the button functionality
public class OnNameSelect : MonoBehaviour {													// Class OnNameSelect Starts here

	public GameObject information;

	GameObject content;

	float itemHeight;
	ProgramList programList;
	ScrollRect scroll;

	void Awake()
	{
		content = GameObject.Find ("Contents");
		programList = GameObject.Find ("Results").GetComponent<ProgramList> ();
		itemHeight = 10f;
		scroll = GameObject.Find ("Results").GetComponent<ScrollRect>();
	}

	public void OnButtonClick()
	{ 

		foreach (Transform child in content.transform) 
		{
			child.gameObject.SetActive (false);												// Setting the result item prefabs in hierarchy inactive.
		}
			
		programList.ScrollToTop (scroll);													// Scroll back to the top
		GameObject info =Instantiate (information) as GameObject;							// Creating new prefab to display information
		info.transform.SetParent (content.transform);

		info.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0f, -itemHeight); 
		info.transform.localScale = Vector2.one;

		#region Displaying Information 

		var uiTitle = info.transform.Find ("Title").GetComponent<Text> ();
		uiTitle.text = "Showing information for: " + programList._progTitle[int.Parse(transform.gameObject.name)];

		var uiDescription = info.transform.Find ("Description").GetComponent<Text> ();
		uiDescription.text = "<b>Description: </b> " + programList._progDesc[int.Parse(transform.gameObject.name)];

		var uiVidType = info.transform.Find ("Video_Type").GetComponent<Text> ();
		uiVidType.text = "<b>Media Type: </b>" + programList._progVidType[int.Parse(transform.gameObject.name)];

		var uiSeasonNumber = info.transform.Find ("Season_Number").GetComponent<Text> ();
		uiSeasonNumber.text = "<b>Season Number: </b> " + programList._progSeasonNumber[int.Parse(transform.gameObject.name)];

		var uiType = info.transform.Find("Type").GetComponent<Text>();
		uiType.text = "<b>Type of the video is: </b>" + programList._progtype[int.Parse(transform.gameObject.name)];

		var uiDateModified = info.transform.Find ("Index_Data_Modified").GetComponent<Text> ();
		uiDateModified.text = "<b>Data Modified: </b>" + programList._progIndexDataModified[int.Parse(transform.gameObject.name)];

		#endregion
	}
}																							// Class ends
