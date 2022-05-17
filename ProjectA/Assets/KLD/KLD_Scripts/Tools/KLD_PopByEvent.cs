using UnityEngine;

public class KLD_PopByEvent : MonoBehaviour
{
    public void PopPooler(string key)
    {
        XL_Pooler.instance.PopPosition(key, transform.position);
    }
}