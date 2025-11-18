using UnityEngine;

public class RaceEndHandler : MonoBehaviour
{
    [Header("Datos de la carrera (asignar o calcular)")]
    public string trackName = "Pista 1";
    public float raceTime = 75.3f;
    public int challengePoints = 300;

    public void SaveRaceResult()
    {
     
        RaceRecord record = new RaceRecord(trackName, raceTime, challengePoints);

        SaveManager.SaveRecord(record);

        Debug.Log("Registro guardado: " + record.trackName);
    }
}