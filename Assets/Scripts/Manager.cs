using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Manager : MonoBehaviour
{
    [SerializeField] private Button reset = null;
    [SerializeField] private Button quit = null;
    private void Awake()
    {
        reset.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        quit.onClick.AddListener(() => Application.Quit());
    }
}
