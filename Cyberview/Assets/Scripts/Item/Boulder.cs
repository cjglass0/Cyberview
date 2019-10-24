using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private int ticsToBreak = 3;
    private float ticLength = 1.2f;
    private bool delaying = false;

    void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Drill" && !delaying)
        {
            ticsToBreak--;
            delaying = true;

            if (ticsToBreak < 0)
            {
                DestroyBoulder();
            } else
            {
                StartCoroutine(Dig());
            }
        }
    }

    IEnumerator Dig()
    {
        Debug.Log("Boulder -> Dig");
        
        //Blink
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(ticLength / 2);
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(ticLength / 2);

        delaying = false;
    }

    void DestroyBoulder()
    {
        Debug.Log("Boulder -> DestroyBoulder");
        Destroy(gameObject);
    }
}
