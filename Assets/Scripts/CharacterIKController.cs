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
        Debug.Log("OnAnimatorIK called");
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

            Transform spine = animator.GetBoneTransform(HumanBodyBones.Spine);

            Vector3 position1 = position[11];
            Vector3 position2 = position[12];
            Vector3 Start = (position1 + position2) / 2.0f; // top of the spine

            Vector3 position3 = position[23];
            Vector3 position4 = position[24];
            Vector3 End = (position3 + position4) / 2.0f; // bottom of the spine

            Vector3 direction = End - Start;
            float interval = 1.0f / (10 + 1);  // Definition of  how much points we want for the spine

            List<Vector3> points = new List<Vector3>();
            for (int j = 1; j <= 10; j++)
            {
                Vector3 point = Start + direction * interval * j; // here we create the points between the start and the end
                points.Add(point);
            }

            spine.position = points[5];

            // Set the body position and rotation based on a calculated value
            /*Vector3 calculatedBodyPosition = // Calculated based on your needs
            Quaternion calculatedBodyRotation = // Calculated based on your needs
            animator.bodyPosition = calculatedBodyPosition;
            animator.bodyRotation = calculatedBodyRotation;*/
        }
    }
}
