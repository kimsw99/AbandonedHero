using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class MoveCardDeck : MonoBehaviour
{
    [SerializeField] List<MoveCard> cards;
    [SerializeField] int handPoint; // �ڵ带 �� �� ���� �� ���ϴ� �ɷ�ġ, �÷��̾� �ɷ�ġ���� ������. �ӽ÷� ���⿡ ����
    [SerializeField] MoveCardData movecardData;

    string[] commonNames;
    bool isCardPositionSet = false;

    public void SetHand()
    {   
        if (MapSystem.instance.moveCardDraw == true) // �̵�ī�� �̱⸦ �ѹ��� ���� 
        {
            PositioningCard();

            //ī�� ���� �߰� ��
            for (int i = 0; i < handPoint; i++) 
                cards[i].SetCard(CardPer());

            MapSystem.instance.moveCardDraw = false;
            MapSystem.instance.cardHideButton.SetActive(true);
        }

        GameObject AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().UISfxPlay(1);

    }

    public void PositioningCard() //�̵��ϴ� �κи� �и�
    {
        if(!isCardPositionSet)
        {

            ResetCardPosition();

            //ī�� �Ѹ���
            var center = -600 + Random.Range(-50, 50f) - 400 / 2 * handPoint;

            for (int i = 0; i < handPoint; i++)
            {
                var cardRect = cards[i].GetComponent<RectTransform>();

                cardRect.gameObject.SetActive(true);
                cardRect.DOAnchorPos(new Vector3(center + 400 * i, Random.Range(-50, 250f)), 1 - i * 0.2f).SetEase(Ease.OutCirc).OnComplete(() => cardRect.GetComponent<Button>().interactable = true);
                cardRect.DORotate(new Vector3(0, 0, Random.Range(-10, 10)), 2);

                isCardPositionSet = true;
            }
        }
        else // ī�� ġ���
        {

            ResetCardPosition();

            foreach (MoveCard i in cards)
            {
                //ī�� ������� ȿ�� �߰� 11-28
                var cardRect = i.GetComponent<RectTransform>();

                i.GetComponent<Button>().interactable = false;

                cardRect.DOAnchorPos(new Vector3(-680, -600), 1).SetEase(Ease.OutCirc);
                cardRect.DORotate(new Vector3(0, 0, 0), 1).OnComplete(() => i.gameObject.SetActive(false));
            }

            isCardPositionSet = false;
        }
    }

    void ResetCardPosition()//��ġ ����
    {
        foreach (MoveCard i in cards)
        {
            var cardRect = i.GetComponent<RectTransform>();

            DOTween.Kill(cardRect);
            if(MapSystem.instance.moveCardDraw) cardRect.anchoredPosition = new Vector2(0, 0);
        }
    }

    // �̵� ī�� ����
    public string CardPer()
    {

        commonNames = new string[] { "�ȱ�", "�޸���", "���� ����", "�غ�", "�߰�", "���ɽ����� �߰���" };
        MapSystem.instance.allowEffect = true;      

        if (MapSystem.curTileNum >= 3) // �̵� -3 ����
        {
            string[] lastNames = 
                commonNames.Concat(new string[] { "�ް�����", "������ ����", "����ġ��", "��ħ�� ����", "�߸� �λ�"}).ToArray();
            return GetRandomName(lastNames);
            
        }
        else if (MapSystem.curTileNum >= 2) // �̵� -2 ����
        {
            string[] middleNames = commonNames.Concat(new string[] { "�ް�����", "������ ����","�߸� �λ�" }).ToArray();
            return GetRandomName(middleNames);
        }
        else if (MapSystem.curTileNum >= 0) // �̵� - �Ұ��� 
        {
            return GetRandomName(commonNames);
        }
        else
        {
            return "";
        }
    }

    private string GetRandomName(string[] nameList)
    {
        var wrPicker = new WeightRandomPick<string>();
         
        foreach (string name in nameList)
        {
            int CardWeight = int.Parse(DataManager.instance.AllMoveCardDatas[name].weight);       
            wrPicker.Add(name,CardWeight);
        }

        return wrPicker.GetRandomPick(); ;
    }



}
