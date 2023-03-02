using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private GameObject m_pausePanel;

    [SerializeField]
    private Button m_resumeBtn;

    [SerializeField]
    private Button m_replayBtn;

    [SerializeField]
    private GameObject m_ShowP1Win;

    [SerializeField]
    private GameObject m_ShowP2Win;


    public void PauseGame()
    {
        Time.timeScale = 0f;
        m_pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        m_pausePanel.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ShowWin(int numberPlayer)
    {
        if (numberPlayer == 1)
        {
            m_ShowP1Win.SetActive(true);
        }
        else if (numberPlayer == 2)
        {
            m_ShowP2Win.SetActive(true);
        }
    }
}
