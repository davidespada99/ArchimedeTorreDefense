using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapperUI : MonoBehaviour
{

    public RectTransform mapTransform;
    public RectTransform shopTransform;
    // Start is called before the first frame update
    void Start()
    {
        //Swapper();
        

    }

    private void Swapper()
    {
        Vector2 mapAnchors = mapTransform.anchoredPosition;
        Vector2 mapPivot = mapTransform.pivot;
        Vector2 mapPosition = mapTransform.position;

        //mapTransform.anchoredPosition = shopTransform.anchoredPosition;
        mapTransform.pivot = shopTransform.pivot;
        mapTransform.position = shopTransform.position;

        //shopTransform.anchoredPosition = mapAnchors;
        shopTransform.pivot = mapPivot;
        shopTransform.position = mapPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
