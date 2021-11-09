using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using edu.tnu.dgd.game;

public class DontDestroyOnLoadController : MonoBehaviour
{

    private static DontDestroyOnLoadController _instance;

    public static DontDestroyOnLoadController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DontDestroyOnLoadController>();
            }

            return _instance;
        }
    }

    private bool _bluetoothConnected = false;

    
    public void RestartLevel()
    {
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(6);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public bool bluetoothConnected
    {
        get
        {
            return _bluetoothConnected;
        }

        set
        {
            _bluetoothConnected = value;
        }
    }


}
