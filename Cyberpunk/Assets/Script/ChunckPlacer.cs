using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunckPlacer : MonoBehaviour
{
    [Header("Start Chunk")]
    [SerializeField] private Chunck FirstChunk;
    [SerializeField] private Chunck[] chuncks;

    private List<Chunck> spawnerChunk = new List<Chunck>();
    private Transform player;

    private void Start()
    {
        player = NewPlayer.Instance.transform;
        spawnerChunk.Add(FirstChunk);
    }

    private void SpawnChunk()
    {
        Chunck newChunk = Instantiate(chuncks[Random.Range(0, chuncks.Length)]);
        newChunk.transform.position = new Vector3 (spawnerChunk[spawnerChunk.Count - 1].End.position.x - newChunk.Begin.localPosition.x,0);
        spawnerChunk.Add(newChunk);

        if(spawnerChunk.Count > 2)
        {
            Destroy(spawnerChunk[0].gameObject);
            spawnerChunk.RemoveAt(0);
        }
    }

    private void Update()
    {
        if(player.position.x + 7f >= spawnerChunk[spawnerChunk.Count - 1].End.position.x)
        {
            SpawnChunk();
        }

    }

}
