using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    public int tailleX = 10;
    public int tailleZ = 10;
    public int nbCristals = 1;
    public int nbSpawn = 1;
    public int PourcentCaseTop;
    public int PourcentCaseMid;
    public int PourcentCaseBot;
    float sizeXcase;
    float sizeZcase;
    public GameObject wallBot;
    public GameObject wallMid;
    public GameObject wallTop;
    public GameObject sol;
    public GameObject cristal;
    public GameObject spawnEnnemy;

    private bool cristalSpawned = false;
    public Vector2[] posCrist;
    public GameObject[,] grille;
    // Start is called before the first frame update
    void Start()
    {
        posCrist = new Vector2[nbCristals];
        sizeXcase = wallBot.transform.localScale.x;
        sizeZcase = wallBot.transform.localScale.z;
        sol.transform.localScale = new Vector3((tailleX*sizeXcase/10)-0.2f, 1, (tailleZ*sizeZcase/10)-0.2f);
        Instantiate(sol, new Vector3(tailleX, 0, tailleZ), Quaternion.identity);
        Generat2();
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();

    }

    private void Update()
    {
         
    }

    void Generat2()
    {
        int caseTop = 0;
        int caseMid = 0;
        int caseBot = 0;
        int tour = 0;
        grille = new GameObject[tailleX, tailleZ];
        for (int i = nbCristals; i > 0; i--) //Place les cristaux sur la map
        {
            int a = Random.Range(0, tailleX);
            int b = Random.Range(0, tailleZ);
            if (grille[a, b] == null)
            {
                cristal.transform.position = new Vector3(a*sizeXcase, wallTop.transform.localScale.y + cristal.transform.localScale.y, b*sizeZcase);
                wallTop.transform.position = new Vector3(a * sizeXcase, wallTop.transform.localScale.y / 2, b * sizeZcase);
                grille[a, b] = Instantiate(cristal, gameObject.transform);
                Instantiate(wallTop, gameObject.transform);
                caseTop++;
                posCrist[tour] = new Vector2(a, b);
                tour++;
            }
        }

        foreach (GameObject crist in grille) // Cree les 8 plateformes Top autour du cristal
        {
            if (crist != null)
            {
                if (crist.tag == "cristal")
                {
                    int a = (int)(crist.transform.position.x/sizeXcase);
                    int b = (int)(crist.transform.position.z/sizeZcase);
                    for (int i = a - 1; i <= a + 1; i++)
                    {
                        for (int j = b - 1; j <= b + 1; j++)
                        {
                            if (i > 0 && j > 0 && i < tailleX && j < tailleZ)
                            {
                                if (grille[i, j] == null)
                                {
                                    wallTop.transform.position = new Vector3(i*sizeXcase, wallTop.transform.localScale.y/2, j*sizeZcase);

                                    grille[i, j] = Instantiate(wallTop, gameObject.transform);
                                    caseTop++;
                                }
                            }
                        }
                    }
                }
            }
        }

        float nbCaseMaxTemp = ((tailleX * tailleZ) / 100)*PourcentCaseTop - caseTop;
        int caseTopRestantes = Random.Range((int)(nbCaseMaxTemp / 3), (int)nbCaseMaxTemp);
        tour = 2;
        while(caseTopRestantes > 0) 
        { 
            foreach (GameObject crist in grille) // Cree d'autres plateformes Top
            {
                if (crist != null && crist.tag == "cristal")
                {
                 
                        int a = (int)(crist.transform.position.x / sizeXcase);
                        int b = (int)(crist.transform.position.z / sizeZcase);
                        for (int i = a - tour; i <= a + tour; i++)
                        {
                            for (int j = b - tour; j <= b + tour; j++)
                            {
                                if (i > 0 && j > 0 && i < tailleX && j < tailleZ)
                                {
                                    if (grille[i, j] == null && caseTopRestantes > 0)
                                    {
                                        int chance = Random.Range(0, 100);
                                        if (chance < PourcentCaseTop)
                                        {
                                            wallTop.transform.position = new Vector3(i * sizeXcase, wallTop.transform.localScale.y / 2, j * sizeZcase);
                                            grille[i, j] = Instantiate(wallTop, gameObject.transform);
                                            caseTopRestantes--;
                                        }
                                    }
                                }
                            }
                        }
                 
                }
            }
            tour++;
        }
        //_______________Pour poser les premières plateformes mid
        tour = 1;
        foreach (GameObject platTop in grille)
        {
            if (platTop != null && platTop.tag == "top")
            {
                int a = (int)(platTop.transform.position.x / sizeXcase);
                int b = (int)(platTop.transform.position.z / sizeZcase);
                for (int i = a - tour; i <= a + tour; i++)
                {
                    for (int j = b - tour; j <= b + tour; j++)
                    {
                        if (i > 0 && j > 0 && i < tailleX && j < tailleZ)
                        {
                            if (grille[i, j] == null)
                            {
                                wallMid.transform.position = new Vector3(i * sizeXcase, wallMid.transform.localScale.y / 2, j * sizeZcase);
                                grille[i, j] = Instantiate(wallMid, gameObject.transform);
                                caseMid++;
                            }
                        }
                    }
                }
            }
        }
        tour++;
        //______________Faire d'autres plateformes Mid
        nbCaseMaxTemp = ((tailleX * tailleZ) / 100) * PourcentCaseMid - caseMid;
        int caseMidRestantes = Random.Range((int)(nbCaseMaxTemp / 3), (int)nbCaseMaxTemp);
        while (caseMidRestantes > 0)
        {
            foreach (GameObject platTop in grille)
            {
                if (platTop != null && platTop.tag == "top")
                {
                    int a = (int)(platTop.transform.position.x / sizeXcase);
                    int b = (int)(platTop.transform.position.z / sizeZcase);
                    for (int i = a - tour; i <= a + tour; i++)
                    {
                        for (int j = b - tour; j <= b + tour; j++)
                        {
                            if (i > 0 && j > 0 && i < tailleX && j < tailleZ)
                            {
                                if (grille[i, j] == null && caseMidRestantes>0)
                                {

                                    int chance = Random.Range(0, 100);
                                    if (chance < PourcentCaseMid)
                                    {
                                        wallMid.transform.position = new Vector3(i * sizeXcase, wallMid.transform.localScale.y / 2, j * sizeZcase);
                                        grille[i, j] = Instantiate(wallMid, gameObject.transform);
                                        caseMidRestantes--;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //______Pour poser les premières Bot
        tour = 1;
        foreach (GameObject platMid in grille)
        {
            if (platMid != null && platMid.tag == "mid")
            {
                int a = (int)(platMid.transform.position.x / sizeXcase);
                int b = (int)(platMid.transform.position.z / sizeZcase);
                for (int i = a - tour; i <= a + tour; i++)
                {
                    for (int j = b - tour; j <= b + tour; j++)
                    {
                        if (i > 0 && j > 0 && i < tailleX && j < tailleZ)
                        {
                            if (grille[i, j] == null)
                            {
                                wallBot.transform.position = new Vector3(i * sizeXcase, wallBot.transform.localScale.y / 2, j * sizeZcase);
                                grille[i, j] = Instantiate(wallBot, gameObject.transform);
                                caseBot++;
                            }
                        }
                    }
                }
            }
        }
        tour++;
        //_____________________Creer d'autres plateformes Bot
        nbCaseMaxTemp = ((tailleX * tailleZ) / 100) * PourcentCaseBot - caseBot;
        int caseBotRestantes = Random.Range((int)(nbCaseMaxTemp / 3), (int)nbCaseMaxTemp);
        while (caseBotRestantes > 0)
        {
            foreach (GameObject platMid in grille)
            {
                if (platMid != null && platMid.tag == "mid")
                {
                    int a = (int)(platMid.transform.position.x / sizeXcase);
                    int b = (int)(platMid.transform.position.z / sizeZcase);
                    for (int i = a - tour; i <= a + tour; i++)
                    {
                        for (int j = b - tour; j <= b + tour; j++)
                        {
                            if (i > 0 && j > 0 && i < tailleX && j < tailleZ)
                            {
                                if (grille[i, j] == null && caseBotRestantes > 0)
                                {

                                    int chance = Random.Range(0, 100);
                                    if (chance < PourcentCaseBot)
                                    {
                                        wallBot.transform.position = new Vector3(i * sizeXcase, wallBot.transform.localScale.y / 2, j * sizeZcase);
                                        grille[i, j] = Instantiate(wallBot, gameObject.transform);
                                        caseBotRestantes--;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //______Spawner ennemy
        int casesVides = 0;
        Vector2 bestPosTemp = Vector2.zero;
        Vector2[] posLoin = new Vector2[nbCristals];
        tour = 0;
        float k = 0;
        float l = 0;
        foreach (Vector2 crist in posCrist)
        {
            float DistMax = 0;
            float indexX = 0;
            float indexZ = 0;
            foreach (GameObject blanc in grille)
            {
                if (indexX > tailleX)
                {
                    indexZ++;
                    indexX = 0;
                }


                if (blanc == null)
                {
                    float distTemp = Vector2.Distance(new Vector2(indexX, indexZ), crist);
                    casesVides++;
                    if (distTemp > DistMax)
                    {
                        DistMax = distTemp;
                        bestPosTemp = new Vector2(indexX, indexZ);
                    }
                    //Debug.Log(indexX + "   " + indexZ);
                }


                indexX++;
            }
            //Debug.Log(bestPosTemp + "      " + DistMax);
            posLoin[tour] = bestPosTemp;
            tour++;
            k += bestPosTemp.x;
            l += bestPosTemp.y;
        }
        int m = (int)k / tour;
        int n = (int)l / tour;
        tour = 0;
        spawnEnnemy.transform.position = new Vector3(m*2,0, n*2);
        Debug.Log(m+ "     " +n);
        int spawnActive = 0;
        if(grille[m,n] == null)
        {
            Debug.Log("ok");
            grille[m, n] = Instantiate(spawnEnnemy, gameObject.transform);
        }
        /* else
         {
             while (spawnActive < nbSpawn)
             {
                 for (int i = m - tour; i < m + tour; i++)
                 {
                     for (int j = n - tour; j < n + tour; j++)
                     {
                         if (grille[i, j] == null && spawnActive < nbSpawn)
                         {
                             grille[m, n] = Instantiate(spawnEnnemy, gameObject.transform);
                             spawnActive++;
                         }
                     }
                 }
                 tour++;*/
    }
}
        //Debug.Log(casesVides);
    

