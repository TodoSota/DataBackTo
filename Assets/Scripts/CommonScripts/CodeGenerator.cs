using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CodeGenerator : MonoBehaviour
{
    public RawImage BarcordImage;
    public int sidePadding = 20;
    public Color panelColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    private readonly string[] _widthsTable = new string[] {
        "212222", "222122", "222221", "121223", "121322",
        "131222", "122213", "122312", "132212", "221213",
        "221312", "231212", "112232", "122132", "122231",
        "113222", "123122", "123221", "223211", "221132",
        "221231", "213212", "223112", "312131", "311222",
        "321122", "321221", "312212", "322112", "322211",
        "212123", "212321", "232121", "111323", "131123", 
        "131321", "112313", "132113", "132311", "211313",
        "231113", "231311", "112133", "112331", "132131",
        "113123", "113321", "133121", "313121", "211331",
        "231131", "213113", "213311", "213131", "311123",
        "311321", "331121", "312113", "312311", "332111",
        "314111", "221411", "431111", "111224", "111422",
        "121124", "121421", "141122", "141221", "112214",
        "112412", "122114", "122411", "142112", "142211",
        "241211", "221114", "413111", "241112", "134111",
        "111242", "121142", "121241", "114212", "124112",
        "124211", "411212", "421112", "421211", "212141",
        "214121", "412121", "111143", "111341", "131141",
        "114113", "114311", "411113", "411311", "113141",
        "114131", "311141", "411131", "211412", "211214",
        "211232", "2331112"
    };

    public void CreateCodeSlit(string text)
    {
        List<int> widths = new List<int>();
        int checkSum = 104;

        // スタートのCodeB
        widths.AddRange(GetPatternWidths(104));

        for (int i = 0; i < text.Length; i++)
        {
            // CodeBはASCIIの32番目の文字が0番目に対応している
            int codeValue = text[i] - 32;
            if (codeValue < 0 || codeValue > 94) continue;
            
            // checkDigitは[何文字目] * 文字の番号
            // iは0始まりだから＋１する
            checkSum += codeValue * (i + 1);
            widths.AddRange(GetPatternWidths(codeValue));
        }

        widths.AddRange(GetPatternWidths(checkSum % 103));
        widths.AddRange(GetPatternWidths(106));

        Draw(widths);
    }

    private IEnumerable<int> GetPatternWidths(int index)
    {
        return _widthsTable[index].Select(c => c - '0');
    }

    private void Draw(List<int> widths)
    {
        int total = widths.Sum() + (sidePadding * 2);
        Texture2D tex = new Texture2D(total, 1, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;

        int x = 0;
        bool isBar = true;

        FillPadding(tex, ref x);

        foreach (int w in widths)
        {
            Color color = isBar ? Color.clear : panelColor;
            for (int i = 0; i < w; i++)
            {
                tex.SetPixel(x++, 0, color);
            }
            isBar = !isBar;
        }

        FillPadding(tex, ref x);

        tex.Apply();
        BarcordImage.texture = tex;
    }
    
    private void FillPadding(Texture2D tex, ref int x)
    {
        for (int i = 0; i < sidePadding; i++)
        {
            tex.SetPixel(x++, 0, panelColor);
        }
    }
}
