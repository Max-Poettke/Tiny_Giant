using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Bow : MonoBehaviour
{
    public GameObject arrows;
    public float maxHoldDuration = 2.5f;
    private GameObject arrow;
    public Transform cam;
    private float start = 0f;
    private float end = 0f;

    [SerializeField]
    private float shotPower = 2f;

    [SerializeField] private Animation animation;
    
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(transform.parent.position, Vector3.right, camera.transform.rotation.eulerAngles.x - oldCam);
        //oldCam = camera.transform.rotation.eulerAngles.x;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            start = Time.time;
            arrow = Instantiate(arrows, transform.position + transform.TransformVector(0f, 0f, 0.3f), transform.rotation, transform.parent);
            StartCoroutine(MoveArrowBack());
        }

        
        if (context.canceled)
        {
            end = Time.time;
            StopCoroutine(MoveArrowBack());
            var rb = arrow.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            var holdMultiplier = Mathf.Min(end - start, maxHoldDuration);
            rb.AddForce(( cam.forward - cam.right / 35) * shotPower * holdMultiplier, ForceMode.Impulse);  //-cam.right / 35 damit der Pfeil leicht nach links fliegt
            arrow.transform.SetParent(null);
            animation.Play("Release", PlayMode.StopAll);
            animation["Release"].speed = 5f;
        }
        
    }

    public IEnumerator MoveArrowBack()
    {
        yield return new WaitForSeconds(0.3f);
        animation.Play("DrawBow", PlayMode.StopAll);
        animation["DrawBow"].speed = 0.5f;
        int time = 0;
        while(time++ < 90)
        {
            if (time == 30)
            {
                animation["DrawBow"].speed = 0.3f;
            }
            arrow.transform.Translate(new Vector3(0, 0, -0.18f) * Time.deltaTime);
            yield return null;
        }
    }
}
