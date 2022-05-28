using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KLD_TutoKeyPref : MonoBehaviour
{
    public void ValidateTutoKey()
    {
        PlayerPrefs.SetInt("HasDoneTutorial", 1);
    }
}
