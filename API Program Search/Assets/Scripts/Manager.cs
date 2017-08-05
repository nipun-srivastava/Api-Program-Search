using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Search))]

// Class which manages the function calls
public class Manager : MonoBehaviour {

	Search programSearch;
	public InputField userInput;
	public ProgramList progList;

	ApiCredentials credentials;

	int offset;
	string currentSearch;

	void Awake()
	{
		InitLocalApiWithCredentials ();										// Initialize credentials

		offset = 0;
		programSearch = GetComponent<Search> ();

		if (programSearch) {
			try {
				programSearch.Initializing (credentials);
				programSearch.onSearchReturned += HandleOnSearchReturned; 	// Handles items returned from new  search

				userInput.onEndEdit.AddListener (OnNewSearchInput);			
				progList.onScrollBottom += HandleOnEndOfList;				// Handles what happens when scrolled
			} catch (UnityException e) {
				Debug.Log (e);			
			}
		} else
			Debug.LogError ("Couldn't Find Search, failed to initialize");
	}

	void InitLocalApiWithCredentials()										//Fetch api key and id from apicredentials.json
	{
		TextAsset credentialsJson = Resources.Load ("ApiCredentials") as TextAsset;
		credentials = JsonUtility.FromJson<ApiCredentials> (credentialsJson.text);
	}

	#region Functions for Additional Searchess

	void HandleOnSearchReturned(ProgramInfo[] programs)						// Add result items to list
	{
		progList.AddItems (programs);
	}

	public void OnNewSearchInput(string searchString)	     	 			// To be called when user enters new item in input field
	{
		progList.ClearItems ();

		offset = 0;
		currentSearch = searchString;

		programSearch.SearchByName (currentSearch, offset);
	}

	void HandleOnEndOfList()												// To be called when scroll bar reaches end
	{
		if (!programSearch.requesting) 
		{
			offset += ProgramList.SinglePageCount;
			programSearch.SearchByName (currentSearch, offset);
		}
	}

	#endregion
}
