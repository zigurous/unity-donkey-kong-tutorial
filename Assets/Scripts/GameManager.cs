using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance;
    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<GameManager>();
                if (m_Instance == null)
                {
                    GameObject go = new GameObject("Game Manager");
                    m_Instance = go.AddComponent<GameManager>();
                }
            }
            return m_Instance;
        }
    }

    private const int NUM_LEVELS = 2;

    public int level { get; private set; } = 0;
    public int lives { get; private set; } = 3;
    public int score { get; private set; } = 0;

    private void Awake()
    {
        if (m_Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (m_Instance == this) {
            m_Instance = null;
        }
    }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        lives = 3;
        score = 0;

        LoadLevel(1);
    }

    private void LoadLevel(int level)
    {
        this.level = level;

        if (level > NUM_LEVELS)
        {
            // Start over again at level 1 once you have beaten all the levels
            // You can also load a "Win" scene instead
            LoadLevel(1);
            return;
        }

        Camera camera = Camera.main;

        // Don't render anything while loading the next scene to create a simple
        // scene transition effect
        if (camera != null) {
            camera.cullingMask = 0;
        }

        Invoke(nameof(LoadScene), 1f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(level);
    }

    public void LevelComplete()
    {
        score += 1000;
        LoadLevel(level + 1);
    }

    public void LevelFailed()
    {
        lives--;

        if (lives <= 0) {
            NewGame();
        } else {
            LoadLevel(level);
        }
    }

}
