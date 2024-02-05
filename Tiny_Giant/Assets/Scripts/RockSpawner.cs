using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


public class RockSpawner : NetworkBehaviour
{
    public Transform leftBound;
    public Transform rightBound;

    private Vector3 leftVector;
    private float distanceBetweenBounds;

    public GameObject rockPrefab1;
    public GameObject rockPrefab2;

    public float rockDelay = 1f;

    private void Start() {
        leftVector = (transform.position - leftBound.position).normalized;
        distanceBetweenBounds = (leftBound.position - rightBound.position).magnitude;
        StartCoroutine(SpawnRocks());
    }

    private IEnumerator SpawnRocks() {
        yield return new WaitForSeconds(5f);
        while (true) {
            yield return new WaitForSeconds(rockDelay);
            int randomInt = Random.Range(0, 1);
            var randomOffsetx = Random.Range(-distanceBetweenBounds, distanceBetweenBounds) / 2;
            var rock = randomInt == 0 ? Runner.Spawn(rockPrefab1, transform.position + leftVector * randomOffsetx, Quaternion.identity) 
                                        : Runner.Spawn(rockPrefab2, transform.position + leftVector * randomOffsetx, Quaternion.identity);
            var randomForceX = Random.Range(0, 10);
            var randomForceZ = Random.Range(-10, 10);
            rock.GetComponent<Rigidbody>().AddForce(randomForceX,0,randomForceZ, ForceMode.Impulse);
        }
    }
}
