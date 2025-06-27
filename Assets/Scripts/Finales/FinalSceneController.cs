using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalSceneController : MonoBehaviour
{
    public Image[] splashImages;
    public TMP_Text[] narrativeTexts;
    public float delayBetweenSteps = 4f;
    public Button quitButton;

    private int step = 0;

    void Start()
    {
        foreach (var img in splashImages) img.gameObject.SetActive(false);
        foreach (var txt in narrativeTexts) txt.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

        StartCoroutine(PlaySequence());
    }

    private System.Collections.IEnumerator PlaySequence()
    {
        while (step < splashImages.Length && step < narrativeTexts.Length)
        {
            splashImages[step].gameObject.SetActive(true);
            narrativeTexts[step].gameObject.SetActive(true);
            yield return new WaitForSeconds(delayBetweenSteps);
            step++;
        }

        quitButton.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuPrincipal"); // si tenés un menú
    }
}
