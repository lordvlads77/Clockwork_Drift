using System;

[Serializable]
public class RaceRecord
{
    public string trackName;
    public float trackTime;
    public int challengePoints;
    public string date;

   
    public RaceRecord(string name, float time, int points)
    {
        trackName = name;
        trackTime = time;
        challengePoints = points;

        
        date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
    }
}