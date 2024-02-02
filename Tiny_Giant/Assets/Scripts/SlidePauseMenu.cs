using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SlidePauseMenu : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Button resumeButton;
    private readonly Vector2 _position = new(0f, 0f);
    private readonly Vector2 _downPosition = new(0f, -400f);
    [SerializeField] private float slideTime = 0.5f;
    public bool moving;
    
    private void OnEnable()
    {
        StartCoroutine(SlideUp());
        if (GameObject.FindWithTag("Player").GetComponent<FirstPersonController>()._gamepad) resumeButton.Select();
    }

    private Vector2 _velocity = Vector2.zero;
    private IEnumerator SlideUp()
    {
        if (moving) yield break;
        moving = true;
        while (Mathf.Ceil(_rectTransform.anchoredPosition.y) < 0f)
        { 
          _rectTransform.anchoredPosition = Vector2.SmoothDamp(_rectTransform.anchoredPosition, _position, ref _velocity, slideTime);
          yield return null;
        }

        moving = false;
    }

    private void FixedUpdate()
    {
        InputSystem.Update();
    }

    public IEnumerator SlideDown()
    {
        if (moving) yield break;
        moving = true;
        var time = 0f;
        while (time < slideTime)
        {
            _rectTransform.anchoredPosition = Vector2.Lerp(_position, _downPosition, time / slideTime);
            time += Time.deltaTime;
            yield return null;
        }
        _rectTransform.anchoredPosition = _downPosition;
        moving = false;
        gameObject.SetActive(false);
    }
}
