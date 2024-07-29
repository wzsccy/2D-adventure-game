using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    public Image heathImage;
    public Image heathDelayImage;
    public Image powerImage;
    private void Update()
    {
        if (heathDelayImage.fillAmount > heathImage.fillAmount)
        {
            heathDelayImage.fillAmount-=Time.deltaTime;
        }
    }

    public void OnHealthChange(float persentage)
    {
        heathImage.fillAmount = persentage;
    }
}
