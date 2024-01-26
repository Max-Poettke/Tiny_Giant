using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeRaiser : MonoBehaviour
{
    public int torchCount;

    [SerializeField] private Vector3 raisedPos;

    private bool _raised;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_raised || torchCount < 2) return;
        _raised = true;
        StartCoroutine(RaiseBridge());
    }

    private IEnumerator RaiseBridge()
    {
        var time = 0f;
        const float raiseTime = 4.5f;
        var position = transform.localPosition;
        while (time < raiseTime)
        {
            transform.localPosition = Vector3.Slerp(position, raisedPos, time / raiseTime);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
