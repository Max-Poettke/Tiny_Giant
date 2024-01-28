using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.InputSystem;

public class BowChest : MonoBehaviour
{
    [SerializeField] private Transform lid;

    [SerializeField] private float openTime;

    [SerializeField] private GameObject decorativeBow;
    private FirstPersonController controller;

    private Bow _bow;

    public enum ChestState
    {
        Waiting, Opening, Open, Empty
    }

    public ChestState _state;
    // Start is called before the first frame update
    void Start()
    {
        _state = ChestState.Waiting;
    }

    public IEnumerator OpenChest()
    {
        var time = 0f;
        var startRotation = lid.rotation;
        var endRotation = Quaternion.Euler(new Vector3(-100, 90, 0));
        _state = ChestState.Opening;
        while (time < openTime)
        {
            lid.rotation = Quaternion.Lerp(startRotation, endRotation, time / openTime);
            time += Time.deltaTime;
            yield return null;
        }

        _state = ChestState.Open;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _bow = other.GetComponentInChildren<Bow>(true);
            controller = other.GetComponent<FirstPersonController>();
            controller._bowChest = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller._bowChest = null;
        }
    }

    public void PickUpBow()
    {
        if (!_bow) return;
        
        _bow.gameObject.SetActive(true);
        decorativeBow.SetActive(false);
    }
}
