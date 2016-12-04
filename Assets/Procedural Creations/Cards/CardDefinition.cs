using UnityEngine;
using System.Collections;

public class CardDefinition : MonoBehaviour
{
	public CardDef Data;
}

[System.Serializable]
public class CardDef
{

	public CardAtlas Atlas;
	public CardStock Stock;
    //下面三个变量实际上我没用，因为用的自己的已经制作好的卡片，所以点数和花色就自己另行定义了
	public string Text;
	public string Symbol; // Atlas shape name
	public int Pattern;
    //后面是我们三国杀游戏用到的变量
	public string Image;
	public bool FullImage;
    public string CardName;
    //TODO
    /*
    * CardName：卡牌名称
    * 
    */
    public int Dianshu;
    /*
     * Dianshu:卡牌点数，可能用于拼点，不过对于标准版本似乎没什么太大的用处
     * 点数范围：1-13
     */
    
    public string Huase;
    /*
     * Huase：卡牌花色，周瑜之类的可能会用到
     * 方块：Fangkuai
     * 红桃：Hongtao
     * 梅花：Meihua
     * 黑桃：Heitao
     */
    
    public string Classification;
    /*
     * Classification:卡牌的分类
     * 卡牌共分为四种：身份牌、体力牌、武将牌和游戏牌。
     * 身份牌："Shenfen"
     * 体力牌："Tili"
     * 武将牌："Wujiang"
     * 游戏牌："Youxi"  
     */
    public string Function;
    /*
     * Function:游戏牌的功能
     * 游戏牌可分为三种：基本牌、非延时锦囊牌、延时锦囊牌、装备牌
     * 基本牌："Jiben"
     * 非延时锦囊牌："Feiyanshijinnang"
     * 延时锦囊牌："Yanshijinnang"
     * 装备牌："Zhuangbei"
     */
    public string Country;
    /*
     * Country:武将牌的国籍
     * 魏国："Wei"
     * 蜀国："Shu"
     * 吴国："Wu"
     * 群雄："Qun"
     */
    public int Xueliang;
    /*
     * Xueliang:武将的血量上限
     * 
     */
    public string Jineng_1;
    public string Jineng_2;
    public string Jineng_3;
    public string Jineng_4;
    /*
     * Jineng:武将技能，从目前来看的话上限只有四个技能，未来可以再扩展
     * --------------------------魏国武将-----------------------------
     * 曹操：
     *      奸雄："Jianxiong"
     *      [护驾]："Hujia"
     * 司马懿：
     *      反馈："Fankui"
     *      鬼才："Guicai"
     * 夏侯惇：
     *      刚烈："Ganglie"
     * 张辽：
     *      突袭："Tuxi"
     * 许褚：
     *      裸衣："Luoyi"
     * 郭嘉：
     *      天妒："Tiandu"
     *      遗计："Yiji"
     * 甄姬：
     *      倾国："Qingguo"
     *      洛神："Qingguo"
     * --------------------------蜀国武将------------------------------
     * 刘备：
     *      仁德："Rende"
     *      [激将]:"Jijiang"
     * 关羽：
     *      武圣："Wusheng"
     * 张飞：
     *      咆哮："Paoxiao"
     * 诸葛亮：
     *      观星："Guanxing"
     *     *空城："Kongcheng"
     * 赵云：
     *      龙胆："Longdan"
     * 马超：
     *     *马术："Mashu"
     *      铁骑："Tieqi"
     * 黄月英：
     *      集智："Jizhi"
     *     *奇才："Qicai"
     * ---------------------------------吴国武将--------------------------------
     * 孙权：
     *      制衡："Zhiheng"
     *     *[救援]："Jiuyuan"
     * 甘宁：
     *      奇袭："Qixi"
     * 吕蒙：
     *      克己："Keji"
     * 黄盖：
     *      苦肉："Kurou"
     * 周瑜：
     *      英姿："Yingzi"
     *      反间："Fanjian"
     * 大乔：
     *      国色："Guose"
     *      琉璃："Liuli"
     * 陆逊：
     *     *谦逊："Qianxun"
     *      连营："Liangying"
     * 孙尚香：
     *      结姻："Jieyin"
     *      枭姬："Xiaoji"
     * ---------------------------------群雄武将------------------------------------
     * 华佗：
     *      急救："Jijiu"
     *      青囊："Qingnang"
     * 吕布：
     *     *无双："Wushuang"
     * 貂蝉：
     *      离间："Lijian"
     *      闭月："Biyue"
     *      
     */

    public CardDef(CardAtlas atlas, CardStock stock, string text, string symbol, int pattern)
	{
		Atlas = atlas;
		Stock = stock;
		Text = text;
		Symbol = symbol;
		Pattern = pattern;
	}

    //我们自己开发的时候都使用这个卡牌定义
    //游戏牌初始化定义
    
    public CardDef(CardAtlas atlas, CardStock stock, string image, string cardname, int dianshu, string huase, string classification, string function)
    {
        Atlas = atlas;
        Stock = stock;
        Image = image;
        CardName = cardname;
        Dianshu = dianshu;
        Huase = huase;
        Classification = classification;
        Function = function;
        //直接默认用full image
        FullImage = true;
    }
    
}