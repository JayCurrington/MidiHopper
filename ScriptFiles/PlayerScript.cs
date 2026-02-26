using UnityEngine;

using UnityEngine.InputSystem;

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


public class PlayerScript : MonoBehaviour
{
    public Vector3 Velocity = new Vector3(0f, 0f, 0f);

    public float gravity = 0.01f;

    public bool staticCollision = false;

    public float jumpVelocity = 0.5f;

    [SerializeField] private PlatformMaker platMaker;

    public Platform headPlatform;

    public int jumping = 0;

    void Start()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
        ConnectAllDevices();
    }
    void RemoveAtwake()
    {
        platMaker = FindFirstObjectByType<PlatformMaker>();
    }

    public void setHeadPlatform(Platform headPlatform)
    {
        headPlatform = headPlatform;
    }

    // Update is called once per frame
    void FixedUpdate() {

        if(transform.position.y < -6)
        {
            
            SceneManager.LoadScene("GameOver");
        }
        if (headPlatform == null)
        {
            headPlatform = platMaker.getHeadPlatform(); //coment
        }
        if (staticCollision == false)
        {
            Velocity.y -= gravity;
            transform.position += Velocity;
        }
        else
        {
            if (Velocity.x > 0)
                Velocity.x -= 0.01f;
            else if (Velocity.x < 0)
                Velocity.x += 0.01f;

            transform.position += Velocity;
            
            
            if (jumping > 1)
            {
                jumping --;
            }
            else if (jumping == 1) {
                jumping --;
                platMaker.removeHeadPlat();
                headPlatform = platMaker.getHeadPlatform();
                Velocity.x =0;

            }
            print("Next Note: "+headPlatform.getNote());
            print(jumping);
            
        }

        string requiredNote = headPlatform.note;
        bool result = checkNote(requiredNote);
        if (result && jumping > 0)
        {
            Velocity = CalculateJumpVelocity(10);

            transform.position += Velocity;
        }
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Velocity.y = 0;
        staticCollision = true;
    }

    public bool checkNote(string note)
    {
        if(platMaker.newPlatformMake.y > -3)
        {
            
        }

        float platformAttitude = headPlatform.getY();
        if (platformAttitude < 1)
        {
            for (int i = 0; i < currentNotes.Count(); i++)
            {
                float platformAltitude = headPlatform.getY();
                if (Equals(currentNotes[i], note))
                {
                    jumping = 10;
                    return true;
                }
            }
        }
        return false;
    }
    private Vector3 CalculateJumpVelocity(float flightTime)
    {
        float targetX = headPlatform.getX(); // platform does not move horizontally
        float targetY = headPlatform.getY() + 1.7f; // current Y

        float dx = targetX - transform.position.x;
        float dy = targetY - transform.position.y;

        // Horizontal velocity (constant)
        float vx = (dx / flightTime);

        // Vertical velocity (accounting for gravity + platform fall)
        float vy = (dy - headPlatform.fallSpeed * flightTime + 0.5f * gravity * flightTime * flightTime) / flightTime;

        return new Vector3(vx, vy, 0f);
    }

    #region Callback implementation

    List<string> currentNotes = new List<string>();

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        var midiDevice = device as Minis.MidiDevice;
        if (midiDevice == null) return;

        print($"MIDI Device {change} ({device.description.product})");

        DisconnectAllDevices();
        ConnectAllDevices();
    }

    void OnWillNoteOn(Minis.MidiNoteControl note, float velocity)
    {
        //print($"ON {note.shortDisplayName} ({note.noteNumber:000}) " );
        currentNotes.Add(note.shortDisplayName[0].ToString());
        string temp = "";
        for (int i = 0; i < currentNotes.Count(); i++)
        {
            temp += currentNotes[i] + ", ";
        }
        print(temp);

    }
    void OnWillNoteOff(Minis.MidiNoteControl note)
    {
        //print($"OFF {note.shortDisplayName,3} ({note.noteNumber:000}) ");
        for (int i = 0; i < currentNotes.Count(); i++)
        {
            if (note.shortDisplayName[0].ToString() == currentNotes[i])
            {
                currentNotes.RemoveAt(i);
            }
        }
    }



    #endregion

    #region Device detection

    List<Minis.MidiDevice> _devices = new();

    void ConnectAllDevices()
    {
        foreach (var device in InputSystem.devices)
        {
            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) continue;

            midiDevice.onWillNoteOn += OnWillNoteOn;
            midiDevice.onWillNoteOff += OnWillNoteOff;


            _devices.Add(midiDevice);
        }
    }

    void DisconnectAllDevices()
    {
        foreach (var midiDevice in _devices)
        {
            midiDevice.onWillNoteOn -= OnWillNoteOn;
            midiDevice.onWillNoteOff -= OnWillNoteOff;

        }
        _devices.Clear();
    }

    #endregion


    #region MonoBehaviour implementation

    void OnDestroy()
    {
        DisconnectAllDevices();
        InputSystem.onDeviceChange -= OnDeviceChange;
    }



    #endregion

}