using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIKController : MonoBehaviour
{
    private Animator animator;
    private Vector3[] position = new Vector3[33];
    public csvReader csvreader;
    public GameObject csvReaderGameObject;
    public GameObject refLine1;
    public GameObject refLine2;
    public GameObject refCube1;
    public GameObject refCube2;
    [SerializeField]
    private GameObject prefabCube;
    //public GameObject refCube3;

    void Start()
    {
        //refCube1 = Instantiate(prefabCube, new Vector3(0,0,0), Quaternion.identity);
        //refCube2 = Instantiate(prefabCube, new Vector3(0, 0, 0), Quaternion.identity);
        Debug.Log("Start called");
        animator = GetComponent<Animator>();
        csvreader = csvReaderGameObject.GetComponent<csvReader>();// Supposant que le script csvReader est attaché au même GameObject
        // Commence une Coroutine pour vérifier si gameObjects est prêt à être utilisé
        StartCoroutine(CheckForGameObjects());
    }

    // Cette Coroutine est lancée après un délai défini et vérifie si gameObjects est prêt à être utilisé
    IEnumerator CheckForGameObjects()
    {
        // Attends 2 secondes pour donner à csvreader le temps d'instancier et de remplir gameObjects
        yield return new WaitForSeconds(2f);

        // Après 2 secondes, vérifie si gameObjects est prêt à être utilisé
        if (csvreader.gameObjects != null && csvreader.gameObjects.Count > 0)
        {
            // Accédez au premier GameObject créé par le csvReader
            GameObject firstGameObject = csvreader.gameObjects[0];

            // Utilisez le GameObject comme vous le souhaitez
            // par exemple, affichez sa position
            Debug.Log(firstGameObject.transform.position);
        }
    }

    void OnAnimatorIK()
    {
        if (csvreader != null)
        {
            position = csvreader.GetPositions();
            Vector3 side1;
            Vector3 side2;

            // Set the left hand IK
            Vector3 pointLH = ((position[19] + position[17]) / 2.0f);
            Vector3 fdDirectionLH = (pointLH - position[15]).normalized;
            side1 = position[19] - position[15];
            side2 = position[17] - position[15];
            Vector3 upDirectionLH = Vector3.Cross(side2, side1).normalized;

            Quaternion rotationLH = Quaternion.LookRotation(fdDirectionLH, upDirectionLH); 

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, position[15]);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, rotationLH);


            // Set the right hand IK
            Vector3 pointRH = ((position[20] + position[18]) / 2.0f);
            Vector3 fdDirectionRH = (pointRH - position[16]).normalized;
            side1 = position[20] - position[16];
            side2 = position[18] - position[16];
            Vector3 upDirectionRH = Vector3.Cross(side1, side2).normalized;

            Quaternion rotationRH = Quaternion.LookRotation(fdDirectionRH, upDirectionRH);

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, position[16]);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rotationRH);

            // Set the left foot IK
            Vector3 fdDirectionLF = (position[31] - position[29]).normalized;
            Vector3 upDirectionLF = (position[25] - position[27]).normalized;
            Quaternion rotationLF = Quaternion.LookRotation(fdDirectionLF, upDirectionLF);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1.0f);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, position[27]);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, rotationLF);

            // Set the right foot IK
            Vector3 fdDirectionRF = (position[32] - position[30]).normalized;
            Vector3 upDirectionRF = (position[26] - position[28]).normalized;
            Quaternion rotationRF = Quaternion.LookRotation(fdDirectionRF, upDirectionRF);

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1.0f);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, position[28]);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, rotationRF);

            // Set the head IK
            animator.SetLookAtWeight(1.0f);
            animator.SetLookAtPosition(position[0]);

            // Set the body  
            side1 = position[11] - position[12];
            side2 = position[23] - position[12];

            Vector3 upDirection = Vector3.Cross(side1, side2).normalized;
            Vector3 bustPosition = Vector3.Lerp((position[23] + position[24]) / 2.0f, (position[11] + position[12]) / 2.0f, 0.5f);
            Vector3 upFromCenterPoint = bustPosition + upDirection;
            Vector3 upDirectionFromCenter = (upFromCenterPoint - bustPosition).normalized;
            Vector3 CenterToHead = (((position[11] + position[12]) / 2.0f) - bustPosition).normalized;

            Quaternion desiredRotation = Quaternion.LookRotation(upDirectionFromCenter, CenterToHead);
            Vector3 spineOffset = animator.GetBoneTransform(HumanBodyBones.Spine).position - animator.transform.position;

            animator.transform.position = bustPosition - spineOffset;
            animator.transform.rotation = desiredRotation;
        }
    }
}