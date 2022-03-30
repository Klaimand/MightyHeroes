using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class KLD_PlayerAim : MonoBehaviour
{
    [SerializeField] KLD_TouchInputs inputs;

    Rigidbody rb;

    [SerializeField] KLD_ZombieList zombieList;
    [SerializeField] Transform defaultTarget = null;


    [SerializeField] bool isAiming = false;
    [SerializeField, ReadOnly] Vector2 inputAimVector = Vector2.zero;
    Vector3 inputAimVector3 = Vector3.zero;
    Vector3 worldAimVector3 = Vector3.zero;



    [SerializeField, ReadOnly] KLD_ZombieAttributes selectedZombie = null;

    [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
    [SerializeField] KLD_AimBehavior aimBehavior;

    Vector3 targetPos = Vector3.zero;

    Vector3 debugPosa = Vector3.zero;
    Vector3 debugPosb = Vector3.zero;

    [SerializeField, ReadOnly] KLD_PlayerAttributes playerAttributes;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitPlayerAttributes();
    }

    // Update is called once per frame
    void Update()
    {
        //Aim();

        selectedZombie = aimBehavior.GetZombieToTarget(zombieList.GetZombies(), playerAttributes);



        targetPos = selectedZombie != null ?
        selectedZombie.transform.position :
        transform.position + rb.velocity;

        targetPos.y = transform.position.y;
        transform.LookAt(targetPos, Vector3.up);

        DrawSelectedLine();
    }

    void Aim()
    {
        if (isAiming && inputAimVector.sqrMagnitude > 0.05f)
        {
            inputAimVector3.x = inputAimVector.x;
            inputAimVector3.y = 0f;
            inputAimVector3.z = inputAimVector.y;

            inputAimVector3 = playerAttributes.transform.rotation * inputAimVector3;
        }
        else
        {
            inputAimVector3 = playerAttributes.transform.forward;
        }

        playerAttributes.worldAimDirection.x = inputAimVector3.x;
        playerAttributes.worldAimDirection.y = inputAimVector3.z;
    }

    void DrawSelectedLine()
    {
        debugPosa = transform.position;
        debugPosb = targetPos;
        debugPosa.y = 1f;
        debugPosb.y = 1f;
        Debug.DrawLine(debugPosa, debugPosb, Color.red);
    }

    void InitPlayerAttributes()
    {
        if (playerAttributes == null)
        {
            playerAttributes = new KLD_PlayerAttributes { transform = this.transform };
        }
        else if (playerAttributes.transform == null)
        {
            playerAttributes.transform = transform;
        }
    }
}

[System.Serializable]
public class KLD_PlayerAttributes
{
    public Transform transform = null;
    public Vector2 worldAimDirection = Vector2.zero;
}