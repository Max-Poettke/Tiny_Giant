using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeRaiser : MonoBehaviour
{
    public int torchCount;

    private bool _midAnimation;
    [SerializeField] private Vector3 raisedPos;
    [SerializeField] private Vector3 loweredPos;

    private bool _raised;
    // Start is called before the first frame update
    void Start()
    {
        torchCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (_midAnimation) return;
        if (_raised && torchCount < 2) StartCoroutine(LowerBridge());
        if (!_raised && torchCount >= 2) StartCoroutine(RaiseBridge());
    }

    private IEnumerator RaiseBridge()
    {
        _raised = true;
        _midAnimation = true;
        var time = 0f;
        const float raiseTime = 4.5f;
        var position = transform.localPosition;
        while (time < raiseTime)
        {
            transform.localPosition = Vector3.Slerp(position, raisedPos, time / raiseTime);
            time += Time.deltaTime;
            yield return null;
        }

        _midAnimation = false;
    }

    private IEnumerator LowerBridge()
    {
        _midAnimation = true;
        _raised = false;
        yield return new WaitForSecondsRealtime(1.5f);
        var time = 0f;
        const float lowerTime = 5f;
        var position = transform.localPosition;
        while (time < lowerTime)
        {
            transform.localPosition = Vector3.Slerp(position, loweredPos, time / lowerTime);
            time += Time.deltaTime;
            yield return null;
        }

        _midAnimation = false;
    }
}
