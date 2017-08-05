using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;

public struct ApiCredentials
{
	public string apiKey;
	public string apiID;
}

// Class which manages the search
public class Search : MonoBehaviour {												// Search Class starts here

	public bool requesting;
	public Text waitText;

	public delegate void OnSearchReturned(ProgramInfo[] info);
	public OnSearchReturned onSearchReturned;

	string baseUrl = "https://external.api.yle.fi/v1/programs/";

	ApiCredentials credentials;

	public void Initializing (ApiCredentials credentials) 							// Initialize credentials with api key and id
	{
		if (credentials.apiID == null || credentials.apiID == "")
			throw new UnityException ("ApiId failed to initialize search");
		if (credentials.apiKey == null || credentials.apiKey == "")
			throw new UnityException ("ApiId failed to initialize search");
	
		this.credentials = credentials;
	}

	#region Generating URL and sending request

	public void SearchByName(string name, int offset = 0) 							// Starting Coroutine to send request to api
	{
		StartCoroutine (SearchRequest (name, offset));
	}

	string GenerateUrl(string query, int offset) 									// Generating URL based on the entered keyword
	{
		return baseUrl + "items.json?q=" + query + "&mediaobject=video&limit=10&offset=" + offset + "&app_id=" + credentials.apiID + "&app_key=" + credentials.apiKey;

	}

	IEnumerator SearchRequest(string name, int offset)
	{
		requesting = true;
		waitText.enabled = true;
		UnityWebRequest searchRequest = UnityWebRequest.Get (GenerateUrl (name, offset));
		yield return searchRequest.Send ();

		requesting = false;
		waitText.enabled = false;

		if (searchRequest.isError || searchRequest.responseCode != 200) 
		{ 			// Checking if search request resulted in an error.
			Debug.LogError ("Search Request failure"); 		
			Debug.LogError ("Error: " + searchRequest.error);						// Show Error 			 
			Debug.LogError ("Response Code:" + searchRequest.responseCode);			// Show Response Code
		} 
		else 
		{
			ProgramData progInfo = JsonUtility.FromJson<ProgramData> (searchRequest.downloadHandler.text);
			onSearchReturned (progInfo.data);
		}
	}

	#endregion
}																					// Class Ends
