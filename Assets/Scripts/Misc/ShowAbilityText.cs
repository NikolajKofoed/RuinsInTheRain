using System.Collections;
using UnityEngine;
using TMPro;


public class ShowAbilityText : MonoBehaviour
{
    public void Show(TextMeshProUGUI text, float time)
    {
        StartCoroutine(ShowNewAbilityText(text, time));
    }

    private IEnumerator ShowNewAbilityText(TextMeshProUGUI newAbilityText, float showAbilityTextTimer)
    {
        newAbilityText.enabled = true;
        yield return new WaitForSeconds(showAbilityTextTimer);
        Debug.Log("Deactivating ability text");
        newAbilityText.enabled = false;
    }
}

