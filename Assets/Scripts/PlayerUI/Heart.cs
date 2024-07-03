using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] Transform fullHeart;

    public void MakeHeartHalf()
    {
        fullHeart.gameObject.SetActive(false);
    }
    public void MakeHeartFull()
    {
        fullHeart.gameObject.SetActive(true);
    }
}
