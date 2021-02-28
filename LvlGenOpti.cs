using UnityEngine;
using UnityEngine.AI;
public class LvlGenOpti : MonoBehaviour
{
    //Taille de la map
    public int sizeX = 10;
    public int sizeZ = 10;

    //Le nombe total de prefabs
    public int nbCristaux = 1;
    public int nbSpawnEnnemy = 1;
    public int PourcentageBot = 10;
    public int PourcentageMid = 10;
    public int PourcentageTop = 10;

    //Calcul des plateformes instanciées
    int caseTop = 0;
    int caseTopToInst;
    int caseMid = 0;
    int caseMidToInst;
    int caseBot = 0;
    int caseBotToInst;

    //Taille d'uns case
    float sizeCaseX;
    float sizeCaseZ;

    //Prefabs
    public GameObject sol;
    public GameObject wallBot;
    public GameObject wallMid;
    public GameObject wallTop;
    public GameObject cristal;
    public GameObject spawnEnnemy;
    public GameObject Player;

    //La grille de ref
    public GameObject[,] grille;

    Vector2[] posCristaux;


    // Start is called before the first frame update
    void Start()
    {
        sizeCaseX = wallBot.transform.localScale.x;
        sizeCaseZ = wallBot.transform.localScale.z;
        grille = new GameObject[sizeX, sizeZ];
        posCristaux = new Vector2[nbCristaux];
        Generate();
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    }

    void Generate()
    {
        Camera.main.transform.position = new Vector3(sizeX, 50, sizeZ);
        sol.transform.localScale = new Vector3(sizeX * sizeCaseX / 10, 1, sizeZ * sizeCaseZ / 10);
        Instantiate(sol, new Vector3(sizeX, 0, sizeZ), Quaternion.identity);

        //Placement des cristaux
        for (int i = nbCristaux; i > 0; i--)
        {
            int a = Random.Range(0, sizeX);
            int b = Random.Range(0, sizeZ);
            if (grille[a, b] == null)
            {
                InstInGrille(cristal, a, b);
                posCristaux[nbCristaux-i] = new Vector2(a, b);
                caseTop++;
            }
        }


        //Placement des premiers Tops
        foreach (GameObject crist in grille)
        {
            if (crist != null && crist.tag == "cristal")
            {
                FirstEntourage(crist, (int)(crist.transform.position.x - (sizeCaseX / 2)) / 2, (int)(crist.transform.position.z - (sizeCaseZ / 2)) / 2);
            }
        }
        //Calcul Nb de Case Top
        caseTopToInst = DistribCase(PourcentageTop, caseTop);
        //Seconds Tops
        int loop = 2;
        while (caseTopToInst > caseTop)
        {
            foreach (GameObject crist in grille)
            {
                if (crist != null && crist.tag == "cristal")
                {
                    int posX = (int)(crist.transform.position.x - (sizeCaseX / 2)) / 2;
                    int posZ = (int)(crist.transform.position.z - (sizeCaseZ / 2)) / 2;
                    AutresEntourage(crist, loop, caseTopToInst, caseTop, posX, posZ);
                }
            }
            loop++;
        }

//        Debug.Log(caseTop);
        //Placement des Mids
        foreach (GameObject TOP in grille)
        {
            if (TOP != null && TOP.tag == "top")
            {
                FirstEntourage(TOP, (int)(TOP.transform.position.x - (sizeCaseX / 2)) / 2, (int)(TOP.transform.position.z - (sizeCaseZ / 2)) / 2);
            }
        }
        //Calcule Nb case Mid
        caseMidToInst = DistribCase(PourcentageMid, caseMid);
        //Seconds Mids
        loop = 2;
        while (caseMidToInst > caseMid)
        {
            foreach (GameObject TOP in grille)
            {
                if (TOP != null && TOP.tag == "top")
                {
                    int posX = (int)(TOP.transform.position.x - (sizeCaseX / 2)) / 2;
                    int posZ = (int)(TOP.transform.position.z - (sizeCaseZ / 2)) / 2;
                    AutresEntourage(TOP, loop, caseMidToInst, caseMid, posX, posZ);
                }
            }
            loop++;
        }

//        Debug.Log(caseMid);
        //Placement des Bots
        foreach (GameObject MID in grille)
        {
            if (MID != null && MID.tag == "mid")
            {
                FirstEntourage(MID, (int)(MID.transform.position.x - (sizeCaseX / 2)) / 2, (int)(MID.transform.position.z - (sizeCaseZ / 2)) / 2);
            }
        }
        //Calcule Nb case Bot
        caseBotToInst = DistribCase(PourcentageBot, caseBot);
        //        Debug.Log(caseBotToInst);
        //Seconds Bot
        /*
            loop = 2;
            while (caseBotToInst > caseBot)
            {
                foreach (GameObject MID in grille)
                {
                    if (MID != null && MID.tag == "mid")
                    {
                        int posX = (int)(MID.transform.position.x - (sizeCaseX / 2)) / 2;
                        int posZ = (int)(MID.transform.position.z - (sizeCaseZ / 2)) / 2;
                        AutresEntourage(MID, loop, caseMidToInst, caseMid, posX, posZ);
                    }
                }
                loop++;
            }
        */
        for (int i = 0; i < nbSpawnEnnemy; i++)
        {
            int a = Random.Range(0, sizeX);
            int b = Random.Range(0, sizeZ);
            while (grille[a, b] != null)
            {
                a = Random.Range(0, sizeX);
                b = Random.Range(0, sizeZ);
            }
            InstInGrille(spawnEnnemy, a, b);
        }

        Instantiate(Player, new Vector3(sizeX, 10, sizeZ), Quaternion.identity);


    }



    void FirstEntourage(GameObject center, int x, int z)
    {
        for (int i = x - 1; i <= x +1; i++)
        {
            for (int j = z - 1; j <= z + 1; j++)
            {
                if (i >= 0 && j >= 0 && i < sizeX && j < sizeZ && grille[i, j] == null)
                {
                    if (center.tag == "cristal")
                    {
                        InstInGrille(wallTop, i, j);
                    }
                    if (center.tag == "top")
                    {
                        InstInGrille(wallMid, i, j);
                    }
                    if (center.tag == "mid")
                    {
                        InstInGrille(wallBot, i, j);
                    }
                }
            }
        }
    }

    int DistribCase(int pourcent, int casePres)
    {
        return Random.Range(casePres, (int)((sizeX * sizeZ) / 100) * pourcent);
    }

    void AutresEntourage(GameObject center, int tour, int caseToInst,int casePres, int x, int z) 
    {
        for (int i = x - tour; i <= x + tour; i++)
        {
            for (int j = z - tour; j <= z + tour; j++)
            {
                if (i >= 0 && j >= 0 && i < sizeX && j < sizeZ && caseToInst > casePres && grille[i, j] == null)
                {
                    if (Random.Range(0, 10) < 1)
                    {
                        if (center.tag == "cristal")
                        {
                            InstInGrille(wallTop, i, j);
                        }
                        if (center.tag == "top")
                        {                                   
                            InstInGrille(wallMid, i, j);
                        }
                        if (center.tag == "mid")
                        {                           
                            InstInGrille(wallBot, i, j);
                        }
                        casePres++;
                    }
                }
            }
        }
    }

    public void InstInGrille(GameObject objToInst,int x,int z)
    {
        if (objToInst.tag == "cristal")
        {
            objToInst.transform.position = new Vector3(x * sizeCaseX +sizeCaseX/2, wallTop.transform.localScale.y + objToInst.transform.localScale.y, z * sizeCaseZ + sizeCaseZ / 2);
            wallTop.transform.position = new Vector3(x * sizeCaseX + sizeCaseX / 2, wallTop.transform.localScale.y / 2, z * sizeCaseZ + sizeCaseZ / 2);
            grille[x, z] = Instantiate(objToInst, gameObject.transform);
            Instantiate(wallTop, gameObject.transform);
        }
        else
        {
            objToInst.transform.position = new Vector3(x * sizeCaseX + sizeCaseX / 2, objToInst.transform.localScale.y / 2, z * sizeCaseZ + sizeCaseZ / 2);
            grille[x, z] = Instantiate(objToInst, gameObject.transform);
            if (objToInst.tag == "top")
            {
                objToInst.GetComponent<blocksMng>().posInGrille = new Vector2(x, z);
                caseTop++;
            }
            if (objToInst.tag == "mid")
            {
                objToInst.GetComponent<blocksMng>().posInGrille = new Vector2(x, z);
                caseMid++;
            }
            if (objToInst.tag == "bot")
            {
                objToInst.GetComponent<blocksMng>().posInGrille = new Vector2(x, z);
                caseBot++;
            }
        }
    }

    // TEST POINT ELOIGNEE
/*

    int caseVide = sizeX * sizeZ - caseTop + caseMid + caseBot;
    Vector2 bestPosTemp = Vector2.zero;
    Vector2[] farPos = new Vector2[nbCristaux];
    loop = 0;
    float totX = 0;
    float totZ = 0;
    foreach(Vector2 crist in posCristaux)
    {
        float DistMax = 0;
        float indexX = 0;
        float indexZ = 0;
        foreach (GameObject caseBlanc in grille)
        {
            if (indexX > sizeX)
            {
                indexZ++;
                indexX = 0;
            }
            if(caseBlanc == null && Vector2.Distance(new Vector2(indexX, indexZ), crist)>DistMax)
            {
                DistMax = Vector2.Distance(new Vector2(indexX, indexZ), crist);
                bestPosTemp = new Vector2(indexX, indexZ);
            }
            indexX++;
        }
        farPos[loop] = bestPosTemp;
        Debug.Log(bestPosTemp);
        loop++;
        totX = bestPosTemp.x;
        totZ = bestPosTemp.y;
    }
    int m = (int)totX / loop;
    int n = (int)totZ / loop;
 //   tour = 0;
    spawnEnnemy.transform.position = new Vector3(m * 2, 0, n * 2);
    Debug.Log(m + "     " + n);
//    int spawnActive = 0;
    if (grille[m, n] == null)
    {
        Debug.Log("ok");
        grille[m, n] = Instantiate(spawnEnnemy, gameObject.transform);
    }
*/
}

