using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIKController : MonoBehaviour
{
    private Animator animator;
    private Vector3[] position = new Vector3[33];
    public csvReader csvreader;
    public GameObject csvReaderGameObject;

    void Start()
    {
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
        //Debug.Log("OnAnimatorIK called");
        if (csvreader != null)
        {
            position = csvreader.GetPositions();

            // Set the left hand IK
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, position[15]);

            // Set the right hand IK
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, position[16]);

            // Set the left foot IK
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1.0f);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, position[27]);

            // Set the right foot IK
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1.0f);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, position[28]);

            // Set the head IK
            animator.SetLookAtWeight(1.0f);
            animator.SetLookAtPosition(position[0]);

            //......................................//


            //calcul des vecteurs du plan 
            Vector3 side1 = position[11] - position[12];
            Vector3 side2 = position[23] - position[12];

            Vector3 upDirection = Vector3.Cross(side1, side2).normalized;

            Vector3 bustPosition = Vector3.Lerp((position[23] + position[24]) / 2.0f, (position[11] + position[12]) / 2.0f, 0.5f);

            Vector3 upFromCenterPoint = bustPosition + upDirection;

            Vector3 upDirectionFromCenter = (upFromCenterPoint - bustPosition).normalized;

            Quaternion desiredRotation = Quaternion.LookRotation(upDirectionFromCenter, animator.transform.up);

            Vector3 spineOffset = animator.GetBoneTransform(HumanBodyBones.Spine).position - animator.transform.position;

            animator.transform.position = bustPosition - spineOffset;
            animator.transform.rotation = desiredRotation;

        }
    }
}
 