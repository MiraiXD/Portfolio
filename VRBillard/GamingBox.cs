// This script creates the world the game takes place in: Cubic box, balls, cuestick, pots, bottom container. Applies textures and colors

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamingBox : MonoBehaviour
{
    // Table containing all the balls
    private GameObject[] billiardBalls;
    // Hierarchy object, parent for the balls
    private GameObject billiardBallsContainer;
    // Table containing all the walls of the cubic box
    private GameObject[] boxWalls;
    // Hierarchy object, parent for the walls
    private GameObject boxWallsContainer;
    // Table containing all the pots
    private GameObject[] potSpheres;
    // Hierarchy object, parent for the pots
    private GameObject potSpheresContainer;
    // Table containing all the planes holding balls when potted
    private GameObject[] planes;
    // Hierarchy object, parent for the planes holding balls when potted
    private GameObject planesContainer;
    // Naming of the walls
    private string[] boxWallsNames = { "frontWall", "rearWall", "rightWall", "leftWall", "topWall", "bottomWall" };
    // Starting positions of all the balls, set in a pyramid
    private Vector3[] ballsStartingPos;
    // Table containing textures to be applied to the balls
    private Texture[] ballsTextures;

    // Single ball's radius
    private float r; 
    // Size of the cube
    private float gamingBoxSize = 1;
    // How big are the balls compared to the cubic box?
    private float ballToBoxRatio = 0.1f;
    // How thick are the walls?
    private float boxWallThickness = 0.01f;
    // The color of all the walls
    [SerializeField]
    private Color boxWallsColor = new Color(0.4f, 0.4f, 1.0f, 0.2f);
    // The color of all the pots
    [SerializeField]
    private Color potSpheresColor = new Color(0.1f, 0.9f, 0.2f, 0.5f);
    // Cuestick prefab to be instantiated at run time
    [SerializeField]
    private GameObject cueStickPrefab;
    // Use this for initialization
    void Start()
    {
        initiateCueStick();
        initiateBoxWalls();
        initiatePotSpheres();
        positionPotSpheres();
        initiateBilliardBalls();
        positionBilliardBalls();
        createPlanes();
    }

    void initiateCueStick()
    {
        cueStickPrefab = (GameObject)Instantiate(cueStickPrefab, new Vector3(gamingBoxSize / 2, 0, 0), Quaternion.identity, this.transform);
        cueStickPrefab.name = "CueStickPrefab";
        cueStickPrefab.transform.localScale *= 0.7f;
    }

    void initiatePotSpheres()
    {
        potSpheres = new GameObject[8];
        potSpheresContainer = new GameObject();
        potSpheresContainer.name = "potSpheresContainer";
        potSpheresContainer.transform.parent = this.transform;
        for (int i = 0; i < potSpheres.Length; i++)
        {
            potSpheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            potSpheres[i].name = "Pot_" + (i + 1);
            potSpheres[i].transform.parent = potSpheresContainer.transform;
            potSpheres[i].transform.localScale *= gamingBoxSize * ballToBoxRatio * 2;
            potSpheres[i].GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
            potSpheres[i].GetComponent<Renderer>().material.color = potSpheresColor;
            potSpheres[i].GetComponent<SphereCollider>().radius *= 0.8f;
            potSpheres[i].GetComponent<SphereCollider>().isTrigger = true;
            potSpheres[i].AddComponent<PotScript>().boxWalls = boxWalls;
        }

    }

    void positionPotSpheres()
    {
        potSpheres[0].transform.position = new Vector3(gamingBoxSize / 2, gamingBoxSize / 2, gamingBoxSize / 2);
        potSpheres[1].transform.position = new Vector3(-gamingBoxSize / 2, gamingBoxSize / 2, gamingBoxSize / 2);
        potSpheres[2].transform.position = new Vector3(gamingBoxSize / 2, -gamingBoxSize / 2, gamingBoxSize / 2);
        potSpheres[3].transform.position = new Vector3(gamingBoxSize / 2, gamingBoxSize / 2, -gamingBoxSize / 2);
        potSpheres[4].transform.position = new Vector3(-gamingBoxSize / 2, -gamingBoxSize / 2, gamingBoxSize / 2);
        potSpheres[5].transform.position = new Vector3(gamingBoxSize / 2, -gamingBoxSize / 2, -gamingBoxSize / 2);
        potSpheres[6].transform.position = new Vector3(-gamingBoxSize / 2, gamingBoxSize / 2, -gamingBoxSize / 2);
        potSpheres[7].transform.position = new Vector3(-gamingBoxSize / 2, -gamingBoxSize / 2, -gamingBoxSize / 2);
    }

    float getRandomPos()
    {
        float num;
        num = Random.Range(3 * r, gamingBoxSize - 3 * r - 2 * boxWallThickness - 2 * r);
        if (num > gamingBoxSize / 2 - boxWallThickness - r)
            num = gamingBoxSize / 2 - 3 * r - boxWallThickness - r - num;
        return num;
    }

    void initiateBilliardBalls()
    {
        billiardBalls = new GameObject[15];
        ballsTextures = new Texture[15];

        billiardBallsContainer = new GameObject();
        billiardBallsContainer.name = "billiardBallsContainer";
        billiardBallsContainer.transform.parent = this.transform;
        for (int i = 0; i < billiardBalls.Length; i++)
        {
            billiardBalls[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            billiardBalls[i].transform.parent = billiardBallsContainer.transform;
            if (i == 0)
                billiardBalls[i].name = "CueBall";
            else
                billiardBalls[i].name = "Ball" + i;

            ballsTextures[i] = (Texture)Resources.Load("Textures/Ball" + i);
            billiardBalls[i].transform.localScale = new Vector3(gamingBoxSize * ballToBoxRatio, gamingBoxSize * ballToBoxRatio, gamingBoxSize * ballToBoxRatio);
            billiardBalls[i].GetComponent<Renderer>().material.shader = Shader.Find("VertexLit");
            billiardBalls[i].GetComponent<Renderer>().material.SetTexture("_MainTex", ballsTextures[i]);
            billiardBalls[i].GetComponent<SphereCollider>().material = (PhysicMaterial)Resources.Load("Materials/BouncyMaterial");
            billiardBalls[i].AddComponent<Rigidbody>().useGravity = false;
            billiardBalls[i].GetComponent<Rigidbody>().drag = 0.1f;
            billiardBalls[i].GetComponent<Rigidbody>().angularDrag = 0.1f;
            billiardBalls[i].tag = "Ball";
        }


    }

    void positionBilliardBalls()
    {
        ballsStartingPos = new Vector3[15];
        r = billiardBalls[0].GetComponent<SphereCollider>().radius * ballToBoxRatio * gamingBoxSize;

        int i = 0;
        //Position the CueBall
        ballsStartingPos[i].x = getRandomPos();
        ballsStartingPos[i].y = getRandomPos();
        ballsStartingPos[i].z = getRandomPos();

        i++;

        ballsStartingPos[i].Set(0, 2 * r, 0);
        i++;

        for (int z = -1; z <= 1; z += 2)
            for (int x = 1; x >= -1; x -= 2)
            {
                ballsStartingPos[i].Set(x * r, 0, z * r);
                i++;
            }

        for (int z = -2; z <= 2; z += 2)
            for (int x = 2; x >= -2; x -= 2)
            {
                ballsStartingPos[i].Set(x * r, -2 * r, z * r);
                i++;
            }

        for (i = 0; i <= billiardBalls.Length - 1; i++)
            billiardBalls[i].transform.position = ballsStartingPos[i];
    }

    void initiateBoxWalls()
    {
        boxWalls = new GameObject[6];
        boxWallsContainer = new GameObject();
        boxWallsContainer.name = "boxWallsContainer";
        boxWallsContainer.transform.parent = this.transform;

        for (int i = 0; i < boxWalls.Length; i++)
        {
            boxWalls[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            boxWalls[i].transform.parent = boxWallsContainer.transform;
            boxWalls[i].transform.localScale = new Vector3(boxWalls[i].transform.localScale.x * gamingBoxSize, boxWalls[i].transform.localScale.y * gamingBoxSize, boxWallThickness);
            boxWalls[i].GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
            boxWalls[i].GetComponent<Renderer>().material.color = boxWallsColor;
            boxWalls[i].GetComponent<BoxCollider>().material = (PhysicMaterial)Resources.Load("Materials/BouncyMaterial");
            boxWalls[i].name = boxWallsNames[i];
            boxWalls[i].tag = "Box";
            switch (boxWalls[i].name)
            {
                case "frontWall":
                    boxWalls[i].transform.Translate(gamingBoxSize / 2 * Vector3.forward);
                    boxWalls[i].transform.LookAt(Vector3.forward);
                    break;
                case "rearWall":
                    boxWalls[i].transform.Translate(gamingBoxSize / 2 * Vector3.back);
                    boxWalls[i].transform.LookAt(Vector3.back);
                    break;
                case "rightWall":
                    boxWalls[i].transform.Translate(gamingBoxSize / 2 * Vector3.right);
                    boxWalls[i].transform.LookAt(Vector3.right);
                    break;
                case "leftWall":
                    boxWalls[i].transform.Translate(gamingBoxSize / 2 * Vector3.left);
                    boxWalls[i].transform.LookAt(Vector3.left);
                    break;
                case "topWall":
                    boxWalls[i].transform.Translate(gamingBoxSize / 2 * Vector3.up);
                    boxWalls[i].transform.LookAt(Vector3.up);
                    break;
                case "bottomWall":
                    boxWalls[i].transform.Translate(gamingBoxSize / 2 * Vector3.down);
                    boxWalls[i].transform.LookAt(Vector3.down);
                    break;
            }
        }
    }

    void createPlanes()
    {
        planes = new GameObject[4];
        planesContainer = new GameObject();
        planesContainer.name = "planesContainer";

        for (int i = 0; i < planes.Length; i++)
        {
            planes[i] = GameObject.CreatePrimitive(PrimitiveType.Plane);
            planes[i].transform.parent = planesContainer.transform;
            planes[i].transform.position = new Vector3(0, -gamingBoxSize, 0);
            planes[i].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            planes[i].GetComponent<Renderer>().material.shader = Shader.Find("VertexLit");
            planes[i].GetComponent<Renderer>().material.color = Color.green;
        }
        planes[0].transform.rotation = Quaternion.Euler(10, 0, 0);
        planes[1].transform.rotation = Quaternion.Euler(-10, 0, 0);
        planes[2].transform.rotation = Quaternion.Euler(0, 0, 10);
        planes[3].transform.rotation = Quaternion.Euler(0, 0, -10);
    }

}
