using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSkill : MonoBehaviour
{
    Unit player;
     public void Skill1()
    {
        SkillData skill1 = DataManager.instance.AllSkillDatas["����ġ��"];
        Dmg(skill1);
    }

    public void Skill2()
    {
        SkillData skill2 = DataManager.instance.AllSkillDatas["�˰�"];
        //if(enforced = false){
        Dmg(skill2);
        //}
        //else if(enforced = true){
        //EnforcedDmg(skill2)
        //}
    }
    public void Dmg(SkillData _skill)
    {
        var sk = _skill.effects[0].Split(':');
        int dmg = int.Parse(sk[1]);
        player.damage = dmg;
    }
    public void EnforcedDmg(SkillData _skill)
    {
        var sk = _skill.enforcedEffects[0].Split(':');
        int enforcedDmg = int.Parse(sk[1]);
        player.damage = enforcedDmg;
    }
    public void useSkill(string _name)
    {
        if (_name == "����ġ��")
        {
            Skill1();
        }
        else if(_name == "�˰�")
        {
            Skill2();
        }
    }
}
