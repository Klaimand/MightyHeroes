using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RSL_Aim : MonoBehaviour
{

    [SerializeField] KLD_ZombieList zombieList;
    [SerializeField] Transform defaultTarget = null;

    [SerializeField, ReadOnly] KLD_ZombieAttributes selectedZombie = null;

    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [SerializeField] KLD_AimBehavior aimBehavior;


    Vector3 targetPos = Vector3.zero;

    Vector3 debugPosa = Vector3.zero;
    Vector3 debugPosb = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        selectedZombie = aimBehavior.GetZombieToTarget(zombieList.GetZombies(), transform);

        targetPos = selectedZombie != null ? selectedZombie.transform.position : defaultTarget.position;


        targetPos.y = transform.position.y;
        transform.LookAt(targetPos, Vector3.up);

        DrawSelectedLine();
        */
    }

    void DrawSelectedLine()
    {
        debugPosa = transform.position;
        debugPosb = targetPos;
        debugPosa.y = 1f;
        debugPosb.y = 1f;
        Debug.DrawLine(debugPosa, debugPosb, Color.red);
    }
}
