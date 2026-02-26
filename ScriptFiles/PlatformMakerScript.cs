using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class PlatformMaker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Platform LastPlatform;
    Platform HeadPlatform;
    public Platform newPlatformMake;

    public string platformSetNotes;


    int untilNextPlatform = 0;
    public float fallSpeed = 0.015f;
    public int spawnFrequency = 500;

    void Start()
    {
        makePlatform();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (untilNextPlatform == spawnFrequency)
        {
            makePlatform();
            untilNextPlatform = 0;
        }
        untilNextPlatform++;


    }

    public void makePlatform()
    {

        if (!string.IsNullOrEmpty(platformSetNotes))
        {
            Platform newPlatform = Platform.Instantiate(newPlatformMake);
            if (HeadPlatform == null)
            {
                HeadPlatform = newPlatform;
            }
            newPlatform.setHead(HeadPlatform);

            char noteChar = platformSetNotes[0];
            newPlatform.setNote(noteChar.ToString());

            int index = 1;
            while(index < platformSetNotes.Length && platformSetNotes[index] == ' ') { index++; }

            string number = "";
            while(index < platformSetNotes.Length && char.IsDigit(platformSetNotes[index]))
            {
                number += platformSetNotes[index];
                index++;
            }

            if (number.Length > 0)
            {
                spawnFrequency = int.Parse(number);
            }

            platformSetNotes = platformSetNotes.Substring(index).TrimStart();

            print(platformSetNotes);

            newPlatform.setFallSpeed(fallSpeed);

            if (LastPlatform != null)
            {
                LastPlatform.setNext(newPlatform);
            }
            LastPlatform = newPlatform;
        }

    }

    public Platform getHeadPlatform()
    {
        return HeadPlatform;
    }

    public void removeHeadPlat()
    {
        Platform newHead = HeadPlatform.getNext();
        HeadPlatform = newHead;
        newHead.replaceHead(newHead);
    }

}