using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Fusion;

public class Bow : NetworkBehaviour
{
    public GameObject arrows;
    public float maxHoldDuration = 2.5f;
    private NetworkObject arrow;
    public Transform cam;
    
    private float start = 0f;
    private float end = 0f;

    

    [SerializeField] private float shotPower = 2f;

    [SerializeField] private new Animation animation;
    
    private GameObject[] activeArrows;
    [SerializeField] private int maxActiveArrows = 10;
    private int pointer;
    
    private Coroutine drawBow;
    
    // Start is called before the first frame update
    void Start()
    {
        activeArrows = new GameObject[maxActiveArrows];
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnShoot(InputAction.CallbackContext context)
    {

        Debug.Log("Shoot");
        if (context.performed)
        {
            /*
            start = Time.time;
            arrow = Instantiate(arrows, transform.position + transform.TransformVector(0f, 0f, 0.3f), transform.rotation, transform.parent);
            */
            arrow = Runner.Spawn(arrows, transform.position + transform.TransformVector(0f, 0f, 0.3f), transform.rotation, Runner.LocalPlayer);
            arrow.gameObject.transform.SetParent(transform);
            if(activeArrows[pointer] != null) Destroy(activeArrows[pointer]);
            activeArrows[pointer++] = arrow.gameObject;
            pointer %= maxActiveArrows; 
            
            drawBow = StartCoroutine(MoveArrowBack());
        }

        
        if (context.canceled)
        {
            end = Time.time;

            if (end - start < minHoldDuration)
            {
                Destroy(arrow);
                StopCoroutine(drawBow);
                animation.Stop();
            }
            else
            {
                if(activeArrows[pointer] != null) activeArrows[pointer].GetComponent<Arrow>().Vanish();
                activeArrows[pointer++] = arrow;
                pointer %= maxActiveArrows; 
                
                StopCoroutine(drawBow);
                var rb = arrow.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
                var holdMultiplier = Mathf.Min(end - start, maxHoldDuration);
                rb.AddForce((cam.forward - cam.right / 35) * shotPower * holdMultiplier, ForceMode.Impulse); 
                arrow.transform.SetParent(null);
                animation.Play("Release", PlayMode.StopAll);
                animation["Release"].speed = 5f;
            }
        }
        
    }

    private IEnumerator DrawBow()
    {
        yield return new WaitForSeconds(minHoldDuration);
        var start1 = Time.time;
        animation.Play("DrawBow", PlayMode.StopAll);
        animation["DrawBow"].speed = 0.4f;
        while(Time.time - start1 < 1f)
        {
            arrow.transform.Translate(new Vector3(0, 0, -0.18f) * Time.deltaTime);
            yield return null;
        }
        animation["DrawBow"].speed = 0.2f;
        while(Time.time - start1 < 2.5f)
        {
            arrow.transform.Translate(new Vector3(0, 0, -0.18f) * Time.deltaTime);
            yield return null;
        }
    }
}
