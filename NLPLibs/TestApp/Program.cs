using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Collections;
using TextNormalizer;
using TextTokenizer;
using TextTagger;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Привет мир:");
            List<Form> formsList = Normalizer.normalize("Привет");
            foreach (Form f in formsList)
            {
                Console.WriteLine(f.word + " " + f.parentLemma.id.ToString());
                for (int i = 0; i < 96; ++i)
                {
                    Console.Write(Convert.ToInt32(f.tags.getById(i)));
                }
                Console.WriteLine();
            }
            formsList = Normalizer.normalize("мир");
            foreach (Form f in formsList)
            {
                Console.WriteLine(f.word + " " + f.parentLemma.id.ToString());
                for (int i = 0; i < 96; ++i)
                {
                    Console.Write(Convert.ToInt32(f.tags.getById(i)));
                }
                Console.WriteLine();
            }
            Console.WriteLine("Абрракадабррра:");
            formsList = Normalizer.normalize("невесёлый");
            foreach (Form f in formsList)
            {
                Console.WriteLine(f.word + " " + f.parentLemma.id.ToString());
                for (int i = 0; i < 96; ++i)
                {
                    Console.Write(Convert.ToInt32(f.tags.getById(i)));
                }
                Console.WriteLine();
            }
            
            Console.WriteLine("Привет, мир! Привет, мир. Привет, мир? Привет, мир...");
            string[] s = SentenceTokenizer.tokenize("Привет, мир! Привет, мир. Привет, мир? Привет, мир...");
            foreach (string w in s)
                Console.Write(w + " | ");
            Console.Write("\n");

            Console.WriteLine("Привет, мир!");
            s = WordTokenizer.tokenize("Привет, мир!");
            foreach (string w in s)
                Console.Write(w + " | ");
            Console.Write("\n");

            Console.WriteLine("Привет, мир");
            s = WordPunctuationTokenizer.tokenize("Привет, мир");
            foreach (string w in s)
                Console.Write(w + " | ");
            Console.Write("\n");

            List<int> lList = new List<int>() {-3, -5, -4, -11, -21, -22, -23, -24, -12, -15, -16, -17, -18, -19, -20};
            LinkConfiguration lConf = new LinkConfiguration();
            Console.WriteLine(lConf.setConf(lList));
            Console.WriteLine();
            
            string lemmas = "300965 253034 184140 192977 317456 388694 55560 72149 67984 363322 241240 128224 331874 387317 114367 267449 49884 304836 201612 366948 331060 257960 304488 388747 162804 239689 82261 102993 210594 162570 198798 386581 225603 363287 164802 267948 160893 372461 144838 313624 225606 122879 37409 51 192524 143259 85153 363259 346944 164805 51532 90480 161428 229442 294553 145471 243063 98043 37265 144216 290502 261868 205175 363181 99331 42110 360028 201517 380220 342451 128192 111250 267952 136015 105492 84214 51543 290501 141901 297595 331871 346835 355910 340544 99336 213368 286941 26115 28565 96208 304343 373369 85032 188332 311151 81427 210575 178492 92342 208750 178876 333375 208757 191429 330869 221038 25314 18836 121986 348839 90581 264705 307396 106591 192793 82512 261565 286921 316534 179394 179395 318660 350851 97285 321441 52157 36985 377004 164485 353630 49410 257727 348430 266518 74164 303641 376585 269377 85407 226226 55041 206544 53369 162204 37235 304369 166264 277255 77569 119487 201616 265490 125386 352818 203943 268625 328580 206619 50732 207919 121146 188868 201663 118144 158822 304808 347395 316379 191396 106583 165750 345741 330875 81561 73610 92991 196227 136923 221681 345744 304408 278686 328116 141203 184810 321532 150212 39403 101455 202081 201665 310004 371201 366103 7668 363127 42156 279545 304424";
            foreach (string lemma in lemmas.Split())
            {
                Console.WriteLine(Normalizer.aggregate(Normalizer.getLemmaById(Convert.ToInt32(lemma)), lConf).id);
            }

            string str = "Далеко за словесными горами в стране гласных и согласных живут рыбные тексты";
            List<Form> forms = Tagger.tagSentence(str);
            foreach (Form form in forms)
            {
                Console.WriteLine(form.word + " : " + form.tags.tagsToString());
            }

            str = "Даже всемогущая пунктуация не имеет власти над рыбными текстами, ведущими безорфографичный образ жизни.";
            forms = Tagger.tagSentence(str);
            foreach (Form form in forms)
            {
                Console.WriteLine(form.word +  " : " + form.tags.tagsToString());
            }

            str = "Однажды одна маленькая строчка рыбного текста по имени Lorem ipsum решила выйти в большой мир грамматики";
            forms = Tagger.tagSentence(str);
            foreach (Form form in forms)
            {
                Console.WriteLine(form.word + " : " + form.tags.tagsToString());
                if (!form.isUnknown())
                {
                    Console.WriteLine(form.parentLemma.id);
                }
                else
                {
                        Console.WriteLine("123");
                }
            }

            str = "Маша съела 3 яблока";
            forms = Tagger.tagSentence(str);
            foreach (Form form in forms)
            {
                Console.WriteLine(form.word + " : " + form.tags.tagsToString());
            }
            
            Console.ReadKey();
        }
    }
}
