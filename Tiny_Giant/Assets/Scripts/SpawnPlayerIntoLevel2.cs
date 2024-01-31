using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class SpawnPlayerIntoLevel2 : NetworkBehaviour
{
   private GameObject _player;
   private CinemachineStoryboard _cine;
   private float fadeTime = 1f;

   private void Start()
   {
      _player = GameObject.FindWithTag("Player");
      if (!_player) return;
      _cine = _player.GetComponentInChildren<CinemachineStoryboard>();
      _player.GetComponentInChildren<NetworkRigidbody3D>().Teleport(transform.position);
      StartCoroutine(FadeBack());

   }

   private IEnumerator FadeBack()
   {
      yield return new WaitForSeconds(2f);
      var time = 0f;
      while (time < fadeTime)
      {
         _cine.m_Alpha = Mathf.Lerp(1f, 0f, time / fadeTime);
         time += Time.deltaTime;
         yield return null;
      }

      _player.GetComponent<FirstPersonController>().playerCanMove = true;
      _cine.m_Alpha = 0f;
   }
}
