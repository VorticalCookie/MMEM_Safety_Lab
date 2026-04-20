using System;
using System.Reflection;
using System.Text.RegularExpressions;

[Serializable]
public class FUserData
{
    // Example fields
    public string userName;
    public int score;
    public float timeTaken;

    // Example dynamic data structure
    public OtherData[] otherData;

    [Serializable]
    public class OtherData
    {
        public string name;
        public string data;
    }

    /// <summary>
    /// Gets a string formatted in CSV for all field titles listed in FUserData
    /// </summary>
    /// <returns>The formatted string</returns>
    public string GetCSVTitles()
    {
        string value = string.Empty;

        FieldInfo[] fields = typeof(FUserData).GetFields(BindingFlags.Instance | BindingFlags.Public);

        for (int infoIndex = 0; infoIndex < fields.Length; infoIndex++)
        {
            string separatedString = Regex.Replace(fields[infoIndex].Name, "[a-z][A-Z]", match => $"{match.Value[0]} {char.ToUpper(match.Value[1])}");
            separatedString = char.ToUpper(separatedString[0]) + separatedString.Substring(1);

            value += separatedString;

            if (infoIndex < fields.Length - 1)
            {
                value += ", ";
            }
        }

        // Go through dynamic data
        if (otherData != null)
        {
            for (int index = 0; index < otherData.Length; index++)
            {
                value += ", " + otherData[index].name;
            }
        }

        return value;
    }

    /// <summary>
    /// Serializes this user data struct to CSV format.
    /// </summary>
    /// <returns>The formatted string</returns>
    public string SerializeToCSV()
    {
        string value = string.Empty;

        FieldInfo[] fields = typeof(FUserData).GetFields(BindingFlags.Instance | BindingFlags.Public);

        for (int infoIndex = 0; infoIndex < fields.Length; infoIndex++)
        {
            value += fields[infoIndex].GetValue(this);

            if (infoIndex < fields.Length - 1)
            {
                value += ", ";
            }
        }

        // Dynamic data
        if (otherData != null)
        {
            for (int index = 0; index < otherData.Length; index++)
            {
                value += ", " + otherData[index].data;
            }
        }

        return value;
    }
}
