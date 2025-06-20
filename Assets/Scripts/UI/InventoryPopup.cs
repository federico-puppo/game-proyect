using UnityEngine;
using TMPro;
using System.Collections;

public class InventoryPopup : MonoBehaviour
{
    public TMP_Text popupText;
    public float displayDuration = 2f;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        popupText.gameObject.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ShowPopup(message));
    }

    private IEnumerator ShowPopup(string message)
    {
        popupText.text = message;
        popupText.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        popupText.gameObject.SetActive(false);
    }
}