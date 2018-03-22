using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShowAllMap : MonoBehaviour {

    private UnityEngine.UI.RawImage image;

    private void Awake()
    {
        image = GetComponent<UnityEngine.UI.RawImage>();
    }
    void OnMouseDown() // or however you catch the click on the object
    {
        if (image)
            image.enabled = !image.enabled;
    }
    public void HideImage()
    {
        image.enabled = false;

        // ===== OR =====

        gameObject.SetActive(false);
    }

    public void ShowImage()
    {
        image.enabled = true;

        // ===== OR =====

        gameObject.SetActive(true);

    }
}
