using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;
using UnityEngine.UI;

public class JSONReader : MonoBehaviour
{    
    //Title Text
    public Text Title;
    //Prefab Headers
    public Text HeadersPrefab;
    //Prefab Texts
    public Text TextsPrefab;
    //Panel
    public CanvasRenderer panel;
    public RectTransform panelRectTransform;

    void Start() {
        ReadJSON();        
    }

    public void ReloadJSON() {
        foreach (Transform child in panel.transform) {
            GameObject.Destroy(child.gameObject);
        }
        ReadJSON();
    }

    private void ReadJSON() {
        string jsonString;
        string Header; 
        float width;
        int numberOfHeaders = 0;
        float spaceBetweenX = 0;
        float PanelSizeX;
        float PanelSizeY;

        //Read the JSON and set the Title
        jsonString = File.ReadAllText(Application.dataPath + "/StreamingAssets/JsonChallenge.json");
        JSONNode data = JSON.Parse(jsonString);
        Title.text = data["Title"].Value;

        //Detect the number of headers
        foreach (JSONNode item in data["ColumnHeaders"]) {
            numberOfHeaders++;
        }

        //Detect size of painel
        PanelSizeX = Mathf.Abs(panelRectTransform.rect.x * 2);
        PanelSizeY = Mathf.Abs(panelRectTransform.rect.y * 2);

        //Set the width base on the max size and the number of headers
        width = PanelSizeX / numberOfHeaders;

        //Read and set the headers
        foreach (JSONNode item in data["ColumnHeaders"]) {
            //Remove quotation marks
            Header = item.ToString();
            Header = Header.Remove(0, 1);
            Header = Header.Remove(Header.Length - 1, 1);
            
            //Instantiate the Header
            Text HeaderTextBox = Instantiate(HeadersPrefab, new Vector3(spaceBetweenX, 0,0), transform.rotation) as Text;
            HeaderTextBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            HeaderTextBox.transform.SetParent(panel.transform, false);
            HeaderTextBox.text = Header;

            //Instantiate the TextBox
            Text TextBox = Instantiate(TextsPrefab, new Vector3(spaceBetweenX, -50, 0), transform.rotation) as Text;
            TextBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            TextBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, PanelSizeY - 50);
            TextBox.transform.SetParent(panel.transform, false);

            //Set the X location for the new column
            spaceBetweenX = spaceBetweenX + width;

            //Read and add the text to the TextBox
            foreach (JSONNode itens in data["Data"]) {
                TextBox.text = TextBox.text + itens[Header].Value.ToString() + "\n" + "\n";
            }
        }
        
    }
}
