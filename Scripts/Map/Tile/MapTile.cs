using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class MapTile : MonoBehaviour
{
    public TileData tileData;

    public TextMeshPro tileName; //Ÿ���̸� Text
    public Button startButton; //��Ʋ���� ��ư
    public SpriteRenderer token;

    void Awake()
    {
        if (GetComponent<SpriteRenderer>() != null) GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.05f, 0.1f)+0.2f, Random.Range(0.1f, 0.2f) + 0.2f, Random.Range(0.05f, 0.1f) + 0.2f, 1);    
    }
    //Ÿ���̸� ����, �̸��� ���� �� ����
    private void Start()
    {
        if (token != null)
        {
            if (DataManager.instance.AllTokenIllusts.Find(x => x.name == tileData.name) != null) token.sprite = DataManager.instance.AllTokenIllusts.Find(x => x.name == tileData.name).sprite;
            else token.sprite = null; 
        }
    }
    public void SetTile(TileData _tileData)
    {
        tileData = _tileData;       
        //����Ÿ���� Ÿ�� �̸��� ������ ������
        if(tileData.name == "����")
        {
            tileName.text = "";
        }
        else
        {
            tileName.text = tileData.name;
        }
    }
    public void TileEffect()
    {     
        PlayManager.instance.curTile = tileData;
        switch (tileData.type)
        {

            case "����":
                SceneManager.LoadScene("PlayScene");
                break;
            case "����":
                MapSystem.instance.tileEffect_UI.SetEvent(this);
                break;
            case "����":
                MapSystem.instance.tileEffect_UI.SetEvent(this);               
                break;

            case "����":              
                MapSystem.instance.tileEffect_UI.SetEvent(this);                
                break;
            default:
                MapSystem.instance.moveCardDraw = true;
                break;

        }
        //GameObject AudioManager = GameObject.Find("AudioManager");
        //AudioManager.GetComponent<SoundManager>().UISfxPlay(18);
    }

}
