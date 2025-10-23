using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;

    [Header("Characters")]
    [SerializeField] GameObject[] aiCharacters;
    [SerializeField] CharacterSceneSpawnPos[] spawnPosistions;
    [SerializeField] List<GameObject> spawnedCharacters = new List<GameObject>();

    [SerializeField] List<CharacterSpawner> spawners = new List<CharacterSpawner>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnCharacter(CharacterSpawner spawner)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            spawners.Add(spawner);
            spawner.SpawnCharacter();
        }
    }

    void DespawnAllCharacters()
    {
        foreach(GameObject character in spawnedCharacters)
        {
            character.GetComponent<NetworkObject>().Despawn();
        }
    }
}

[System.Serializable]
public class CharacterSceneSpawnPos
{
    public GameObject character;
    public Vector3 scenePos;
}