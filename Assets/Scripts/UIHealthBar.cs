using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Image mask;
    float originalsize;

    public static UIHealthBar instance { get; private set; }
    public bool hasTask;
    //public bool ifCompleteTask;
    public int fixedNum;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        originalsize = mask.rectTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //设置当前UI血条显示
    public void SetValue(float fillPercent)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalsize * fillPercent);
    }
}
