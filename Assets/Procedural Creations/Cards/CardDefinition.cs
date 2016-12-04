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
    //������������ʵ������û�ã���Ϊ�õ��Լ����Ѿ������õĿ�Ƭ�����Ե����ͻ�ɫ���Լ����ж�����
	public string Text;
	public string Symbol; // Atlas shape name
	public int Pattern;
    //��������������ɱ��Ϸ�õ��ı���
	public string Image;
	public bool FullImage;
    public string CardName;
    //TODO
    /*
    * CardName����������
    * 
    */
    public int Dianshu;
    /*
     * Dianshu:���Ƶ�������������ƴ�㣬�������ڱ�׼�汾�ƺ�ûʲô̫����ô�
     * ������Χ��1-13
     */
    
    public string Huase;
    /*
     * Huase�����ƻ�ɫ�����֮��Ŀ��ܻ��õ�
     * ���飺Fangkuai
     * ���ң�Hongtao
     * ÷����Meihua
     * ���ң�Heitao
     */
    
    public string Classification;
    /*
     * Classification:���Ƶķ���
     * ���ƹ���Ϊ���֣�����ơ������ơ��佫�ƺ���Ϸ�ơ�
     * ����ƣ�"Shenfen"
     * �����ƣ�"Tili"
     * �佫�ƣ�"Wujiang"
     * ��Ϸ�ƣ�"Youxi"  
     */
    public string Function;
    /*
     * Function:��Ϸ�ƵĹ���
     * ��Ϸ�ƿɷ�Ϊ���֣������ơ�����ʱ�����ơ���ʱ�����ơ�װ����
     * �����ƣ�"Jiben"
     * ����ʱ�����ƣ�"Feiyanshijinnang"
     * ��ʱ�����ƣ�"Yanshijinnang"
     * װ���ƣ�"Zhuangbei"
     */
    public string Country;
    /*
     * Country:�佫�ƵĹ���
     * κ����"Wei"
     * �����"Shu"
     * �����"Wu"
     * Ⱥ�ۣ�"Qun"
     */
    public int Xueliang;
    /*
     * Xueliang:�佫��Ѫ������
     * 
     */
    public string Jineng_1;
    public string Jineng_2;
    public string Jineng_3;
    public string Jineng_4;
    /*
     * Jineng:�佫���ܣ���Ŀǰ�����Ļ�����ֻ���ĸ����ܣ�δ����������չ
     * --------------------------κ���佫-----------------------------
     * �ܲ٣�
     *      ���ۣ�"Jianxiong"
     *      [����]��"Hujia"
     * ˾��ܲ��
     *      ������"Fankui"
     *      ��ţ�"Guicai"
     * �ĺ��
     *      ���ң�"Ganglie"
     * ���ɣ�
     *      ͻϮ��"Tuxi"
     * ���ң�
     *      ���£�"Luoyi"
     * ���Σ�
     *      ��ʣ�"Tiandu"
     *      �żƣ�"Yiji"
     * �缧��
     *      �����"Qingguo"
     *      ����"Qingguo"
     * --------------------------����佫------------------------------
     * ������
     *      �ʵ£�"Rende"
     *      [����]:"Jijiang"
     * ����
     *      ��ʥ��"Wusheng"
     * �ŷɣ�
     *      ������"Paoxiao"
     * �������
     *      ���ǣ�"Guanxing"
     *     *�ճǣ�"Kongcheng"
     * ���ƣ�
     *      ������"Longdan"
     * ����
     *     *������"Mashu"
     *      ���"Tieqi"
     * ����Ӣ��
     *      ���ǣ�"Jizhi"
     *     *��ţ�"Qicai"
     * ---------------------------------����佫--------------------------------
     * ��Ȩ��
     *      �ƺ⣺"Zhiheng"
     *     *[��Ԯ]��"Jiuyuan"
     * ������
     *      ��Ϯ��"Qixi"
     * ���ɣ�
     *      �˼���"Keji"
     * �Ƹǣ�
     *      ���⣺"Kurou"
     * ��褣�
     *      Ӣ�ˣ�"Yingzi"
     *      ���䣺"Fanjian"
     * ���ǣ�
     *      ��ɫ��"Guose"
     *      ������"Liuli"
     * ½ѷ��
     *     *ǫѷ��"Qianxun"
     *      ��Ӫ��"Liangying"
     * �����㣺
     *      ������"Jieyin"
     *      �ɼ���"Xiaoji"
     * ---------------------------------Ⱥ���佫------------------------------------
     * ��٢��
     *      ���ȣ�"Jijiu"
     *      ���ң�"Qingnang"
     * ������
     *     *��˫��"Wushuang"
     * ������
     *      ��䣺"Lijian"
     *      ���£�"Biyue"
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

    //�����Լ�������ʱ��ʹ��������ƶ���
    //��Ϸ�Ƴ�ʼ������
    
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
        //ֱ��Ĭ����full image
        FullImage = true;
    }
    
}