using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class BattleCardDataAndTarget
{
    public BattleCard battleCard;
    public Unit target;
    public BattleCardDataAndTarget(BattleCard _battleCard, Unit _target)
    {
        battleCard = _battleCard;
        target = _target;
    }
}

public class BattleCard : MonoBehaviour, IEndDragHandler, IDropHandler, IDragHandler
{
    public BattleCardData battleCardData;
    RectTransform rect;
    BattleSystem system;
    [SerializeField] TextMeshProUGUI cardNameText, cardDesText;
    [SerializeField] Image illust;

    [SerializeField] GameObject diceconditioner;

    DiceConditioner diceConditioner;

    public bool targeting = false;
    public bool enforced = false;

    public bool[] diceCondition = new bool[3];

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    #region Setting

    public void SetCard(BattleCardData _battleCardData)
    {
        battleCardData = _battleCardData;
        illust.sprite = DataManager.instance.AlllBattleCardIllusts.Find(x => x.name == battleCardData.name).sprite;
        cardNameText.text = _battleCardData.name;
        cardNameText.color = Color.white;

        cardDesText.text = "";

        if (_battleCardData.skillData.effects[0] == "����")
        {
            cardDesText.text = "����";
        }
        else
        {
            for (int i = 0; i < _battleCardData.skillData.effects.Count; i++)
            {
                var des = _battleCardData.skillData.effects[i].Split('/')[0].Split(":");

                if (_battleCardData.skillData.effects[i].Split('/').Length > 1) cardDesText.text += "��ü ";

                if (des.Length > 2) cardDesText.text += float.Parse(des[2]) * 100 + "% ";

                cardDesText.text += des[0] + " " + des[1] + "\n";

            }
        }


        //
        if (battleCardData.skillData.enforcedEffects[0] == "����")
        {
            diceconditioner.gameObject.SetActive(false);
        }
        else
        {
            diceconditioner.gameObject.SetActive(true);

            diceConditioner = GetComponent<DiceConditioner>();
            diceConditioner.SetDiceCondition(battleCardData.diceCondition);
        }
    }

    #endregion

    #region Interect

    public void Zoom(bool _zoom) // true�� Ȯ��, ī�� Eventtrigger ������Ʈ���� ���, ���콺�� ������ Ȯ��, �������� ���
    {
        if(_zoom)
        {
            rect.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.2f);
        }
        else
        {
            rect.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        BattleSystem.instance.battleCardDeck.SetHandCardPosition();
        Zoom(false);
        BattleSystem.instance.targeter.SetUseMode(false);
    }
    public void OnDrag(PointerEventData eventData)
    {
        BattleSystem.instance.targeter.SetPosition(ReturnWorldPoint(), battleCardData);
        BattleSystem.instance.targeter.SetUseMode(true);

        if (BattleSystem.instance.targeter.isTargeting && BattleSystem.instance.playerSkillTarget != null)
        {
            foreach(Image i in GetComponentsInChildren<Image>())
            {
                i.color = Color.red;
            }
        }
        else
        {
            foreach (Image i in GetComponentsInChildren<Image>())
            {
                i.color = Color.white;
            }
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        if (eventData.pointerDrag.GetComponent<Dice>() != null)
        {
            if (battleCardData.skillData.enforcedEffects[0] == "����") return;

            var tmpDice = eventData.pointerDrag.GetComponent<Dice>();

            if (ConditionCheck(tmpDice.number) && enforced == false)
            {
                eventData.pointerDrag.GetComponent<Dice>().Use();
                GameObject.Find("AudioManager").GetComponent<SoundManager>().UISfxPlay(25);
                EnforceCard();
            }

        }
        if (eventData.pointerDrag.GetComponent<BattleCard>() != null)
        {
            //1. ���� ������ / 2. ��� ��������
            if (BattleSystem.instance.targeter.isTargeting && BattleSystem.instance.playerSkillTarget != null)
            {
                UseCard();
            }
            GameObject AudioManager = GameObject.Find("AudioManager");
            AudioManager.GetComponent<SoundManager>().UISfxPlay(5);
        }
    }

    bool ConditionCheck(int _number)
    {
        var c = battleCardData.diceCondition.Split('/')[0].Split(':');

        switch (c[0])
        {
            case "����":

                _number += 1;

                var dc = c[1].Split(',');
                if (dc.Length == 1)
                {
                    if (c[1] == "¦��")
                    {
                        if (_number == 2 || _number == 4 || _number == 6) return true;
                        else return false;
                    }
                    else if (c[1] == "Ȧ��")
                    {
                        if (_number == 1 || _number == 3 || _number == 5) return true;
                        else return false;
                    }
                    else if (c[1] == "��ü")
                    {
                        return true;
                    }
                    else if (int.Parse(c[1]) == _number)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    //�������� ���߿�
                }

                break;
            case "����":

                _number += 1;
                dc = c[1].Split('~');
                if (int.Parse(dc[0])  <= _number && int.Parse(dc[1])  >= _number) return true;
                else return false;

            case "�̻�":

                _number += 1;
                if (_number >= int.Parse(c[1])) return true;
                else return false;


            case "����":

                _number += 1;
                if (_number <= int.Parse(c[1])) return true;
                else return false;
        }

        return true;
    }
    public Vector3 ReturnWorldPoint()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return pos;
    }
    #endregion

    public void EnforceCard()
    {
        if (battleCardData.skillData.enforcedEffects.Count <= 0)
        {
            enforced = false;
            return;
        }
        cardNameText.text = '*'+battleCardData.name;
        cardNameText.color = Color.red;

        cardDesText.text = "";


        enforced = true;


        for (int i = 0; i < battleCardData.skillData.enforcedEffects.Count; i++)
        {
            var des = battleCardData.skillData.enforcedEffects[i].Split('/')[0].Split(':');

            if (battleCardData.skillData.enforcedEffects[i].Split('/').Length > 1) cardDesText.text += "��ü ";

            if (des.Length > 2) cardDesText.text += float.Parse(des[2]) * 100 + "% ";

            cardDesText.text += des[0] + " " + des[1] + "\n";

        }
    }
    public void UseCard()
    {
        rect.DOScale(new Vector3(1f, 1f, 1f), 0f);
        BattleSystem.instance.battleCardDeck.UseCard(this);
        BattleSystem.instance.UseBattleCard(new BattleCardDataAndTarget( this, BattleSystem.instance.playerSkillTarget));
        gameObject.SetActive(false);
    }


}
