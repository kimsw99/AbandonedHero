using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class MoveCard : MonoBehaviour
{
    public MoveCardData moveCardData;
    public static MoveCard instance;
    public int cardEffectCount;

    [SerializeField] Image illust;
    [SerializeField] TextMeshProUGUI nameText, desText;

    public List<string> remainEffect = new();
    
    public void SetCard(string cardName)
    {
        moveCardData = DataManager.instance.AllMoveCardDatas[cardName];

        illust.sprite = DataManager.instance.AlllMoveCardIllusts.Find(x => x.name == moveCardData.name).sprite;

        nameText.text = moveCardData.name;

        desText.text = "";

        foreach (string i in moveCardData.effects)
        {
            desText.text += i;
            desText.text += "\n";        
        }  
        
    }

    //카드 클릭 시 발동
    public void SelectCard()
    {
        remainEffect = moveCardData.effects;
        cardEffectCount = 0;

        MoveEffect(); //카드가 가지고 있는 효과, 2개가 잇으면 2부터 효과를 발동할때마다 1씩 감소하는 메서드     

    }

    public void MoveEffect()
    {
        if (cardEffectCount != remainEffect.Count)
        {
            MapSystem.instance.ActMoveCardEffect(remainEffect[cardEffectCount++].Split(':'), this);
            MapSystem.instance.allowEffect = true;
        }
        else if(cardEffectCount >= remainEffect.Count && MapSystem.instance.allowEffect)
        {
            MapSystem.instance.EndCardEffect();
        }
    }
}
