using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blocksMng : MonoBehaviour
{
    public int haut = 0;
    public int bas = 0;
    public int gauche = 0;
    public int droit = 0;
    Vector3 scale;
    Vector3 scaleStair;
    Vector3 posBlock;
    public GameObject stairs;
    public Vector2 posInGrille;
    // Start is called before the first frame update
    void Start()
    {
        posBlock = transform.position;
        scale = transform.localScale;
        scaleStair = stairs.transform.localScale;
    }

    // Update is called once per frame  x * sizeCaseX + sizeCaseX / 2
    void Update()
    {//Mettre les stairs dans la public grille pour en faire pop seulement 1
        if (haut >= 5)
        {   
            Instantiate(stairs, new Vector3(posBlock.x, scale.y-1, posBlock.z + scale.z / 2 + 2), Quaternion.LookRotation(Vector3.right));
            haut = 0;
        }
        if (bas >= 5)
        {
            Instantiate(stairs, new Vector3(posBlock.x, scale.y - 1, posBlock.z - scale.z / 2 - 2), Quaternion.LookRotation(Vector3.left));
            bas = 0;
        }
        if (gauche >= 5)
        {
            Instantiate(stairs, new Vector3(posBlock.x - scale.x / 2 - 2, scale.y - 1, posBlock.z), Quaternion.identity);
            gauche = 0;
        }
        if (droit >= 5)
        {
            Instantiate(stairs, new Vector3(posBlock.x + scale.x / 2 + 2, scale.y - 1, posBlock.z), Quaternion.LookRotation(Vector3.back));
            droit = 0;
        }
        
    }
    void OnTriggerEnter(Collider col)
    {

        Vector3 pos = col.gameObject.transform.position;
        if(col.gameObject.tag == "ennemy")
        {
            if (pos.z > posBlock.z && pos.x > posBlock.x - scale.x / 2 && pos.x < posBlock.x + scale.x / 2)
            {
                haut++;
                Destroy(col.gameObject);
            }
            if (pos.z < posBlock.z && pos.x > posBlock.x - scale.x / 2 && pos.x < posBlock.x + scale.x / 2)
            {
                bas++;
                Destroy(col.gameObject);
            }
            if (pos.x > posBlock.x && pos.z > posBlock.z - scale.z / 2 && pos.z < posBlock.z + scale.z / 2)
            {
                droit++;
                Destroy(col.gameObject);
            }
            if (pos.x < posBlock.x && pos.z > posBlock.z - scale.z / 2 && pos.z < posBlock.z + scale.z / 2)
            {
                gauche++;
                Destroy(col.gameObject);
            }
        }
    }
}
