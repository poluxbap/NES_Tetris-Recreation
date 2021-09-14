using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumbersManager : MonoBehaviour
{
    public SpriteRenderer[] sprites;
    public Sprite[] numbers;

    private void Awake()
    {
        sprites = new SpriteRenderer[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            sprites[i] = transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
        }
    }

    public void ChangeNumber(int number)
    {
        int digits = sprites.Length;

        for(int i = 0; i < digits; i++)
        {
            int currentDigit = GetDigit(number, i + 1);

            sprites[i].sprite = numbers[currentDigit];
        }
    }

    private int GetDigit(int val, int digit)
    {
        float v = val / Mathf.Pow(10, digit - 1);
        int i = (int)v;
        return i % 10;
    }
}
