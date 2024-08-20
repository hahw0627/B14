using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private Button button4;
    [SerializeField] private Button button5;
    [SerializeField] private Button button6;

    [SerializeField]
    private TextMeshProUGUI text;

    private void Start()
    {
        // 각 버튼에 클릭 이벤트를 등록합니다.
        button1.onClick.AddListener(() => ChangeText(button1));
        button2.onClick.AddListener(() => ChangeText(button2));
        button3.onClick.AddListener(() => ChangeText(button3));
        button4.onClick.AddListener(() => ChangeText(button4));
        button5.onClick.AddListener(() => ChangeText(button5));
        button6.onClick.AddListener(() => ChangeText(button6));
    }

    public void ChangeText(Button btn)
    {
        switch (btn.name)
        {
            case "Button1":
                text.text = "게임을 종료 하고 다시 접속하면 시간을 계산해서 방치 보상을 줍니다.\n" +
                            "광고를 보고 특수 재화인 GemStone 보상을 얻을 수 있습니다.\n" +
                            "(연속 시청 방지를 위해 일정 시간 동안 버튼이 비활성화 됩니다.)";
                break;
            case "Button2":
                text.text = "Gold는 몬스터 처치, 방치 보상, 상점의 교환 시스템으로 획득 가능합니다.\n" +
                            "Gem은 보스 몬스터 처치, 광고 보상, 상점의 교환 시스템으로 획득 가능합니다.";
                break;
            case "Button3":
                text.text = "뽑기를 통해 같은 스킬과 동료가 나와 5개가 모이면 강화를 할 수 있습니다.";
                break;
            case "Button4":
                text.text = "능력치는 총 6개입니다. 버튼에 나온 금액만큼 사용하여 해당 스탯을 강화합니다.\n" +
                            "단, 강화할 때마다 강화 비용이 점점 늘어나니 전략을 세워 강화해보세요!";
                break;
            case "Button5":
                text.text = "배속 버튼을 누르면 게임의 속도가 빨라져 좀 더 게임의 시원함을 느낄 수 있습니다.";
                break;
            case "Button6":
                text.text = "스킬을 장착하고 자동 스킬 버튼을 누르면 자동으로 스킬이 날아가 전투에 도움을 줍니다.\n" +
                            "강력한 스킬들을 장착해 자동 버튼을 사용하여 전투에 대한 편안함을 느껴보세요.";
                break;
            default:
                text.text = "알 수 없는 버튼이 눌렸습니다.";
                break;
        }
    }
}
