using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    [SerializeField] private GameObject styleSelectionUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowStyleSelection();
        }
    }

    private void ShowStyleSelection()
    {
        if (styleSelectionUI != null)
        {
            styleSelectionUI.SetActive(true);
            
            // Actualizar las opciones disponibles basado en los items equipados
            GameManager.Instance.UpdateStyleOptions();
        }
    }

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

    private void LoadTargetScene()
    {
        // Aquí iría la lógica para cargar la escena objetivo
        // Puedes usar SceneManager.LoadScene(targetSceneName);
    }
}
