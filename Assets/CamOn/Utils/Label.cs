using UnityEngine;
using System.Collections;

public class Label : MonoBehaviour
{
	private GUIStyle guiStyle; 
    private string text = "";
	public string Text{
		get {return text;}
	}
    private Vector3 screenPosition;

	private Label(){
	}
	
	public bool DisplayObjectName = true;
	
    // Use this for initialization
    void Start ()
    {
    	guiStyle = new GUIStyle ();
    	guiStyle.fontStyle = FontStyle.Bold;
    	guiStyle.normal.textColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (text.Length > 0)
        {
			int textHeight = 20;
			foreach (char c in text)
					if (c == '\n')
						textHeight += 15;
					
            int textWidth = 10 * text.Length;
			if (DisplayObjectName)
				textWidth += 10 * name.Length;
					
            if (gameObject.GetComponent<Camera>() != null)
            {
                GUI.Label(new Rect(10, Screen.height-textHeight-10, textWidth, textHeight), text,guiStyle);
            }
            else
            {
				string label = text;
				if (DisplayObjectName)
					label = name + ": " + label;

                screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                GUI.Box(new Rect(screenPosition.x - textWidth / 2, Screen.height - screenPosition.y - textHeight/2, textWidth, textHeight), label);
            }
        }
    }
	
	public static Label OnScreen {
		get {
			Camera cam = Camera.main;
			if (cam == null)
				cam = Camera.current;
			if (cam == null)
				cam = Camera.main;
#if UNITY_FLASH
			
#else
			if (cam == null)
				cam = Camera.allCameras [0];
#endif
			
			if (cam.gameObject.GetComponent<Label> () == null)
				cam.gameObject.AddComponent<Label> ();
			return cam.gameObject.GetComponent<Label> ();
		}
	}
	
	public static Label OnRenderer(Renderer r){
		if (r.gameObject.GetComponent<Label>() == null)
			r.gameObject.AddComponent<Label>();
		return r.gameObject.GetComponent<Label>();
	}
	
    public void Log(string s)
    {
        text += "\n"+ s;
    }

    public void Write(string s)
    {
        text = s;
    }
}