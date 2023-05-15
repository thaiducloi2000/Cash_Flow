using Fusion;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dice : NetworkBehaviour {

    public static Dice Instance { get; private set; }
    // Array of dice sides sprites to load from Resources folder
    public Sprite[] diceSides;

    // Reference to sprite renderer to change sprites
    public Image rend;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Use this for initialization
    private void Start () {

        // Assign Renderer component
        rend = GetComponent<Image>();

	}
	
    
}
