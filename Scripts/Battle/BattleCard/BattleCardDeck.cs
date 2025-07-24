using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
public class BattleCardDeck : MonoBehaviour
{
    [SerializeField] RectTransform[] battleCardPool;

    //�ڵ� �� ��
    //�ϳ� ���� Ŀ����, ������ �۾�����
    //�� ���� ���������� �ٽ� �����·� ����

    List<BattleCardData> instantBattleCardData = new List<BattleCardData>();
    List<BattleCard> curHands = new List<BattleCard>();

    public Pocket pocket;


    public int curHandCardCount = 3; // �̰Ŵ� �ǽð����� ����Ǵ� ���� ����
    public int bonusHandCardCount = 0;
   
    public void SetPlayerTurn()
    {
        ResetCard();

        curHandCardCount = PlayerData.handCount + bonusHandCardCount;
        bonusHandCardCount = 0;
        SetHandCardData();
        SetHand(); //�ʱ�ȭ�� ���� ����
        SetDices();
        GameObject AudioManager = GameObject.Find("AudioManager");
        AudioManager.GetComponent<SoundManager>().UISfxPlay(1);
    }
    void SetDices()
    {
        foreach (RectTransform i in pocket.dices)
        {
            i.GetComponent<Dice>().Set();
        }
    }

    public void RerollDice(int _c)
    {
        BattleSystem.instance.curDiceCount += _c;
        pocket.ReturnDice();
        pocket.PocketClick();
    }

    void ResetCard()
    {
        foreach(RectTransform j in battleCardPool)
        {
            ResetColor(j);
        }
    }

    void ResetColor(RectTransform _tmp)
    {
        var tmp = _tmp.GetComponent<BattleCard>();
        tmp.enforced = false;
        foreach (Image i in tmp.GetComponentsInChildren<Image>())
        {
            i.color = Color.white;
        }
    }

    public void SetBattleCardDeck()
    {
        instantBattleCardData = PlayerData.playerBattleCardDeck.ToList(); // �� 
    }

    public void SetHand() //�Ű����� ����?
    {
        foreach (RectTransform i in battleCardPool) i.gameObject.SetActive(false);

        SetHandCardPosition();
    }

    public void AddHand(BattleCardData _battleCardData = null) //null�̸� ��ο�
    {
        SetHandCardPosition();
        curHandCardCount++;
        var battleCard = battleCardPool[System.Array.FindIndex(battleCardPool, x => !x.gameObject.activeSelf)].GetComponent<BattleCard>();
        if (_battleCardData == null)
        {
            var randomInt = Random.Range(0, instantBattleCardData.Count);
            battleCard.SetCard(instantBattleCardData[randomInt]);
            instantBattleCardData.RemoveAt(randomInt);
        }
        else
        {
            battleCard.SetCard(_battleCardData);
        }
        curHands.Add(battleCard); //������ ��ġ �� ����
        SetHandCardPosition();
        ArrangeCurHand();
    }

    void ArrangeCurHand()
    {
        var tmp = new List<BattleCard>();
        foreach (RectTransform i in battleCardPool)
        {
            if (i.gameObject.activeSelf)
            {
                tmp.Add(i.GetComponent<BattleCard>());
            }
            else continue;
        }
        curHands = tmp;
    }

    public void SetHandCardData()
    {
        for (int i = 0; i < curHandCardCount; i++)
        {
            var randomInt = Random.Range(0, instantBattleCardData.Count);
            var battleCard = battleCardPool[i].GetComponent<BattleCard>();
            battleCard.SetCard(instantBattleCardData[randomInt]);
            instantBattleCardData.RemoveAt(randomInt);
            curHands.Add(battleCard);

            if (instantBattleCardData.Count <= 0) SetBattleCardDeck();
        }
    }

    public void SetHandCardPosition()
    {
        int i = 0;
        foreach(BattleCard j in curHands)
        {
            DOTween.Kill(j.GetComponent<RectTransform>());

            j.gameObject.SetActive(true);
            j.GetComponent<RectTransform>().DOAnchorPos(new Vector2(800 + (curHandCardCount * 20) - (200 - 10 * curHandCardCount) * (curHandCardCount - i + 1),
                -400 - System.MathF.Abs(curHandCardCount / 2 - i) * 20), 0.2f * (i + 1));
            j.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -5 + (5 * curHandCardCount) - (10 * i)), 0.2f * (i + 1));

            i++;
        }
    }
    public void UseCard(BattleCard _battleCard)
    {
        curHandCardCount--;
        curHands.Remove(_battleCard);
        SetHandCardPosition();
    }

    public void EndTurn()
    {
        curHandCardCount = 0;
        curHands.Clear();
        foreach (RectTransform i in battleCardPool)
        {
            i.DOAnchorPos(new Vector2(0, -850), 1).OnComplete(() => i.gameObject.SetActive(false));
        }
    }
}
