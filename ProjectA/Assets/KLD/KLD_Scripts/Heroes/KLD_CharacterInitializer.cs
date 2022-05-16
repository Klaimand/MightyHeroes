using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class KLD_CharacterInitializer : MonoBehaviour
{
    [Header("Get References")]
    public Animator animator;
    public Transform scaler;
    public Transform weaponHolderParent;
    public RigBuilder rigBuilder;
    public TwoBoneIKConstraint rightHandIK;
    public TwoBoneIKConstraint leftHandIK;

    [Header("Set References")]
    [SerializeField] KLD_PlayerAnimDoSpell playerAnimDoSpell;
    [SerializeField] MultiAimConstraint multiAimConstraint;

    WeightedTransformArray newData;

    public void Init(XL_Characters _character, Transform _targetPosSmooth)
    {
        playerAnimDoSpell.character = _character;
        //multiAimConstraint.data.sourceObjects.SetTransform(0, _targetPosSmooth);

        newData = multiAimConstraint.data.sourceObjects;
        newData.Clear();
        newData.Add(new WeightedTransform(_targetPosSmooth, 1f));
        multiAimConstraint.data.sourceObjects = newData;
        rigBuilder.Build();

        //rigBuilder.Build();
    }
}
