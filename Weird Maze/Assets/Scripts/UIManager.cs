using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager;
    public Button restartButton;
    public GameObject failedPanel;
    public int sceneNumber;

    private void Awake()
    {
        sceneNumber = GameManager.gameManager.sceneNumber;
        if (uiManager == null)
        {
            uiManager = this;
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        GameOverUI();
        SuccessUI();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        restartButton.gameObject.SetActive(false);
        failedPanel.SetActive(false);
    }
    public void GameOverUI()
    {
        if (GameManager.gameManager.isGameOver)
        {
            StartCoroutine(FailedPanelActive());
        }
    }
    public void SuccessUI()
    {
        if (GameManager.gameManager.isSuccess)
        {
            StartCoroutine(LoadNextScene());
        }
    }

    IEnumerator FailedPanelActive()
    {
        yield return new WaitForSeconds(2f);
        failedPanel.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(4f);
        restartButton.gameObject.SetActive(true);
        SceneManager.LoadScene(sceneNumber +1);
    }
}
