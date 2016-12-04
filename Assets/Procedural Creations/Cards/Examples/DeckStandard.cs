using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeckStandard : CardDeck
{
	public CardAtlas Atlas;
	public CardStock Stock;
	
	public override void Initialize()
	{
		if (Atlas == null) { Debug.LogError("CardAtlas is not initialized."); }
		if (Stock == null) { Debug.LogError("CardStock is not initialized."); }

		Debug.Log("Atlas = "+Atlas.name);
		string [] suits = new string[]{"Heart","Spade","Diamond","Club"};
		string [] prefixes = new string[]{"H-","S-","D-","C-"};
		List<CardDef> defs = new List<CardDef>();
        //开始导入游戏牌（108张）
        #region 基本牌定义(53张)
        //"杀"牌定义(30张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_02_Meihua_Sha01", "Sha", 2, "Meihua", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_03_Meihua_Sha02", "Sha", 3, "Meihua", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_04_Meihua_Sha03", "Sha", 4, "Meihua", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_05_Meihua_Sha04", "Sha", 5, "Meihua", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_06_Fangkuai_Sha05", "Sha", 6, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_06_Meihua_Sha06", "Sha", 6, "Meihua", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_07_Fangkuai_Sha07", "Sha", 7, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_07_Heitao_Sha08", "Sha", 7, "Heitao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_07_Meihua_Sha09", "Sha", 7, "Meihua", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_08_Fangkuai_Sha10", "Sha", 8, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_08_Heitao_Sha11", "Sha", 8, "Heitao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_08_Heitao_Sha12", "Sha", 8, "Heitao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_08_Meihua_Sha13", "Sha", 8, "Meihua", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_08_Meihua_Sha14", "Sha", 8, "Meihua", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_09_Fangkuai_Sha15", "Sha", 9, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_09_Heitao_Sha16", "Sha", 9, "Heitao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_09_Heitao_Sha17", "Sha", 9, "Heitao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_09_Meihua_Sha18", "Sha", 9, "Meihua", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_09_Meihua_Sha19", "Sha", 9, "Meihua", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_10_Fangkuai_Sha20", "Sha", 10, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_10_Heitao_Sha21", "Sha", 10, "Heitao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_10_Heitao_Sha22", "Sha", 10, "Heitao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_10_Hongtao_Sha23", "Sha", 10, "Hongtao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_10_Hongtao_Sha24", "Sha", 10, "Hongtao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_10_Meihua_Sha25", "Sha", 10, "Meihua", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_10_Meihua_Sha26", "Sha", 10, "Meihua", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_11_Hongtao_Sha27", "Sha", 11, "Hongtao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_11_Meihua_Sha28", "Sha", 11, "Meihua", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_11_Meihua_Sha29", "Sha", 11, "Meihua", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_13_Fangkuai_Sha30", "Sha", 13, "Fangkuai", "Youxi", "Jiben"));

        //"闪"牌定义(15张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_02_Fangkuai_Shan01", "Shan",2, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_02_Fangkuai_Shan02", "Shan",2, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_02_Hongtao_Shan03", "Shan",2, "Hongtao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_02_Hongtao_Shan04", "Shan",2, "Hongtao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_03_Fangkuai_Shan05", "Shan",3, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_04_Fangkuai_Shan06", "Shan",4, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_05_Fangkuai_Shan07", "Shan",5, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_06_Fangkuai_Shan08", "Shan",6, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_07_Fangkuai_Shan09", "Shan",7, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_08_Fangkuai_Shan10", "Shan",8, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_09_Fangkuai_Shan11", "Shan",9, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_10_Fangkuai_Shan12", "Shan",10, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_11_Fangkuai_Shan13", "Shan",11, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_11_Fangkuai_Shan14", "Shan",11, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_13_Hongtao_Shan15", "Shan",13, "Hongtao", "Youxi", "Jiben"));

        //"桃"牌定义(8张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_03_Hongtao_Tao01", "Tao",3, "Hongtao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_04_Hongtao_Tao02", "Tao",4, "Hongtao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_06_Hongtao_Tao03", "Tao",6, "Hongtao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_07_Hongtao_Tao04", "Tao",7, "Hongtao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_08_Hongtao_Tao05", "Tao",8, "Hongtao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_09_Hongtao_Tao06", "Tao",9, "Hongtao", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_12_Fangkuai_Tao07", "Tao",12, "Fangkuai", "Youxi", "Jiben"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_12_Hongtao_Tao08", "Tao",12, "Hongtao", "Youxi", "Jiben"));
        #endregion

        #region 非延时锦囊牌定义（36张)
        //"决斗"牌定义(3张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_01_Fangkuai_Juedou01", "Juedou",1, "Fangkuai", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_01_Heitao_Juedou02", "Juedou",1, "Heitao", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_01_Meihua_Juedou03", "Juedou",1, "Meihua", "Youxi", "Feiyanshijinnang"));
        //"桃园结义"牌定义(1张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_01_Hongtao_Taoyuanjieyi", "Taoyuanjieyi", 1, "Hongtao", "Youxi", "Feiyanshijinnang"));
        //"万箭齐发"牌定义(1张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_01_Hongtao_Wanjianqifa", "Wanjianqifa", 1, "Hongtao", "Youxi", "Feiyanshijinnang"));
        //"顺手牵羊"牌定义(5张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_03_Fangkuai_Shunshouqianyang01", "Shunshouqianyang", 3, "Fangkuai", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_03_Heitao_Shunshouqianyang02", "Shunshouqianyang", 3, "Heitao", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_04_Fangkuai_Shunshouqianyang03", "Shunshouqianyang", 4, "Fangkuai", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_04_Heitao_Shunshouqianyang04", "Shunshouqianyang", 4, "Heitao", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_11_Heitao_Shunshouqianyang05", "Shunshouqianyang", 11, "Heitao", "Youxi", "Feiyanshijinnang"));
        //"过河拆桥"牌定义(6张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_03_Heitao_Guohechaiqiao01", "Guohechaiqiao",3, "Heitao", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_03_Meihua_Guohechaiqiao02", "Guohechaiqiao",3, "Meihua", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_04_Heitao_Guohechaiqiao03", "Guohechaiqiao",4, "Heitao", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_04_Meihua_Guohechaiqiao04", "Guohechaiqiao",4, "Meihua", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_12_Heitao_Guohechaiqiao05", "Guohechaiqiao",12, "Heitao", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_12_Hongtao_Guohechaiqiao06", "Guohechaiqiao",12, "Hongtao", "Youxi", "Feiyanshijinnang"));
        //"五谷丰登"牌定义(2张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_03_Hongtao_Wugufengdeng01", "Wugufengdeng", 3, "Hongtao", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_04_Hongtao_Wugufengdeng02", "Wugufengdeng", 4, "Hongtao", "Youxi", "Feiyanshijinnang"));
        //"南蛮入侵"牌定义(3张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_07_Heitao_Nanmanruqin01", "Nanmanruqin", 7, "Heitao", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_07_Meihua_Nanmanruqin02", "Nanmanruqin", 7, "Meihua", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_13_Heitao_Nanmanruqin03", "Nanmanruqin", 13, "Heitao", "Youxi", "Feiyanshijinnang"));
        //"无中生有"牌定义(4张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_07_Hongtao_Wuzhongshengyou01", "Wuzhongshengyou", 7, "Hongtao", "Youxi", "Feiyanchijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_08_Hongtao_Wuzhongshengyou02", "Wuzhongshengyou", 8, "Hongtao", "Youxi", "Feiyanchijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_09_Hongtao_Wuzhongshengyou03", "Wuzhongshengyou", 9, "Hongtao", "Youxi", "Feiyanchijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_11_Hongtao_Wuzhongshengyou04", "Wuzhongshengyou", 11, "Hongtao", "Youxi", "Feiyanchijinnang"));
        //"无懈可击"牌定义(4张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_11_Heitao_Wuxiekeji01", "Wuxiekeji", 11, "Heitao", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_12_Fangkuai_Wuxiekeji02", "Wuxiekeji", 12, "Fangkuai", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_12_Meihua_Wuxiekeji03", "Wuxiekeji", 12, "Meihua", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_13_Meihua_Wuxiekeji04", "Wuxiekeji", 13, "Meihua", "Youxi", "Feiyanshijinnang"));
        //"借刀杀人"牌定义(2张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_12_Meihua_Jiedaosharen01", "Jiedaosharen", 12, "Meihua", "Youxi", "Feiyanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_13_Meihua_Jiedaosharen02", "Jiedaosharen", 13, "Meihua", "Youxi", "Feiyanshijinnang"));
        #endregion

        #region 延时锦囊牌定义(5张)
        //"闪电"牌定义(2张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_01_Heitao_Shandian01", "Shandian", 1, "Heitao", "Youxi", "Yanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_12_Hongtao_Shandian02", "Shandian", 12, "Hongtao", "Youxi", "Yanshijinnang"));
        //"乐不思蜀"牌定义(3张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_06_Heitao_Lebusishu01", "Lebusishu", 6, "Heitao", "Youxi", "Yanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_06_Hongtao_Lebusishu02", "Lebusishu", 6, "Hongtao", "Youxi", "Yanshijinnang"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_06_Meihua_Lebusishu03", "Lebusishu", 6, "Meihua", "Youxi", "Yanshijinnang"));
        #endregion

        #region 装备牌定义(19张)
        //"武器"牌定义(10张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_01_Fangkuai_Zhugeliannu01", "Zhugeliannu", 1, "Fangkuai", "Youxi", "Zhuangbei"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_01_Meihua_Zhugeliannu02", "Zhugeliannu", 1, "Meihua", "Youxi", "Zhuangbei"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_02_Heitao_Cixiongshuanggujian", "Cixiongshuanggujian", 2, "Heitao", "Youxi", "Zhuangbei"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_02_Heitao_Hanbingjian", "Hanbingjian", 2, "Heitao", "Youxi", "Zhuangbei"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_05_Fangkuai_Guanshifu", "Guanshifu", 5, "Fangkuai", "Youxi", "Zhuangbei"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_05_Heitao_Qinglongyanyuedao", "Qinglongyanyuedao", 5, "Heitao", "Youxi", "Zhuangbei"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_05_Hongtao_Qilingong", "Qilingong", 5, "Hongtao", "Youxi", "Zhuangbei"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_06_Heitao_Qinggangjian", "Qinggangjian", 6, "Heitao", "Youxi", "Zhuangbei"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_12_Fangkuai_Fangtianhuaji", "Fangtianhuaji", 12, "Fangkuai", "Youxi", "Zhuangbei"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_12_Heitao_Zhangbashemao", "Zhangbashemao", 12, "Heitao", "Youxi", "Zhuangbei"));
        //"防具"牌定义(3张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_02_Heitao_Baguazhen01", "Baguazhen", 2, "Heitao", "Youxi", "Zhuangbei"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_02_Meihua_Baguazhen02", "Baguazhen", 2, "Meihua", "Youxi", "Zhuangbei"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_02_Meihua_Renwangdun", "Renwangdun", 2, "Meihua", "Youxi", "Zhuangbei"));
        //"+1马"牌定义(3张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_05_Heitao_Jiayi01", "Jiayi", 5, "Heitao", "Youxi", "Zhuangbei"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_05_Meihua_Jiayi02", "Jiayi", 5, "Meihua", "Youxi", "Zhuangbei"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_13_Hongtao_Jiayi03", "Jiayi", 13, "Hongtao", "Youxi", "Zhuangbei"));
        //"-1马"牌定义(3张)
        defs.Add(new CardDef(Atlas, Stock, "Youxi_05_Hongtao_Jianyi01", "Jianyi", 5, "Hongtao", "Youxi", "Zhuangbei"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_13_Fangkuai_Jianyi02", "Jianyi", 13, "Fangkuai", "Youxi", "Zhuangbei"));
        defs.Add(new CardDef(Atlas, Stock, "Youxi_13_Heitao_Jianyi03", "Jianyi", 13, "Heitao", "Youxi", "Zhuangbei"));
        #endregion

        //下面这段是在21点里才会用的
        /*
        for (int i=0; i<4; ++i)
		{
			//int ii = i*13;
			string symbol = suits[i];
			defs.Add( new CardDef(Atlas,Stock,"A",symbol,1) );
			defs.Add( new CardDef(Atlas,Stock,"2",symbol,2) );
			defs.Add( new CardDef(Atlas,Stock,"3",symbol,3) );
			defs.Add( new CardDef(Atlas,Stock,"4",symbol,4) );
			defs.Add( new CardDef(Atlas,Stock,"5",symbol,5) );
			defs.Add( new CardDef(Atlas,Stock,"6",symbol,6) );
			defs.Add( new CardDef(Atlas,Stock,"7",symbol,7) );
			defs.Add( new CardDef(Atlas,Stock,"8",symbol,8) );
			defs.Add( new CardDef(Atlas,Stock,"9",symbol,9) );
			defs.Add( new CardDef(Atlas,Stock,"10",symbol,10) );
			string prefix = prefixes[i];
			CardDef jj = new CardDef(Atlas,Stock,"J",symbol,0);
            jj.Image = prefix+"Jack";
            jj.FullImage = true;
            //jj.Image = "Jiben-Sha";
			defs.Add(jj);
			CardDef qq = new CardDef(Atlas,Stock,"Q",symbol,0);
			qq.Image = prefix+"Queen";
			defs.Add( qq );
			CardDef kk = new CardDef(Atlas,Stock,"K",symbol,0);
			kk.Image = prefix+"King";
			defs.Add( kk );
		}
        */

        m_itemList = new DeckItem[defs.Count];
		for (int i=0; i<defs.Count; ++i)
		{
			DeckItem item = new DeckItem();
			item.Count = 1;
			item.Card = defs[i];
			m_itemList[i] = item;
		}
        
	}
}
