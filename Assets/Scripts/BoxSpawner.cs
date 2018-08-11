using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour {

    [SerializeField]
    Box boxPrefab;

    public float spawnTime = 1f;

    private void Start() {
        StartCoroutine(RepeatedlySpawnBoxes());
    }

    IEnumerator RepeatedlySpawnBoxes() {
        while (true) {
            Box spawnedBox = Instantiate(this.boxPrefab, this.transform.position, Quaternion.identity, this.transform.parent);
            yield return new WaitForSeconds(this.spawnTime);
        }
    }

}
