using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{

    public static RoomManager Instance;

    private void Awake()
    {
        if (Instance) // Baþka bir room manager var mý bakar
        {
            Destroy(gameObject); //eðer varsa siler. Tek bir room manager olmalý
            return;
        }
        DontDestroyOnLoad(gameObject); //eðer bir room manager yoksa Instance ilgili obje olur ve sahne geçiþlerinde yok edilmez.
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex == 1) // Game scenedeyiz
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerManager"),Vector3.zero,Quaternion.identity);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
