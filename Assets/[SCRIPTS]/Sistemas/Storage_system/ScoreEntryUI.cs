using UnityEngine;
using TMPro;

public class ScoreEntryUI : MonoBehaviour
{
    public TMP_Text trackText;
    public TMP_Text timeText;
    public TMP_Text scoreText;
    public TMP_Text dateText;

    public void Setup(RaceResult data)
    {
        trackText.text = data.trackName;

        int minutes = (int)(data.time / 60);
        int seconds = (int)(data.time % 60);
        int milliseconds = (int)((data.time - (int)data.time) * 1000);

        timeText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
        scoreText.text = "Score: " + data.score;
        dateText.text = data.date;
    }
}