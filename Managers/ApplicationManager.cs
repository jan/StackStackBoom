using System;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    public enum Branch
    {
        Main,
        Dev,
        Playtest,
        Demo
    }

    private static ApplicationManager instance;
    public static ApplicationManager Instance
    {
        get
        {
            if (instance == null) instance = FindAnyObjectByType<ApplicationManager>();
            return instance;
        }
    }

    // This is an attack vector, of course. Nothing critical relies on this though.
    public static bool IsDev => Application.isEditor || AppDomain.CurrentDomain.BaseDirectory.Contains("janbromberger");
    public static bool IsPlaytest => AppDomain.CurrentDomain.BaseDirectory.Contains("Playtest");
    public static bool IsDemo => Application.platform == RuntimePlatform.WebGLPlayer;

    public static Branch GetBranch()
    {
        if (IsDev) return Branch.Dev;
        if (IsPlaytest) return Branch.Playtest;
        if (IsDemo) return Branch.Demo;
        return Branch.Main;
    }

    public GameManager GameManager => gameManager;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SettingsManager.LoadDefaults();
    }
}
