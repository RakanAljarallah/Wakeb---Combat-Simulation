using UnityEngine;

public class WeapenManager : MonoBehaviour
{

    public Weapen currentWeapen;
    
    
    public void changeFireType(WeapenFireType fireType)
    {
        currentWeapen.fireType = fireType;
    }
}
