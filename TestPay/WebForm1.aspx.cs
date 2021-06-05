using Common.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestPay
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //君临支付//
            /// JunLinPay a = new JunLinPay();
            // a.RechargeNotify("mch_id=100216&out_order_no=102258&order_state=1&order_amount=300.00&pay_amount=300.00&sign=94dc2bfb9feb672f48134a24efc0bfc8", "e043fb5cf891243917fd7fcf4b56be0535c751fb");//支付回调测试
            //  a.Recharge("100008", "wechat_qr", new Random().Next(1000, 9999).ToString(), System.DateTime.Now, 500, "https://www.baidu.com", "https//www.baidu.com", "https://www.biadu.com", "192.168.1.1", "559fbd27a3065b288b3f871d35345b22d082eb94", "test", "https://jlpay-api.com/gateway");//支付请求
            //a.RechargeQuery("", "", "", "");//支付查询
            //安付支付alipay   weixin
            // afpay a = new afpay();
            // a.Recharge("TEST", "alipay", new Random().Next(1000, 9999).ToString(), System.DateTime.Now, 5000, "http://api.hqcp88888.com/user/notify/afpay", "https//www.baidu.com", "https://www.biadu.com", "123.123.123.123", "se1y64k112h56gj525448h89sb0sg65w", "test", "http://gateway.enrichtec.com:9909/api/v1.1/pay/placeorder");//支付请求
            //隆发支付 -银联钱包(云闪付)["UNION_WALLET"]
            //  lfpay a = new lfpay();
            // a.Recharge("LFP201808250000", "UNION_WALLET", new Random().Next(1000, 9999).ToString(), System.DateTime.Now, 300, "http://api.hqcp89998.com/user/notify/afpay", "https//www.baidu.com", "https://www.biadu.com", "123.123.123.123", "7D7E768BB4CB7EBCE6E3B067A6351342", "test", "http://47.94.6.240:9003/api/pay");//支付请求
            // hhpay a = new hhpay();
            //  a.RechargeNotify("{\"app_id\":\"ba17a612136bbce26630fb2554058223\",\"method\":\"trade.wap.pay.return\",\"sign_type\":\"RSA2\",\"format\":\"JSON\",\"charset\":\"utf-8\",\"stamptime\":\"2019-11-18 15:57:59\",\"version\":\"1.0\",\"biz_content\":{\"out_trade_no\":\"12312313\",\"trade_no\":\"213133123231ewqeq1331\",\"total_amount\":\"200.00\",\"commission\":\"30\",\"balance\":\"333.00\",\"status\":\"交易成功\",\"success_time\":\"2019-11-18 15:57:59\",\"drawee\":\"小二\",\"source\":\"网银转账\",\"payee\":\"疯子\",\"payee_card\":\"622323232323233\",\"passback_params\":\"out_trade_no=201923233223232321;\"},\"sign\":\"eZmsyjdq69fDlCQVpLifpRUaYyXnCMBPh1h/uXDeklTWWu0mz/NQXRojnkCezyKfXNHGekjWlc2Jad9evP/6xrBDpFsxvXEz+HHaN5Q7hsF/Bd7loNXFR7iAL5yfZbgknARYX4qHj+i7cYX2QXA4WhmmjOmo5zGtVs0dreyxAnMbZuiDz2KU3Kby+cAxNqttc41Cy6pUcYWC6vDr+KwC3LIu/hlGGRxEiJU/9sztqhPH7G8f5jbDtXqYFQnXeyqAWlE8Gt+pxC4uGGLW1q0OxBXP1YjTCAb9TwEaE+muWiZwjCEQI2j8AgO406OsmKGUPY2EnIVCAyBnuYgNF6viNph+DGgwOerS5dAS/sCXdPo0D0Apx2euTxXM5KdYkSS4u+lD++It/cSjNGbtupQW/4B3KQtLWferizbuSnHKYIhjcSIAVaBSjxJLWZaHLSUsKu4Y6/RNivJHeXgTdhCUQZ7G6OmyG3v8jX0eXU+wD03xkBTffQku5fl5ZCOCzM7Uf/tTrmv3tWP2HCwHldWS6V3mr/BvAhSsne0p0rDuMHl6mlU303azyBxxKAA9KQzmvrLDIZIGVjONW74lNYYRBgFao9mjYeyeKVZVVKL1eA68JTGgaKlGIUVY7ceuR05XpvNNEb14B8jQjEd8EkOme4wTrIULfb1/eFPxUZpBYKc=\"}", "");//dui 的
            // a.Recharge("ba17a612136bbce26630fb2554058223", "2", new Random().Next(100000000, 999999999).ToString(), System.DateTime.Now, 300, "http://api.hqcp89998.com/user/notify/afpay", "https//www.baidu.com", "https://www.biadu.com", "123.123.123.123", "7D7E768BB4CB7EBCE6E3B067A6351342", "test", "https://test.huanhuanpay.com/api/payment/order.do");//支付请求
            //a.RechargeNotify("{\"app_id\":\"ba17a612136bbce26630fb2554058223\",\"sign\":\"eZmsyjdq69fDlCQVpLifpRUaYyXnCMBPh1h/uXDeklTWWu0mz/NQXRojnkCezyKfXNHGekjWlc2Jad9evP/6xrBDpFsxvXEz+HHaN5Q7hsF/Bd7loNXFR7iAL5yfZbgknARYX4qHj+i7cYX2QXA4WhmmjOmo5zGtVs0dreyxAnMbZuiDz2KU3Kby+cAxNqttc41Cy6pUcYWC6vDr+KwC3LIu/hlGGRxEiJU/9sztqhPH7G8f5jbDtXqYFQnXeyqAWlE8Gt+pxC4uGGLW1q0OxBXP1YjTCAb9TwEaE+muWiZwjCEQI2j8AgO406OsmKGUPY2EnIVCAyBnuYgNF6viNph+DGgwOerS5dAS/sCXdPo0D0Apx2euTxXM5KdYkSS4u+lD++It/cSjNGbtupQW/4B3KQtLWferizbuSnHKYIhjcSIAVaBSjxJLWZaHLSUsKu4Y6/RNivJHeXgTdhCUQZ7G6OmyG3v8jX0eXU+wD03xkBTffQku5fl5ZCOCzM7Uf/tTrmv3tWP2HCwHldWS6V3mr/BvAhSsne0p0rDuMHl6mlU303azyBxxKAA9KQzmvrLDIZIGVjONW74lNYYRBgFao9mjYeyeKVZVVKL1eA68JTGgaKlGIUVY7ceuR05XpvNNEb14B8jQjEd8EkOme4wTrIULfb1/eFPxUZpBYKc=\",\"method\":\"trade.wap.pay.return \",\"sign_type\":\"RSA2\",\"format\":\"JSON\",\"charset\":\"utf - 8\",\"stamptime\":\"2019 - 11 - 18 15:57:59\",\"version\":\"1.0\",\"biz_content\":\"{\"out_trade_no\":\"12312313\",\"trade_no\":\"213133123231ewqeq1331\",\"total_amount\":\"200.00\",\"commission\":\"30\",\"balance\":\"333.00\",\"status\":\"交易成功\",\"success_time\":\"2019-11-18 15:57:59\",\"drawee\":\"小二\",\"source\":\"网银转账\",\"payee\":\"疯子\",\"payee_card\":\"622323232323233\",\"passback_params\":\"out_trade_no=201923233223232321;\"}","");
            //a.RechargeNotify("{\"app_id\":\"ba17a612136bbce26630fb2554058223\",\"method\":\"trade.wap.pay.return \",\"sign_type\":\"RSA2\",\"format\":\"JSON\",\"charset\":\"utf - 8\",\"stamptime\":\"2019 - 11 - 18 15:57:59\",\"version\":\"1.0\",\"out_trade_no\":\"12312313\",\"trade_no\":\"213133123231ewqeq1331\",\"total_amount\":\"200.00\",\"commission\":\"30\",\"balance\":\"333.00\",\"status\":\"交易成功\",\"success_time\":\"2019-11-18 15:57:59\",\"drawee\":\"小二\",\"source\":\"网银转账\",\"payee\":\"疯子\",\"payee_card\":\"622323232323233\",\"passback_params\":\"out_trade_no=201923233223232321;, \"sign\":\"eZmsyjdq69fDlCQVpLifpRUaYyXnCMBPh1h/uXDeklTWWu0mz/NQXRojnkCezyKfXNHGekjWlc2Jad9evP/6xrBDpFsxvXEz+HHaN5Q7hsF/Bd7loNXFR7iAL5yfZbgknARYX4qHj+i7cYX2QXA4WhmmjOmo5zGtVs0dreyxAnMbZuiDz2KU3Kby+cAxNqttc41Cy6pUcYWC6vDr+KwC3LIu/hlGGRxEiJU/9sztqhPH7G8f5jbDtXqYFQnXeyqAWlE8Gt+pxC4uGGLW1q0OxBXP1YjTCAb9TwEaE+muWiZwjCEQI2j8AgO406OsmKGUPY2EnIVCAyBnuYgNF6viNph+DGgwOerS5dAS/sCXdPo0D0Apx2euTxXM5KdYkSS4u+lD++It/cSjNGbtupQW/4B3KQtLWferizbuSnHKYIhjcSIAVaBSjxJLWZaHLSUsKu4Y6/RNivJHeXgTdhCUQZ7G6OmyG3v8jX0eXU+wD03xkBTffQku5fl5ZCOCzM7Uf/tTrmv3tWP2HCwHldWS6V3mr/BvAhSsne0p0rDuMHl6mlU303azyBxxKAA9KQzmvrLDIZIGVjONW74lNYYRBgFao9mjYeyeKVZVVKL1eA68JTGgaKlGIUVY7ceuR05XpvNNEb14B8jQjEd8EkOme4wTrIULfb1/eFPxUZpBYKc=\"}","");
             cbbpay a = new cbbpay();
            a.Recharge("210284", "2", new Random().Next(100000000, 999999999).ToString(), System.DateTime.Now, 1000, "http://api.hqcp1238.com/user/notify/afpay", "https//www.baidu.com", "https://www.biadu.com", "123.123.123.123", "PY3N11aUvAnoqtHfubDxTmgOtWD9kKUm", "test", "https://cbb.168cbb.com/order/service_order");//支付请求
            // lzpay a = new lzpay();
            // a.Recharge("10129", "933", new Random().Next(100000000, 999999999).ToString(), System.DateTime.Now, 100, "http://api.hqcp1238.com/user/notify/afpay", "http://api.coauahdgsd.net/Pay_Index.html", "http://api.coauahdgsd.net/Pay_Index.html", "123.123.123.123", "05eg1l5xg3o91rmlo33q0t44tamy3f74", "test", "http://api.coauahdgsd.net/Pay_Index.html");//支付请求
           //thbpay a = new thbpay();//微信902   支付宝904 
            //a.RechargeNotify("user_id=1180&trade_no=98964832484536640659&out_trade_no=100526&transaction_money=50000&datetime=2019-11-21+21%3a48%3a51&returncode=00&attach=&sign=0CF2E437481A9232A099CDA2609E44CE","683fa0a81e7a229e93be393ca3a68ff5");
           // a.Recharge("1180", "902", new Random().Next(100000000, 999999999).ToString(), System.DateTime.Now, 500, "http://api.hqcp1238.com/user/notify/afpay", "https://www.baidu.com", "https://www.baidu.com", "123.123.123.123", "683fa0a81e7a229e93be393ca3a68ff5", "test", "http://pay.tonghuibaoabc.com/index/pay/gateway");//支付请求
           // jjpay a = new jjpay();
            //Request.UserHostAddress
           // a.RechargeNotify("code=0000&currency=CNY&mchntId=100006&mchntOrderId=104952&message=%e8%af%b7%e6%b1%82%e5%a4%84%e7%90%86%e6%88%90%e5%8a%9f&orderId=06019112117115162809&remarks=Games&signMethod=MD5&signature=E460A5F21E19B9496D96DE9FED85BFFB&success=true&txnAmt=30000", "12440d569a7f32f9b9b641454800fff7");
            //a.Recharge("100006", "2", new Random().Next(100000000, 999999999).ToString(), System.DateTime.Now, 300, "http://api.hqcp1238.com/user/notify/afpay", "https://www.baidu.com", "https://www.baidu.com", "192.168.1.1", "12440d569a7f32f9b9b641454800fff7", "test", "http://47.91.135.157/jinji/directpay/qrcode");//支付请求
        }
    }
}