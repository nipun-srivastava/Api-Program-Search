// Maintain Data here

[System.Serializable]
public struct ProgramData
{
	public ProgramInfo[] data;
}

[System.Serializable]
public struct ProgramInfo
{
	public ProgramTitle title;
	public ProgramDescription description;
	public ProgramVideo video;
	public ProgramPartOfSeason partOfSeason;
}

[System.Serializable]
public struct ProgramTitle 			// Store Program Titles
{
	public string fi;
}

[System.Serializable]
public struct ProgramDescription 	// Store Program Description
{
	public string fi;
}

[System.Serializable]
public struct ProgramVideo			// Store Program Video Information
{
	public string type;
}

[System.Serializable]
public struct ProgramPartOfSeason	// Store Season Information
{
	public string seasonNumber;
	public string type;
	public string indexDataModified;
}