using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerEnnemy : MonoBehaviour
{

    public GameObject ennemy;
    public int nbEnToSpawn;
    public float dellaySpawn;
    private bool readyToSpawn;
    public float temps;
    int enInGame;

    private void Start()
    {
        temps = 0;
        enInGame = 0;
        readyToSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        temps += Time.deltaTime;
        if (temps >= dellaySpawn && enInGame < nbEnToSpawn)
        {
            Instantiate(ennemy, transform.position, Quaternion.identity);
            temps = 0;
            enInGame++;
        }
    }
}
