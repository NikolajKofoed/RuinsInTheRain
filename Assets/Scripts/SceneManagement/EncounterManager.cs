using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : Singleton<EncounterManager>
{
    private Dictionary<int, bool> encounterChecker = new Dictionary<int, bool>();

    /// <summary>
    /// Checks if an encounter has already been completed.
    /// </summary>
    public bool HasEncounterBeenCompleted(int encounterId)
    {
        return encounterChecker.ContainsKey(encounterId) && encounterChecker[encounterId];
    }


    /// <summary>
    /// Marks an encounter as completed.
    /// </summary>
    public void CompleteEncounter(int encounterId)
    {
        if (encounterChecker.ContainsKey(encounterId))
        {
            encounterChecker[encounterId] = true;
        }
        else
        {
            encounterChecker.Add(encounterId, true);
        }
    }

    /// <summary>
    /// Optionally reset all encounters (e.g. for testing or new game)
    /// </summary>
    public void ResetAllEncounters()
    {
        encounterChecker.Clear();
    }
}


