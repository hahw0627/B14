using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCsv : MonoBehaviour
{
    Dictionary<string, EquipmentData> equipmentDic = new Dictionary<string, EquipmentData>();
    void Start()
    {
        LoadEquipmentData();
    }
    void LoadEquipmentData()
    {
        string path = "CSV/";
        var equipmentcsvData = Resources.Load<TextAsset>(path + "EquipmentCSVData");
        Deserial(equipmentcsvData.text.TrimEnd()); // trimend 를 쓰면 csv 마지막 공백을 제외한체 들어온다. 
    }

    void Deserial(string data)
    {
        var rowData = data.Split('\n');             //string[]
        for (int i = 1; i < rowData.Length; i++)
        {
            var datas = rowData[i].Split(',');

            string itemCode = datas[0];
            Define.GachaRarity gachaRarity = (Define.GachaRarity)System.Enum.Parse(typeof(Define.GachaRarity), datas[1]);
            Define.EquipmentType equipmentType = (Define.EquipmentType)System.Enum.Parse(typeof(Define.EquipmentType), datas[2]);
            Define.EquipmentGrade equipmentGrade = (Define.EquipmentGrade)System.Enum.Parse(typeof(Define.EquipmentGrade), datas[3]);

            string nameText = datas[4];
            string descriptionText = datas[5];
            string spriteName = datas[6];
            int atk = int.Parse(datas[7]);
            int def = int.Parse(datas[8]);

            string path = "EquipmentSprite/";
            Sprite sprite = Resources.Load<Sprite>(path + spriteName);

            EquipmentData equipmentData = new EquipmentData
            {
                id = itemCode,
                gachaRarity = gachaRarity,
                equipmentGrade = equipmentGrade,
                equipmentType = equipmentType,
                equipmentName = nameText,
                description = descriptionText,
                spriteName = spriteName,
                sprite = sprite,
                atackPower = atk,
                defense = def
                //추가 

            };

            equipmentDic.Add(itemCode, equipmentData);
        }
    }

    public EquipmentData GetEquipmentByCode(string itemCode)
    {
        if (equipmentDic.ContainsKey(itemCode))
        {
            Debug.Log(itemCode+"코드불러옴");
            return equipmentDic[itemCode];
        }
        Debug.Log("딕셔너리에 아이템코드가 없음");
        return null;
    }

}

