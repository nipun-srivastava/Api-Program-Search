using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]

// Class to Manage results of the search query sent to the api
public class ProgramList : MonoBehaviour {										// Class Starts here

	RectTransform resultHolder;

	float itemHeight;
	float itemWidhth;
	public GameObject listItem;																		
																				// Creating Lists to store information of the title selected.
	[HideInInspector]															
	public List<string> _progTitle = new List<string> ();						// Titles
	[HideInInspector]
	public List<string> _progDesc = new List<string>();							// Description
	[HideInInspector]
	public List<string> _progVidType = new List<string>();						// Video Type
	[HideInInspector]
	public List<string> _progSeasonNumber = new List<string>();					// Season Number
	[HideInInspector]
	public List<string> _progtype = new List<string>();							// Program Type
	[HideInInspector]
	public List<string> _progIndexDataModified = new  List<string>();			// Index Data Modified

	public static int SinglePageCount = 10;
	public delegate void OnScrollBottom();
	public OnScrollBottom onScrollBottom;

	List<GameObject> allItems = new List<GameObject> ();						// Store Prefab GameObjects

	void Awake()
	{
		resultHolder = transform.Find ("ViewPort/Contents") as RectTransform;
		itemWidhth   = resultHolder.sizeDelta.x;
		itemHeight 	 = resultHolder.sizeDelta.y / 10f;
		transform.GetComponent<ScrollRect> ().onValueChanged.AddListener (OnValueChanged);
	}

	void OnValueChanged(Vector2 newPosition)
	{
		if (allItems.Count > 0 && newPosition.y < 0)
			onScrollBottom ();
	}

	#region Information Prefab Creation

	public void ClearItems()										//Method to Clear the list 				
	{
		foreach (GameObject item in allItems) 
		{
			Destroy (item);
		}

		Destroy (GameObject.FindGameObjectWithTag ("Information"));	// Destroy the information showing prefab 

		allItems.Clear ();
	}

	public void ScrollToTop(ScrollRect scroll) 						// Method to scroll back up when information is displayed
	{
		scroll.normalizedPosition = new Vector2 (0, 1);
	}

	public void AddItems(ProgramInfo[] programs)					// Add items to list
	{
		foreach (ProgramInfo program in programs) 					// Loop Begins
		{
			var programTitle = program.title.fi;
		
			if (programTitle == null || programTitle == "")	
				continue;

			// Variables with information fields for the current title
			var progDescription = program.description.fi;
			var progVidType = program.video.type;
			var progSeasonNumber = program.partOfSeason.seasonNumber;
			var progType = program.partOfSeason.type;
			var progIndexModified = program.partOfSeason.indexDataModified;

			GameObject newItem = Instantiate (listItem) as GameObject;
			allItems.Add (newItem);									    //Instantiating the Prefab and assigning it title value.

			resultHolder.sizeDelta = new Vector2 (itemWidhth, allItems.Count * itemHeight);
			newItem.transform.SetParent (resultHolder);

			newItem.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0f, -itemHeight * allItems.Count);
			newItem.transform.localScale = Vector2.one;
			newItem.gameObject.name = allItems.IndexOf (newItem).ToString ();		// Setting name of prefab to its index in list.

			var uiTitle  = newItem.transform.Find ("Title").GetComponent<Text> ();
			uiTitle.text = allItems.IndexOf(newItem).ToString() + ". " + programTitle;
			
			// Adding each field to the respective list so that it can be fetched later.
			_progTitle.Add (programTitle);
			_progDesc.Add(progDescription);
			_progVidType.Add(progVidType);
			_progSeasonNumber.Add(progSeasonNumber);
			_progtype.Add(progType);
			_progIndexDataModified.Add(progIndexModified);
		}																			// Loop Ends
	
		IfNoResults ();
	}																			    // Function Ends

	void IfNoResults()															    // Function which handles cases with no results.
	{
		if (allItems.Count == 0) 
		{

			GameObject newItem = Instantiate (listItem) as GameObject;
			resultHolder.sizeDelta = new Vector2 (itemWidhth, allItems.Count * itemHeight);
			newItem.transform.SetParent (resultHolder);

			newItem.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0f, -itemHeight);
			newItem.transform.localScale = Vector2.one;

			var uiTitle  = newItem.transform.Find ("Title").GetComponent<Text> ();
			uiTitle.text = "No Results matching your search :(.Please enter a new keyword.";

			allItems.Add (newItem);													// Adding it to list so it can be cleared too.
		}
	}

	#endregion 
}																					// End of Class
