using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class TileEvent : MonoBehaviour
{
    public MapTile mapTile;
    private string BtnText;
    bool isTreasure = false;
    #region SerializeFiled
    [SerializeField] List<TextMeshProUGUI> UIText;
    [SerializeField] TextMeshProUGUI resultText; 
    [SerializeField] GameObject OddEvenGame;
    [SerializeField] GameObject Options;
    [SerializeField] GameObject Option;
    [SerializeField] GameObject MoveBar;
    [SerializeField] Image EndingUI;

    #endregion
    #region List<>
    public List<GetBattleCard> getBattleCards;
    public List<LostItemCard> lostItems;
    public static List<BattleCardData> getbattleCardDatas = new List<BattleCardData>();
    public static List<LostItem> getLostItmeCardDatas = new List<LostItem>();
    #endregion
    #region Bool
    private bool isSpinning = false;
    private bool isLostItem = false;
    #endregion

    #region TileEvent Setting
    public void SetEvent(MapTile _mapTile)
    {
        gameObject.SetActive(true);
        mapTile = _mapTile;
        SetEventUI(mapTile);
    }

    public void SetEventUI(MapTile _mapTile)
    {
        UIText[0].text = _mapTile.tileData.title;
        UIText[1].text = _mapTile.tileData.desc;
        SoundManager soundManager = GameObject.Find("AudioManager").GetComponent<SoundManager>();

        switch (_mapTile.tileData.name)
        {
            case "�ź��� ����":
                Options.SetActive(true);
                soundManager.UISfxPlay(19);
                break;
            case "������ ����":
                SetGetCard(_mapTile.tileData.cardCount[1]);
                soundManager.UISfxPlay(20);
                break;
            case "������":
                OddEvenGame.SetActive(true);
                soundManager.UISfxPlay(21);
                break;
            case "���":
                SetGetCard(_mapTile.tileData.cardCount[1]);
                soundManager.UISfxPlay(22);
                break;
            case "��������":
                SetGetCard(_mapTile.tileData.cardCount[1]);
                soundManager.UISfxPlay(23);
                break;
        }
    }
    public void SetGetCard(int _n)
    {
 
        // ī�� �Ѹ���
        for (int i = 0; i < _n; i++)
        {
            //�緡����
            if (mapTile != null && mapTile.tileData.name == "������ ����")
            {
                lostItems[i].gameObject.SetActive(true);
                float xOffset = CalculateXOffset(_n, i);
                lostItems[i].gameObject.transform.localPosition = new Vector3(xOffset, -90, 0);

                GameObject getLostItemcard = lostItems[i].gameObject;
                LostItem card;

                string randomName = DataManager.instance.AllLostItemList[Random.Range(0, DataManager.instance.AllLostItemDatas.Count)];
                card = DataManager.instance.AllLostItemDatas[randomName];

                // GetBattleCard ������Ʈ�� ���ͼ� ī�带 ����
                var lostcard = getLostItemcard.GetComponent<LostItemCard>();
                lostcard.SetCard(card);

                isLostItem = true;
            }
           
            //���, ��������, ������
            else
            {
                getBattleCards[i].gameObject.SetActive(true);
                float xOffset = CalculateXOffset(_n, i);
                getBattleCards[i].gameObject.transform.localPosition = new Vector3(xOffset, -90, 0);

                //��Ʋ ī�� ����
                GameObject getCard = getBattleCards[i].gameObject;

                string randomName;
                BattleCardData card;
                if (mapTile.tileData.GetOrDelete == "ȹ��")
                {
                    randomName = DataManager.instance.AllBattleCardList[Random.Range(0, DataManager.instance.AllBattleCardDatas.Count)];
                    getBattleCards[i].gameObject.GetComponent<EventTrigger>().enabled = true;
                    card = DataManager.instance.AllBattleCardDatas[randomName];
                }
                else
                {
                    randomName = PlayerData.playerBattleCardDeck[Random.Range(0, PlayerData.playerBattleCardDeck.Count)].name;
                    getBattleCards[i].gameObject.GetComponent<EventTrigger>().enabled = false;
                    card = DataManager.instance.AllBattleCardDatas[randomName];
                    getbattleCardDatas.Add(card);
                }

                // GetBattleCard ������Ʈ�� ���ͼ� ī�带 ����
                var battleCard = getCard.GetComponent<GetBattleCard>();
                battleCard.SetCard(card);

            }
        }
        Option.SetActive(true);
    }

    public void GainTreasure(int _n)
    {
        UIText[0].text = "����";
        UIText[1].text = "";
        isTreasure = true;
        for (int i = 0; i < _n; i++) //���� ���� �� �ϰ� �� ī�庸������ �Ѿ
        {                       
            if (PlayManager.instance.isStone)
            {
                if (i == 2)
                {
                    lostItems[i].gameObject.SetActive(true);
                    float xOffset = CalculateXOffset(_n, i);
                    lostItems[i].gameObject.transform.localPosition = new Vector3(xOffset, -50, 0);

                    GameObject getLostItemcard = lostItems[i].gameObject;
                    LostItem card;

                    string randomName = DataManager.instance.AllLostItemList[Random.Range(0, DataManager.instance.AllLostItemDatas.Count)];
                    card = DataManager.instance.AllLostItemDatas[randomName];

                    // GetBattleCard ������Ʈ�� ���ͼ� ī�带 ����
                    var lostcard = getLostItemcard.GetComponent<LostItemCard>();
                    lostcard.SetCard(card);
                }
                else
                {
                    getBattleCards[i].gameObject.SetActive(true);
                    float xOffset = CalculateXOffset(_n, i);
                    getBattleCards[i].gameObject.transform.localPosition = new Vector3(xOffset, -50, 0);

                    //��Ʋ ī�� ����
                    GameObject getCard = getBattleCards[i].gameObject;

                    string randomName;
                    BattleCardData card;

                    randomName = DataManager.instance.AllBattleCardList[Random.Range(0, DataManager.instance.AllBattleCardDatas.Count)];
                    getBattleCards[i].gameObject.GetComponent<EventTrigger>().enabled = true;
                    card = DataManager.instance.AllBattleCardDatas[randomName];

                    // GetBattleCard ������Ʈ�� ���ͼ� ī�带 ����
                    var battleCard = getCard.GetComponent<GetBattleCard>();
                    battleCard.SetCard(card);
                }
            }
            else
            {
                getBattleCards[i].gameObject.SetActive(true);
                float xOffset = CalculateXOffset(_n, i);
                getBattleCards[i].gameObject.transform.localPosition = new Vector3(xOffset, -50, 0);

                //��Ʋ ī�� ����
                GameObject getCard = getBattleCards[i].gameObject;

                string randomName;
                BattleCardData card;

                randomName = DataManager.instance.AllBattleCardList[Random.Range(0, DataManager.instance.AllBattleCardDatas.Count)];
                getBattleCards[i].gameObject.GetComponent<EventTrigger>().enabled = true;
                card = DataManager.instance.AllBattleCardDatas[randomName];

                // GetBattleCard ������Ʈ�� ���ͼ� ī�带 ����
                var battleCard = getCard.GetComponent<GetBattleCard>();
                battleCard.SetCard(card);
            }
        }

        Option.SetActive(true);
    }

    private float CalculateXOffset(int n, int i)
    {
        if (n == 2)
            return i == 1 ? 200 : -200;
        if (n == 3 && i > 0)
            return i == 2 ? 310 : -310;
        return 0;
    }
    #endregion
    #region TileEvents
      
    //Ȧ, ¦ ����
    public void OddEvenGames()
    {
        //Ŭ���� ��ư�� ������Ʈ ��������
        GameObject ClickButton = EventSystem.current.currentSelectedGameObject;
        //��ư�� text ��������
        BtnText = ClickButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        //��ư Ŭ���� ��ư�� ���� �ٲ�(UI�ٲٸ� ���� ����)
        Image BtnImg = ClickButton.GetComponent<Image>();
        BtnImg.color = Color.green;

        if (!isSpinning)
        {
            StartCoroutine(SpinResult(BtnText,ClickButton));
        }
    }

    private IEnumerator SpinResult(string expectedResult, GameObject _clickBtn)
    {
        GameObject.Find("AudioManager").GetComponent<SoundManager>().UISfxPlay(18);
        isSpinning = true;
        float startTime = 0f;
        RectTransform MoveBarRect = MoveBar.GetComponent<RectTransform>();
        while(true)

        {
            string rantext = Random.Range(0, 2) == 0 ? "Ȧ" : "¦";
            MoveBarRect.DOAnchorPosY(94f, 0.2f);
            resultText.text = rantext;
            startTime += Time.deltaTime;

            if(startTime>=0.45f)
            {
                MoveBarRect.DOAnchorPosY(0f, 0.2f);               
                yield return new WaitForSeconds(1f);
                break;
            }
            yield return new WaitForSeconds(0.3f);

            MoveBar.transform.localPosition = new Vector3(0, -98f, 0);
        }

        // ����� ������
        isSpinning = false;

        MapSystem.instance.tileEffect_UI.resultText.color = Color.red;
        
        // ����� ���� �¸� �Ǵ� �й� �ؽ�Ʈ ǥ��
        if (MapSystem.instance.tileEffect_UI.resultText.text == expectedResult)
        {
            UIText[1].text = "�¸�";
            _clickBtn.GetComponent<Image>().color = Color.white;
            OddEvenGame.SetActive(false);
            SetGetCard(mapTile.tileData.cardCount[1]);
        }
        else
        {
            UIText[1].text = "�й�";
            _clickBtn.GetComponent<Image>().color = Color.white;
            OddEvenGame.SetActive(false);
            mapTile.tileData.GetOrDelete = "����";
            SetGetCard(2);
        }
        
    }
    public void TestofStone()
    {
        GameObject ClickButton = EventSystem.current.currentSelectedGameObject;
        BtnText = ClickButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text;

        if (BtnText == "����")
        {
            PlayManager.instance.curTile = DataManager.instance.AllTileDatas["�������"];
            Options.SetActive(false);
            PlayManager.instance.isStone = true;
            SceneManager.LoadScene("PlayScene");
        }
        else if (BtnText == "����")
        {
            Options.SetActive(false);
            EndEvent();
        }
    }
    #endregion
    #region TileEvent END
    public void EndEvent()
    {
        //����(����, �Ϲ�����)
        if(isTreasure)
        {
            if (PlayManager.instance.isStone)
            {
                
                foreach (var card in getbattleCardDatas)
                {
                    PlayerData.GainCard(card.name);
                }
                foreach (var card in getLostItmeCardDatas)
                {
                    PlayerData.GainLostItem(card.name);
                }
            }
            else
            {
                foreach (var card in getbattleCardDatas)
                {
                    PlayerData.GainCard(card.name);
                }
            }
            resetTileEvent();
            gameObject.SetActive(false);
            Option.SetActive(false);
            isTreasure = false;
            PlayManager.instance.isStone = false;
        }
        //���� ���� 
        else if (BtnText == "����")
        {
            resetTileEvent();
            gameObject.SetActive(false);
            Option.SetActive(false);
        }
        //��������
        else if (mapTile.tileData.name == "��������")
        {
            PlayerData.DeleteCard(getbattleCardDatas[0].name);
            resetTileEvent();
            gameObject.SetActive(false);
            Option.SetActive(false);
        }
        //������, ���
        else if (!isLostItem && getbattleCardDatas.Count <= mapTile.tileData.cardCount[0])
        {
            foreach (var card in getbattleCardDatas)
            {
                if (mapTile.tileData.GetOrDelete == "ȹ��")
                {
                    PlayerData.GainCard(card.name);
                }
                else if (mapTile.tileData.GetOrDelete == "����")
                {
                    PlayerData.DeleteCard(card.name);
                }
            }
            resetTileEvent();
            gameObject.SetActive(false);
            Option.SetActive(false);
        }
        //������ ����
        else if (isLostItem && getLostItmeCardDatas.Count <= mapTile.tileData.cardCount[0])
        {
            foreach (var card in getLostItmeCardDatas)
            {              
                PlayerData.GainLostItem(card.name);
            }
            resetTileEvent();
            gameObject.SetActive(false);
            Option.SetActive(false);
        }     
    }
    public void resetTileEvent()
    {
        getbattleCardDatas.Clear();
        getLostItmeCardDatas.Clear();
        resultText.color = Color.black; 
        isLostItem = false;
        foreach (var card in getBattleCards)
        {
            card.gameObject.GetComponent<Outline>().enabled = false;
            card.transform.transform.localScale = new Vector3(1, 1, 1);           
            card.isSelect = false;
            card.ClickCount = 0;
            card.gameObject.SetActive(false);
        }
        foreach (var card in lostItems)
        {
            card.gameObject.GetComponent<Outline>().enabled = false;
            card.transform.transform.localScale = new Vector3(1, 1, 1);
            card.isSelect = false;
            card.ClickCount = 0;
            card.gameObject.SetActive(false);
        }
        MapSystem.instance.moveCardDraw = true;
    }
    #endregion
}
