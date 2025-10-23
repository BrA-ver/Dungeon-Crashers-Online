using UnityEngine;
using Unity.Netcode;

public class CharacterSpawner : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] GameObject character;
    [SerializeField] GameObject spawnedCharacter;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WorldAIManager.instance.SpawnCharacter(this);
        gameObject.SetActive(false);
    }

    public void SpawnCharacter()
    {
        if (character == null) return;
        spawnedCharacter = Instantiate(character);

        spawnedCharacter.transform.position = transform.position;
        spawnedCharacter.transform.rotation = transform.rotation;
        spawnedCharacter.GetComponent<NetworkObject>().Spawn();
        
    }
}
