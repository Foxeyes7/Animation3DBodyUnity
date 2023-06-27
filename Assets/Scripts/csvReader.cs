using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using TMPro;
using UnityEngine.UI;

public class csvReader : MonoBehaviour
{
    private List<Vector3> pointList = new List<Vector3>();
    private List<GameObject> gameObjects = new List<GameObject>();

    private List<List<Vector3>> globalListCam1;
    private List<List<Vector3>> globalListCam2;
    private List<List<Vector3>> globalListCam3;
    private List<List<Vector3>> globalListCam4;

    private GameObject yBot;

    [SerializeField]
    private GameObject prefabCube, prefabEmpty, prefabLine;

    private GameObject lines, people;
    private bool runExercice = false;

    [SerializeField]
    private float offsetX, offsetY, offsetZ, multW, multH;

    [SerializeField]
    private int minFrameCount;

    private string[] directories;
    private string[] files;
    private string[] videos;

    [SerializeField]
    private TMP_Dropdown dropdown;
    [SerializeField]
    private TMP_Text text;
    private List<string> dropOptions = new List<string>();

    [SerializeField]
    private camManager camManager;

    [SerializeField]
    private Toggle toggle;
    private bool OneCSVmode = false;

    // Start is called before the first frame update
    void Start()
    {

        directories = Directory.GetDirectories(@"..\Mediapipe-3D-Transcription-main\Assets\CSV");

        yBot = GameObject.Find("Y_Bot");

        foreach (string dir in directories)
        {
            System.IO.FileInfo monDir = new System.IO.FileInfo(dir);
            string name = monDir.Name;
            dropOptions.Add(name);
        }


        dropdown.ClearOptions();
        dropdown.AddOptions(dropOptions);
        OnDropDownChange();
    }

    // Update is called once per frame
    void Update()
    {
        int frameCountOnVideo = camManager.GetFrameCount();
        int currentFrameOnVideo = camManager.GetFrameIndex();

        if (runExercice)
        {
            int frameDiff = frameCountOnVideo - minFrameCount;
            if(currentFrameOnVideo > frameDiff)
            {
                for (int i = 0; i < pointList.Count; i++)
                {
                    gameObjects[i].transform.position = globalListCam4[currentFrameOnVideo-frameDiff][i];

                    if (yBot == null)
                    {
                        Debug.LogError("Y_Bot not found");
                        return;
                    }

                    /*
                    if (i == 0)
                    {
                        Transform Head = yBot.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(1).GetChild(0);
                        if (Head != null)
                        {
                            Head.position = globalListCam4[currentFrameOnVideo - frameDiff][0];
                            Quaternion lookRotation = Quaternion.Euler(globalListCam4[currentFrameOnVideo - frameDiff][0]);
                            Quaternion finalRotation = lookRotation * Quaternion.Euler(-90, 180, 0);
                            Head.rotation = finalRotation;
                        }
                    }*/

                    //-------------------------------------------- LEFT ARM ----------------------------------------------//
                    if (i == 11)
                    {
                        Transform LeftForeArm = yBot.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
                        if (LeftForeArm != null)
                        {
                            LeftForeArm.position = globalListCam4[currentFrameOnVideo - frameDiff][11];
                            Vector3 jointBasePosition = globalListCam4[currentFrameOnVideo - frameDiff][11];// Position of the parent joint
                            Vector3 jointTargetPosition = globalListCam4[currentFrameOnVideo - frameDiff][13];// Position of the target joint
                            Vector3 directionToTarget = jointTargetPosition - jointBasePosition;// Compute the direction vector from base to target
                            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);// Create a rotation that points the z-axis towards the target
                            Quaternion finalRotation = lookRotation * Quaternion.Euler(90, 0, 0); // Adjust the rotation 90 degrees around the x-axis to make the y-axis point towards the target
                            LeftForeArm.rotation = finalRotation; // Apply the rotation to the joint
                        }
                    }
                    if (i == 13)
                    {
                        Transform LeftForeArm = yBot.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
                        if (LeftForeArm != null){
                            LeftForeArm.position = globalListCam4[currentFrameOnVideo - frameDiff][13];
                            Vector3 jointBasePosition = globalListCam4[currentFrameOnVideo - frameDiff][13];// Position of the parent joint
                            Vector3 jointTargetPosition = globalListCam4[currentFrameOnVideo - frameDiff][15];// Position of the target joint
                            Vector3 directionToTarget = jointTargetPosition - jointBasePosition;// Compute the direction vector from base to target
                            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);// Create a rotation that points the z-axis towards the target
                            Quaternion finalRotation = lookRotation * Quaternion.Euler(90, 0, 0); // Adjust the rotation 90 degrees around the x-axis to make the y-axis point towards the target
                            LeftForeArm.rotation = finalRotation; // Apply the rotation to the joint
                        }
                    }
                    if (i == 15)
                    {
                        Transform RightWrist = yBot.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
                        if (RightWrist != null)
                        {
                            RightWrist.position = globalListCam4[currentFrameOnVideo - frameDiff][15];
                            Vector3 jointBasePosition = globalListCam4[currentFrameOnVideo - frameDiff][15];// Position of the parent joint
                            Vector3 position1 = globalListCam4[currentFrameOnVideo - frameDiff][17]; ;  // Premier Vector3
                            Vector3 position2 = globalListCam4[currentFrameOnVideo - frameDiff][19]; ;  // Deuxième Vector3
                            Vector3 averagePosition = (position1 + position2) / 2.0f; // Mean of the hand position
                            Vector3 directionToTarget = averagePosition - jointBasePosition;// Compute the direction vector from base to target
                            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);// Create a rotation that points the z-axis towards the target
                            Quaternion finalRotation = lookRotation * Quaternion.Euler(90, 0, 0); // Adjust the rotation 90 degrees around the x-axis to make the y-axis point towards the target
                            RightWrist.rotation = finalRotation;
                        }
                    }

                    //-------------------------------------------- RIGHT ARM ----------------------------------------------//
                    if (i == 12)
                    {
                        Transform LeftForeArm = yBot.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0);
                        if (LeftForeArm != null)
                        {
                            LeftForeArm.position = globalListCam4[currentFrameOnVideo - frameDiff][12];
                            Vector3 jointBasePosition = globalListCam4[currentFrameOnVideo - frameDiff][12];// Position of the parent joint
                            Vector3 jointTargetPosition = globalListCam4[currentFrameOnVideo - frameDiff][14];// Position of the target joint
                            Vector3 directionToTarget = jointTargetPosition - jointBasePosition;// Compute the direction vector from base to target
                            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);// Create a rotation that points the z-axis towards the target
                            Quaternion finalRotation = lookRotation * Quaternion.Euler(90, 0, 0); // Adjust the rotation 90 degrees around the x-axis to make the y-axis point towards the target
                            LeftForeArm.rotation = finalRotation; // Apply the rotation to the joint
                        }
                    }
                    if (i == 14)
                    {
                        Transform RightForeArm = yBot.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0);
                        if (RightForeArm != null){
                            RightForeArm.position = globalListCam4[currentFrameOnVideo - frameDiff][14];
                            Vector3 jointBasePosition = globalListCam4[currentFrameOnVideo - frameDiff][14];// Position of the parent joint
                            Vector3 jointTargetPosition = globalListCam4[currentFrameOnVideo - frameDiff][16];// Position of the target joint
                            Vector3 directionToTarget = jointTargetPosition - jointBasePosition;// Compute the direction vector from base to target
                            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);// Create a rotation that points the z-axis towards the target
                            Quaternion finalRotation = lookRotation * Quaternion.Euler(90, 0, 0); // Adjust the rotation 90 degrees around the x-axis to make the y-axis point towards the target
                            RightForeArm.rotation = finalRotation; // Apply the rotation to the joint
                        }
                    }
                    if (i == 16)
                    {
                        Transform RightWrist = yBot.transform.GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0);
                        if (RightWrist != null){
                            RightWrist.position = globalListCam4[currentFrameOnVideo - frameDiff][16];
                            Vector3 jointBasePosition = globalListCam4[currentFrameOnVideo - frameDiff][16];// Position of the parent joint
                            Vector3 position1 = globalListCam4[currentFrameOnVideo - frameDiff][18]; ;  // Premier Vector3
                            Vector3 position2 = globalListCam4[currentFrameOnVideo - frameDiff][20]; ;  // Deuxième Vector3
                            Vector3 averagePosition = (position1 + position2) / 2.0f; // Mean of the hand position
                            Vector3 directionToTarget = averagePosition - jointBasePosition;// Compute the direction vector from base to target
                            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);// Create a rotation that points the z-axis towards the target
                            Quaternion finalRotation = lookRotation * Quaternion.Euler(90, 0, 0); // Adjust the rotation 90 degrees around the x-axis to make the y-axis point towards the target
                            RightWrist.rotation = finalRotation;
                        }
                    }

                    //-------------------------------------------- LEFT LEG ----------------------------------------------//

                    if (i == 23)
                    {
                        Transform LeftUpLeg = yBot.transform.GetChild(2).GetChild(0);
                        if (LeftUpLeg != null)
                        {
                            LeftUpLeg.position = globalListCam4[currentFrameOnVideo - frameDiff][i];
                            Vector3 jointBasePosition = globalListCam4[currentFrameOnVideo - frameDiff][i]; // Position of the parent joint
                            Vector3 jointTargetPosition = globalListCam4[currentFrameOnVideo - frameDiff][25]; // Position of the target joint
                            Vector3 directionToTarget = jointTargetPosition - jointBasePosition; // Compute the direction vector from base to target
                            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget); // Create a rotation that points the z-axis towards the target
                            Quaternion finalRotation = lookRotation * Quaternion.Euler(-90, 180, 0); // Adjust the rotation 90 degrees around the x-axis to make the y-axis point towards the target
                            LeftUpLeg.rotation = finalRotation;
                        }
                    }

                    if (i == 25)
                    {
                        Transform leftLegJoint = yBot.transform.GetChild(2).GetChild(0).GetChild(0);
                        if (leftLegJoint != null)
                        {
                            leftLegJoint.position = globalListCam4[currentFrameOnVideo - frameDiff][i];
                            Vector3 jointBasePosition = globalListCam4[currentFrameOnVideo - frameDiff][i]; // Position of the parent joint
                            Vector3 jointTargetPosition = globalListCam4[currentFrameOnVideo - frameDiff][27]; // Position of the target joint
                            Vector3 directionToTarget = jointTargetPosition - jointBasePosition; // Compute the direction vector from base to target
                            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget); // Create a rotation that points the z-axis towards the target
                            Quaternion finalRotation = lookRotation * Quaternion.Euler(-90, 180, 0); // Adjust the rotation 90 degrees around the x-axis to make the y-axis point towards the target
                            leftLegJoint.rotation = finalRotation;
                        }
                    }

                    if (i == 27)
                    {
                        Transform LeftFoot = yBot.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0);
                        if (LeftFoot != null)
                        {
                            LeftFoot.position = globalListCam4[currentFrameOnVideo - frameDiff][i];
                            Vector3 jointBasePosition = globalListCam4[currentFrameOnVideo - frameDiff][i]; // Position of the parent joint
                            Vector3 jointTargetPosition = globalListCam4[currentFrameOnVideo - frameDiff][31]; // Position of the target joint
                            Vector3 directionToTarget = jointTargetPosition - jointBasePosition; // Compute the direction vector from base to target
                            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget); // Create a rotation that points the z-axis towards the target
                            Quaternion finalRotation = lookRotation * Quaternion.Euler(-90, 180, 0); // Adjust the rotation 90 degrees around the x-axis to make the y-axis point towards the target
                            LeftFoot.rotation = finalRotation;
                        }
                    }

                    //-------------------------------------------- RIGHT LEG ---------------------------------------------//

                    if (i == 24)
                    {
                        Transform RightUpLeg = yBot.transform.GetChild(2).GetChild(1);
                        if (RightUpLeg != null)
                        {
                            RightUpLeg.position = globalListCam4[currentFrameOnVideo - frameDiff][i];
                            Vector3 jointBasePosition = globalListCam4[currentFrameOnVideo - frameDiff][i]; // Position of the parent joint
                            Vector3 jointTargetPosition = globalListCam4[currentFrameOnVideo - frameDiff][26]; // Position of the target joint
                            Vector3 directionToTarget = jointTargetPosition - jointBasePosition; // Compute the direction vector from base to target
                            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget); // Create a rotation that points the z-axis towards the target
                            Quaternion finalRotation = lookRotation * Quaternion.Euler(-90, 180, 0); // Adjust the rotation 90 degrees around the x-axis to make the y-axis point towards the target
                            RightUpLeg.rotation = finalRotation;
                        }
                    }

                    if (i == 26)
                    {
                        Transform RightLegJoint = yBot.transform.GetChild(2).GetChild(1).GetChild(0);
                        if (RightLegJoint != null)
                        {
                            RightLegJoint.position = globalListCam4[currentFrameOnVideo - frameDiff][i];
                            Vector3 jointBasePosition = globalListCam4[currentFrameOnVideo - frameDiff][i]; // Position of the parent joint
                            Vector3 jointTargetPosition = globalListCam4[currentFrameOnVideo - frameDiff][28]; // Position of the target joint
                            Vector3 directionToTarget = jointTargetPosition - jointBasePosition; // Compute the direction vector from base to target
                            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget); // Create a rotation that points the z-axis towards the target
                            Quaternion finalRotation = lookRotation * Quaternion.Euler(-90, 180, 0); // Adjust the rotation 90 degrees around the x-axis to make the y-axis point towards the target
                            RightLegJoint.rotation = finalRotation;
                        }
                    }

                    if (i == 28)
                    {
                        Transform RightFoot = yBot.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0);
                        if (RightFoot != null)
                        {
                            RightFoot.position = globalListCam4[currentFrameOnVideo - frameDiff][i];
                            Vector3 jointBasePosition = globalListCam4[currentFrameOnVideo - frameDiff][i]; // Position of the parent joint
                            Vector3 jointTargetPosition = globalListCam4[currentFrameOnVideo - frameDiff][32]; // Position of the target joint
                            Vector3 directionToTarget = jointTargetPosition - jointBasePosition; // Compute the direction vector from base to target
                            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget); // Create a rotation that points the z-axis towards the target
                            Quaternion finalRotation = lookRotation * Quaternion.Euler(-90, 180, 0); // Adjust the rotation 90 degrees around the x-axis to make the y-axis point towards the target
                            RightFoot.rotation = finalRotation;
                        }
                    }

                    //-------------------------------------------- Spine ---------------------------------------------//

                    Transform Spine1 = yBot.transform.GetChild(2).GetChild(2).GetChild(0);
                    Transform Spine2 = yBot.transform.GetChild(2);
                    if (Spine1 != null && Spine2 != null)
                    {
                        Vector3 position1 = globalListCam4[currentFrameOnVideo - frameDiff][11];
                        Vector3 position2 = globalListCam4[currentFrameOnVideo - frameDiff][12];
                        Vector3 Start = (position1 + position2) / 2.0f;

                        Vector3 position3 = globalListCam4[currentFrameOnVideo - frameDiff][23];
                        Vector3 position4 = globalListCam4[currentFrameOnVideo - frameDiff][24];
                        Vector3 End = (position3 + position4) / 2.0f;

                        Vector3 direction = End - Start;
                        float interval = 1.0f / (9 + 1);  // Pour obtenir cinq points, nous divisons par six

                        List<Vector3> points = new List<Vector3>();
                        for (int j = 1; j <= 9; j++)
                        {
                            Vector3 point = Start + direction * interval * j;
                            points.Add(point);
                        }

                        Spine1.position = points[3];
                        Vector3 directionToTarget1 = End - points[3];// Compute the direction vector from base to target
                        Quaternion rotationToTarget1 = Quaternion.FromToRotation(Spine1.up, directionToTarget1);
                        Quaternion finalRotation1 = rotationToTarget1 * Quaternion.Euler(0, 0, 0);
                        Spine1.rotation = finalRotation1;

                        Spine2.position = points[7];

                        Vector3 directionToTarget2 = End - points[7];// Compute the direction vector from base to target
                        Quaternion rotationToTarget2 = Quaternion.FromToRotation(Spine2.up, directionToTarget2);
                        Spine2.rotation = rotationToTarget2 * Spine2.rotation;

                    }
                }
            }
        }
    }

    public void OnToggleChangeCSV()
    {
        if (toggle.isOn)
        {
            OneCSVmode = true;
        }
        else
        {
            OneCSVmode = false;
        }
        OnDropDownChange();
    }

    public void OnDropDownChange()
    {
        Destroy(people);
        Destroy(lines);
        runExercice = false;

        files = Directory.GetFiles(directories[dropdown.value], "*.csv");
        
        if(files.Length == 4) 
        {
            foreach (string file in files)
            {
                System.IO.FileInfo monfile = new System.IO.FileInfo(file);
                string name = monfile.Name;

                char number = name[name.Length - 5];

                switch (number)
                {
                    case '1':
                        globalListCam1 = ReadCSVFile(monfile.FullName);
                        break;
                    case '2':
                        globalListCam2 = ReadCSVFile(monfile.FullName);
                        break;
                    case '3':
                        globalListCam3 = ReadCSVFile(monfile.FullName);
                        break;
                    case '4':
                        globalListCam4 = ReadCSVFile(monfile.FullName);
                        break;
                }
            }
            
            if (globalListCam3.Count < globalListCam4.Count)
            {
                minFrameCount = globalListCam3.Count;
            }
            else
            {
                minFrameCount = globalListCam4.Count;
            }

            for (int n = 0; n < minFrameCount; n++)
            {
                for (int i = 0; i < pointList.Count; i++)
                {
                    float newX, newY, newZ;

                    if (!OneCSVmode)
                    {
                        newX = globalListCam4[n][i].x;
                        newY = globalListCam4[n][i].y;
                        newZ = globalListCam3[n][i].x;
                    }
                    else
                    {
                        newX = globalListCam4[n][i].x;
                        newY = globalListCam4[n][i].y;
                        newZ = globalListCam4[n][i].z;
                    }

                    newX *= multW;
                    newX += offsetX;
                    newY *= multH;
                    newY += offsetY;
                    newZ *= multW;
                    newZ += offsetZ;

                    globalListCam4[n][i] = new Vector3(newX, newY, newZ);
                }
            }

            videos = Directory.GetFiles(directories[dropdown.value], "*.mp4");
            List<string> list = new List<string>();

            if (videos.Length == 4)
            {
                foreach (string video in videos)
                {
                    System.IO.FileInfo myvideo = new System.IO.FileInfo(video);

                    list.Add(myvideo.FullName);
                }
                runExercice = true;
            }
            else { runExercice = false; }

            camManager.SetClipsList(list);
        }
        else { runExercice = false; }

        if (runExercice)
        {
            InstanciateCube();
            //InstanciateLines();
            text.text = dropdown.options[dropdown.value].text + " is running ...";
        }
        else
        {
            text.text = "Files missing for : " + dropdown.options[dropdown.value].text;
            camManager.SetClipsList(new List<string>());
        }
    }


    public List<List<Vector3>> ReadCSVFile(string filename)
    {
        StreamReader strReader = new StreamReader(filename);
        List<List<Vector3>> globalList = new List<List<Vector3>>();
        
        bool endOfFile = false;

        while (!endOfFile)
        {
            //endOfFile = true;
            string data_string = strReader.ReadLine();
            
            if(data_string == null)
            {
                endOfFile = true;
                break;
            }

            string[] data_values = data_string.Split(',');
            float[] data_float = new float[data_values.Length];

            for(int i = 0; i < data_values.Length; i++)
            {
                data_float[i] = float.Parse(data_values[i], CultureInfo.InvariantCulture.NumberFormat);
                //data_float[i] *= multiplicator;
            }
            
            pointList = new List<Vector3>();

            for (int i = 0; i < data_values.Length; i += 3)
            {
                pointList.Add(new Vector3(data_float[i], data_float[i + 1], data_float[i + 2]));
            }
            globalList.Add(pointList);
        }
        return globalList;
    }

    void InstanciateCube()
    {
        // Instanciate all the cubes
        people = Instantiate(prefabEmpty, new Vector3(0, 0, 0), Quaternion.identity);
        people.name = "People";

        gameObjects.Clear();
        for (int i = 0; i < pointList.Count; i++)
        {
            GameObject cube = Instantiate(prefabCube, globalListCam4[0][i], Quaternion.identity);
            cube.transform.SetParent(people.transform);
            cube.name = i.ToString();
            cube.GetComponent<Renderer>().enabled = false;
            gameObjects.Add(cube);

        }
    }

    void InstanciateLines()
    {
        // Instanciate the lines
        lines = Instantiate(prefabEmpty, new Vector3(0, 0, 0), Quaternion.identity);
        lines.name = "Lines";

        for (int i = 0; i < 9; i++)
        {
            GameObject line = Instantiate(prefabLine);
            line.GetComponent<lineTracer>().setGameObjects(gameObjects);
            line.transform.SetParent(lines.transform);
            line.name = "line "+ i.ToString();
            if(i == 8) 
            { 
                line.GetComponent<lineTracer>().setCircle(true);
            }
            else
            {
                line.GetComponent<lineTracer>().setIndexLine(i);
            }
        }
    }
}
