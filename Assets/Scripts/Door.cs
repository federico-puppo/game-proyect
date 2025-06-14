using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private string targetSceneName;

    // 🔸 Esto todavía no lo usamos
    [SerializeField] private GameObject styleSelectionUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 🔸 Esto todavía no lo usamos
            ShowStyleSelection();

            // ✅ Esto sí lo usamos: guardar la escena real y pasar a la UI
            GameManager.Instance.SetNextScene(targetSceneName);
            SceneManager.LoadScene("MentalUI");
        }
    }

    // 🔸 Esto todavía no lo usamos
    private void ShowStyleSelection()
    {
        if (styleSelectionUI != null)
        {
            styleSelectionUI.SetActive(true);
            GameManager.Instance.UpdateStyleOptions();
        }
    }

    // 🔸 Esto todavía no lo usamos
    public void SelectStyle(string style)
    {
        if (styleSelectionUI != null)
        {
            styleSelectionUI.SetActive(false);
        }

        switch (style)
        {
            case "Aggressive":
                if (GameManager.Instance.CanUseStyle("Aggressive"))
                {
                    GameManager.Instance.ChooseAggressive();
                    LoadTargetScene();
                }
                break;
            case "Investigative":
                if (GameManager.Instance.CanUseStyle("Investigative"))
                {
                    GameManager.Instance.ChooseInvestigative();
                    LoadTargetScene();
                }
                break;
            case "Stealth":
                if (GameManager.Instance.CanUseStyle("Stealth"))
                {
                    GameManager.Instance.ChooseStealth();
                    LoadTargetScene();
                }
                break;
        }
    }

    // 🔸 Esto todavía no lo usamos
    private void LoadTargetScene()
    {
        // Puedes usar SceneManager.LoadScene(targetSceneName);
    }
}
