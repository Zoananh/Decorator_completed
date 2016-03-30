using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Decorator
{
    class Translater : DecoratorStream
    {
        public Translater() { }
        public Translater(IStream stream) : base(stream)
        {

        }
        public override TextForFile Read(TextForFile textFile)
        {
            base.Read(textFile);
            if (!textFile._isMultyLine)
                textFile.ToMultyLine();

            Translate(textFile);
            return textFile;
        }

        public override void Wrire(TextForFile textFile)
        {
            if (!textFile._isMultyLine)
                textFile.ToMultyLine();

            Translate(textFile);
            base.Wrire(textFile);
        }

        //private void Translate(TextForFile textFile)
        //{
        //    throw new NotImplementedException();
        //}

        public void Translate(TextForFile textFile)
        {
            for (int i = 0; i < textFile._txtFile.Length; i++)
            {
                WebRequest request = WebRequest.Create("https://translate.yandex.net/api/v1.5/tr/translate?"
                + "key=trnsl.1.1.20160327T153219Z.ad960363e3593116.bce4bfab39e711d063f4477b94cb12d4187ef225"
                + "&text=" + textFile._txtFile[i]
                + "&lang=en"
                + "&[format=plain]"
                + "&[options=1]");
                WebResponse response = request.GetResponse();

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    string resXml = sr.ReadToEnd();
                    XmlDocument result = new XmlDocument();
                    result.LoadXml(resXml);
                    XmlNodeList textNodes = result.GetElementsByTagName("text");
                    textFile._txtFile[i] = textNodes[0].InnerText;
                   
                }
            }
        }
    }
}
