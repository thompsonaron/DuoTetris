using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class IconToggle : MonoBehaviour
{
    public Sprite iconTrue;
    public Sprite iconFalse;

    public bool defaultIconState = true;

    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = (defaultIconState) ? iconTrue : iconFalse;
    }

    public void ToggleIcon(bool state)
    {
        if (!image || !iconTrue|| !iconFalse)
        {
            Debug.LogWarning("WARNING! ICONTOGGLE missing iconTrue or iconFalse");
        }
        image.sprite = (state) ? iconTrue : iconFalse;
    }
}
