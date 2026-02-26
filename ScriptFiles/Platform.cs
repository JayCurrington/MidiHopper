using UnityEngine;

public class Platform : MonoBehaviour
{
    public Platform nextPlatform;
    public Platform headPlatform;

    public float fallSpeed;
    public string note;

    //setters
    public void setHead(Platform newHead)
    {
        headPlatform = newHead;
    }

    public void setNext(Platform newNext)
    {
        nextPlatform = newNext;
    }
    public void setFallSpeed (float newFS)
    {
        fallSpeed = newFS;
    }

    public void setNote(string newNote)
    {
        note = newNote;
        print(headPlatform + "   " + note);
        if (note == "A")
        {
            //set location
            transform.position = new Vector3(-7f, 6f, 0f);
        }
        else if (note == "B")
        {
            //set location
            transform.position = new Vector3(-3.5f, 6f, 0f);
        }
        else if (note == "C")
        {
            //set location
            transform.position = new Vector3(0f, 6f, 0f);
        }
        else if (note == "D")
        {
            //set location
            transform.position = new Vector3(3.5f, 6f, 0f);
        }
        else if (note == "E")
        {
            //set location
            transform.position = new Vector3(7f, 6f, 0f);
        }
        else if (note == "X")
        {
            //set location
            transform.position = new Vector3(-10f, 6f, 0f);
        }
        else
        {
            print("Game Done");
        }

    }

    //getters
    public float getX()
    {
        return transform.position.x;
    }
    public float getY()
    {
        return transform.position.y;
    }
    public Platform getNext()
    {
        return nextPlatform;
    }

    public string getNote()
    {
        return note;
    }

    public void replaceHead(Platform newHead)
    {
        headPlatform = newHead;
        if (nextPlatform != null)
        {
            nextPlatform.replaceHead(headPlatform);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - fallSpeed, transform.position.z);

    }
}