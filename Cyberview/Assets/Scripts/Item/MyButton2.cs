using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton2 : MonoBehaviour
{
    public MonoBehaviour target, target2;
    public bool pressed = false;
    public int pressCount = 0;
    bool checkForExit;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (checkForExit && pressed)
        {
            checkForExit = false;
            StartCoroutine(ExitCheck());
        }
    }

    IEnumerator ExitCheck()
    {
        Vector3 checkPos = gameObject.transform.position;
        Vector2 size = new Vector2(6.56f, 2.27f);

        List<Collider2D> collidersAtCheckLocation = new List<Collider2D>(Physics2D.OverlapBoxAll(checkPos, size, 0));
        for (int i = collidersAtCheckLocation.Count - 1; i >= 0; i--)
        {
            if (!(collidersAtCheckLocation[i].gameObject.name == "_Player" || collidersAtCheckLocation[i].gameObject.tag == "HeavyBlock")) {
                collidersAtCheckLocation.Remove(collidersAtCheckLocation[i]); 
            }
        }
        if (collidersAtCheckLocation.Count == 0)
        {
            //no object on collider
            if (target is ActivatedBySwitchInterface a)
            {
                a.switchTurnedOff();
                pressed = false;
                Debug.Log("lower call1");
            }
            if (target2 != null)
            {
                if (target2 is ActivatedBySwitchInterface a2)
                {
                    a2.switchTurnedOff();
                }
            }
            
        }
        Debug.Log("collidersAtCheckLocation.Count" + collidersAtCheckLocation.Count);
        yield return new WaitForSeconds(.5f);
        if (collidersAtCheckLocation.Count != 0) checkForExit = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" || collision.gameObject.tag == "HeavyBlock")
        {
            checkForExit = true;
            if (target is ActivatedBySwitchInterface a)
            {
                a.switchTurnedOn();
                pressed = true;
            }
            if (target2 != null)
            {
                if (target2 is ActivatedBySwitchInterface a2)
                {
                    a2.switchTurnedOn();
                }
            }
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 9 || collision.gameObject.tag == "HeavyBlock") &&
            collision.gameObject.GetComponent<Rigidbody2D>().velocity.y <= 0.1 &&
            collision.gameObject.transform.position.y > transform.position.y + 2)
        {
            if (target is ActivatedBySwitchInterface a && pressCount == 0)
            {
                a.switchTurnedOn();
                pressed = true;
            }
            if (target2 is ActivatedBySwitchInterface a2 && pressCount == 0)
            {
                a2.switchTurnedOn();
                pressed = true;
            }
            pressCount += 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == 9 || collision.gameObject.tag == "HeavyBlock") && pressed)
        {
            if (pressCount == 1 && target is ActivatedBySwitchInterface a)
            {
                a.switchTurnedOff();
            }
            if (pressCount == 1 && target2 is ActivatedBySwitchInterface a2)
            {
                a2.switchTurnedOff();
            }
            pressCount -= 1;
            if (pressCount <= 0)
            {
                pressed = false;
            }
        }
    }*/

    
}
