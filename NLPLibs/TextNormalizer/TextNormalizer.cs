using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.IO.Compression;

namespace TextNormalizer
{
    interface ITagCollection
    {
        bool getById(int id);
        bool getByReference(string reference);
        void setById(bool value, int id);
        void setByReference(bool value, string reference);
    }

    /// <summary>
    /// Link class
    /// </summary>
    public class LemmaLink
    {
        /// <summary>
        /// Destination lemma of link.
        /// </summary>
        public Lemma to;

        /// <summary>
        /// Departure of link.
        /// </summary>
        public Lemma from;

        /// <summary>
        /// Type of link.
        /// </summary>
        public int type;

        /// <summary>
        /// Create a link.
        /// </summary>
        /// <param name="to">Link destination</param>
        /// <param name="from">Link departure point</param>
        /// <param name="type">Link type</param>
        public LemmaLink(Lemma to, Lemma from, int type)
        {
            this.to = to;
            this.from = from;
            this.type = type;
        }
    }

    internal static class TagIndexes
    {
        public static Dictionary<string, int> allTagsIdxs = new Dictionary<string, int>
        {
            {"1per", 0}, {"2per", 1}, {"3per", 2}, {"ADJF", 3}, {"ADJS", 4}, {"ADVB", 5}, 
            {"ANim", 6}, {"Abbr", 7}, {"Af-p", 8}, {"Anph", 9}, {"Anum", 10}, {"Apro", 11}, 
            {"Arch", 12}, {"COMP", 13}, {"CONJ", 14}, {"Cmp2", 15}, {"Coll", 16}, {"Coun", 17}, 
            {"Dist", 18}, {"Dmns", 19}, {"Erro", 20}, {"Fimp", 21}, {"Fixd", 22}, {"GNdr", 23}, 
            {"GRND", 24}, {"Geox", 25}, {"INFN", 26}, {"INTJ", 27}, {"Impe", 28}, {"Infr", 29}, 
            {"Inmx", 30}, {"Litr", 31}, {"Ms-f", 32}, {"NOUN", 33}, {"NPRO", 34}, {"NUMR", 35}, 
            {"Name", 36}, {"Orgn", 37}, {"PRCL", 38}, {"PRED", 39}, {"PREP", 40}, {"PRTF", 41}, 
            {"PRTS", 42}, {"Patr", 43}, {"Pltm", 44}, {"Poss", 45}, {"Prdx", 46}, {"Prnt", 47}, 
            {"Qual", 48}, {"Ques", 49}, {"Sgtm", 50}, {"Slng", 51}, {"Subx", 52}, {"Supr", 53}, 
            {"Surn", 54}, {"V-be", 55}, {"V-bi", 56}, {"V-ej", 57}, {"V-en", 58}, {"V-ey", 59}, 
            {"V-ie", 60}, {"V-oy", 61}, {"V-sh", 62}, {"VERB", 63}, {"Vpre", 64}, {"ablt", 65}, 
            {"accs", 66}, {"actv", 67}, {"anim", 68}, {"datv", 69}, {"excl", 70}, {"femn", 71}, 
            {"futr", 72}, {"gen1", 73}, {"gen2", 74}, {"gent", 75}, {"impf", 76}, {"impr", 77}, 
            {"inan", 78}, {"incl", 79}, {"indc", 80}, {"intr", 81}, {"loc1", 82}, {"loc2", 83}, 
            {"loct", 84}, {"masc", 85}, {"neut", 86}, {"nomn", 87}, {"past", 88}, {"perf", 89}, 
            {"plur", 90}, {"pres", 91}, {"pssv", 92}, {"sing", 93}, {"tran", 94}, {"voct", 95}, 
            {"NONE", 96}, {"PNCT", 97}, {"UNKN", 98}, {"NUMB", 99}, {"LATN", 100}, {"ROMN", 101}
        };

        public static Dictionary<string, int> lemmasTagsIdxs = new Dictionary<string, int>
        {
            {"1per", 0}, {"2per", 1}, {"3per", 2}, {"ADJF", 3}, {"ADJS", 4}, {"ADVB", 5}, {"ANim", 6}, 
            {"Abbr", 7}, {"Anph", 8}, {"Anum", 9}, {"Apro", 10}, {"Arch", 11}, {"COMP", 12}, 
            {"CONJ", 13}, {"Coll", 14}, {"Dist", 15}, {"Dmns", 16}, {"Erro", 17}, {"Fixd", 18}, 
            {"GNdr", 19}, {"GRND", 20}, {"Geox", 21}, {"INFN", 22}, {"INTJ", 23}, {"Impe", 24}, 
            {"Infr", 25}, {"Inmx", 26}, {"Ms-f", 27}, {"NOUN", 28}, {"NPRO", 29}, {"NUMR", 30}, 
            {"Name", 31}, {"Orgn", 32}, {"PRCL", 33}, {"PRED", 34}, {"PREP", 35}, {"PRTF", 36}, 
            {"PRTS", 37}, {"Patr", 38}, {"Pltm", 39}, {"Poss", 40}, {"Prdx", 41}, {"Prnt", 42}, 
            {"Qual", 43}, {"Ques", 44}, {"Sgtm", 45}, {"Slng", 46}, {"Subx", 47}, {"Supr", 48}, 
            {"Surn", 49}, {"VERB", 50}, {"actv", 51}, {"anim", 52}, {"femn", 53}, {"impf", 54}, 
            {"inan", 55}, {"intr", 56}, {"masc", 57}, {"neut", 58}, {"past", 59}, {"perf", 60}, 
            {"pres", 61}, {"pssv", 62}, {"tran", 63}
        };
    }

    /// <summary>
    /// Form tag class
    /// </summary>
    public class FormTag
    {
        private string _reference;
        /// <summary>
        /// Text reference of tag.
        /// </summary>
        public string reference
        {
            get { return this._reference; }
            set { this._id = TagIndexes.allTagsIdxs[value]; this._reference = value; }
        }

        private int _id;
        /// <summary>
        /// Id of tag.
        /// </summary>
        public int id
        {
            get { return this._id; }
            set { this._reference = TagIndexes.allTagsIdxs.First(kvp => kvp.Value.Equals(value)).Key; this._id = value; }
        }

        /// <summary>
        /// Create tag by text reference.
        /// </summary>
        /// <param name="reference">text reference</param>
        public FormTag(string reference)
        {
            this.reference = reference;
        }

        /// <summary>
        /// Create tag by id
        /// </summary>
        /// <param name="id">tag's id</param>
        public FormTag(int id)
        {
            this.id = id;
        }
    };

    /// <summary>
    /// Lemma tag class
    /// </summary>
    public class LemmaTag
    {
        private string _reference;
        /// <summary>
        /// Text reference of tag.
        /// </summary>
        public string reference
        {
            get { return this._reference; }
            set { this._id = TagIndexes.lemmasTagsIdxs[value]; this._reference = value; }
        }

        private int _id;
        /// <summary>
        /// Id of tag.
        /// </summary>
        public int id
        {
            get { return this._id; }
            set { this._reference = TagIndexes.lemmasTagsIdxs.First(kvp => kvp.Value.Equals(value)).Key; this._id = value; }
        }

        /// <summary>
        /// Create tag by text reference.
        /// </summary>
        /// <param name="reference">text reference</param>
        public LemmaTag(string reference)
        {
            this.reference = reference;
        }

        /// <summary>
        /// Create tag by id
        /// </summary>
        /// <param name="id">tag's id</param>
        public LemmaTag(int id)
        {
            this.id = id;
        }
    };

    /// <summary>
    /// Class of tag collection of lemma.
    /// </summary>
    public class LemmaTagCollection: ITagCollection
    {
        private BitArray _tags;

        /// <summary>
        /// Create empty tag collection.
        /// </summary>
        public LemmaTagCollection()
        {
            this._tags = new BitArray(64);
            this._tags.SetAll(false);
        }

        /// <summary>
        /// Get tag by its id.
        /// </summary>
        /// <param name="id">Id of tag.</param>
        public bool getById(int id)
        {
            return this._tags[id];
            
        }

        /// <summary>
        /// Get tag by its string representation.
        /// </summary>
        /// <param name="reference">String representation of tag.</param>
        public bool getByReference(string reference)
        {
            LemmaTag nT = new LemmaTag(reference);
            return this._tags[nT.id];
        }

        /// <summary>
        /// Set or unset tag by its id.
        /// </summary>
        /// <param name="value">Set (true) or unset (false) tag.</param>
        /// <param name="id">Id of tag.</param>
        public void setById(bool value, int id)
        {
            this._tags[id] = value;
        }

        /// <summary>
        /// Set or unset tag by its string representation.
        /// </summary>
        /// <param name="value">Set (true) or unset (false) tag.</param>
        /// <param name="reference">String representation of tag.</param>
        public void setByReference(bool value, string reference)
        {
            LemmaTag nT = new LemmaTag(reference);
            this._tags[nT.id] = value;
        }
    };

    /// <summary>
    /// Class of form tag collection (form tags include lemma tags)
    /// </summary>
    public class FormTagCollection: ITagCollection
    {
        private BitArray _tags;

        /// <summary>
        /// Create empty tag collection.
        /// </summary>
        public FormTagCollection()
        {
            _tags = new BitArray(102);
            _tags.SetAll(false);
        }

        /// <summary>
        /// Get tag by its id.
        /// </summary>
        /// <param name="id">Id of tag.</param>
        public bool getById(int id)
        {
            return this._tags[id];
        }

        /// <summary>
        /// Get tag by its string representation.
        /// </summary>
        /// <param name="reference">String representation of tag.</param>
        public bool getByReference(string reference)
        {
            FormTag nT = new FormTag(reference);
            return this._tags[nT.id];
        }

        /// <summary>
        /// Set or unset tag by its id.
        /// </summary>
        /// <param name="value">Set (true) or unset (false) tag.</param>
        /// <param name="id">Id of tag.</param>
        public void setById(bool value, int id)
        {
            this._tags[id] = value;
        }

        /// <summary>
        /// Set or unset tag by its string representation.
        /// </summary>
        /// <param name="value">Set (true) or unset (false) tag.</param>
        /// <param name="reference">String representation of tag.</param>
        public void setByReference(bool value, string reference)
        {
            FormTag nT = new FormTag(reference);
            this._tags[nT.id] = value;
        }

        /// <summary>
        /// Create string representation of tags.
        /// </summary>
        /// <returns>String representation</returns>
        public string tagsToString()
        {
            string s = "";
            for (int i = 0; i < 102; ++i)
            {
                s += (this._tags[i] ? '1' : '0');
            }
            return s;
        }
    };

    /// <summary>
    /// Lemma class
    /// </summary>
    public class Lemma
    {
        /// <summary>
        /// List of lemma's forms.
        /// </summary>
        public List<Form> forms;

        /// <summary>
        /// Lemma's tags.
        /// </summary>
        public LemmaTagCollection tags;

        /// <summary>
        /// Links from/to lemma.
        /// </summary>
        public Dictionary<int, List<LemmaLink>> links;
        
        /// <summary>
        /// First form of lemma
        /// </summary>
        public string firstForm;

        /// <summary>
        /// Lemma's id.
        /// </summary>
        public int id;

        /// <summary>
        /// Create lemma.
        /// </summary>
        /// <param name="lemmaForm">First form of lemma</param>
        /// <param name="lemmaTags">Tags as a string (as in opencorpora's dictionary)</param>
        /// <param name="id">Lemma's id</param>
        public Lemma(string lemmaForm, string[] lemmaTags, int id)
        {
            this.firstForm = lemmaForm;
            this.forms = new List<Form>();
            this.links = new Dictionary<int, List<LemmaLink>>();
            this.id = id;
            tags = new LemmaTagCollection();
            foreach (string tag in lemmaTags)
            {
                tags.setByReference(true, tag);
            }
        }

        internal void addForm(Form curForm)
        {
            forms.Add(curForm);
        }

        internal void addLink(LemmaLink link)
        {
            if (!links.ContainsKey(link.type))
            {
                links[link.type] = new List<LemmaLink>();
            }
            links[link.type].Add(link);
        }
    };

    /// <summary>
    /// Form class
    /// </summary>
    public class Form
    {
        /// <summary>
        /// Lemma of form.
        /// </summary>
        public Lemma parentLemma;

        /// <summary>
        /// Tags of form (include lemma's tags)
        /// </summary>
        public FormTagCollection tags;

        /// <summary>
        /// Word that is described by form. 
        /// </summary>
        public string word;

        /// <summary>
        /// Create form.
        /// </summary>
        /// <param name="word">Word described by form</param>
        /// <param name="formTags">Tags in dictionary format</param>
        /// <param name="parentLemma">Parent lemma</param>
        public Form(string word, string[] formTags, Lemma parentLemma)
        {
            this.word = word;
            this.parentLemma = parentLemma;
            tags = new FormTagCollection();
            foreach (string tag in formTags)
            {
                tags.setByReference(true, tag);
            }
        }

        /// <summary>
        /// Is form a number? (like "2", "123")
        /// </summary>
        /// <returns>Boolean value</returns>
        public bool isNumber()
        {
            return this.tags.getByReference("NUMB");
        }

        /// <summary>
        /// True when form is not in dictionary.
        /// </summary>
        /// <returns>Boolean value</returns>
        public bool isUnknown()
        {
            return this.tags.getByReference("UNKN");
        }

        /// <summary>
        /// Is form a punctuation sign?
        /// </summary>
        /// <returns>Boolean value</returns>
        public bool isPunct()
        {
            return this.tags.getByReference("PNCT");
        }
    };

    /// <summary>
    /// Configuration of dictionary links.
    /// </summary>
    public class LinkConfiguration
    {
        private List<int> _lTypes;

        /// <summary>
        /// Link types. Their absolute values determines a dictionary type of link and signs determines a direction: + - forward, - - back
        /// </summary>
        public List<int> lTypes { get { return this._lTypes; } }

        private int compareLinkType(int a, int b)
        {
            if (a == 11 && (b == -3 || b == -4 || b == -5 || b == -6))
            {
                return -1;
            }
            if (b == 11 && (a == -3 || a == -4 || a == -5 || a == -6))
            {
                return 1;
            }

            if (a == -11 && (b == -3 || b == -4 || b == -5 || b == -6))
            {
                return 1;
            }
            if (b == -11 && (a == -3 || a == -4 || a == -5 || a == -6))
            {
                return -1;
            }

            if ((a == -21 || a == -22) && (b == -2 || b == -12 || (b <= -15 && b >= -20)))
            {
                return 1;
            }
            if ((b == -21 || b == -22) && (a == -2 || a == -12 || (a <= -15 && a >= -20)))
            {
                return -1;
            }

            // проверки на совместимость
            if ((a == -23 || a == -24) && (b == -7 || b == -8 || b == -9 || b == 10))
            {
                return -2;
            }
            if ((b == -23 || b == -24) && (a == -7 || a == -8 || a == -9 || a == 10))
            {
                return -2;
            }

            if ((a >= 0 && a <= 9) || a == -10 || a >= 12 || a <= -25)
            {
                return -2;
            }
            if ((b >= 0 && b <= 9) || b == -10 || b >= 12 || b <= -25)
            {
                return -2;
            }


            return 0;
        }

        private bool reSort()
        {
            bool changes = true;
            while (changes)
            {
                changes = false;
                for (int i = 0; i < _lTypes.Count; ++i)
                {
                    for (int j = i + 1; j < _lTypes.Count; ++j)
                    {
                        int compRes = compareLinkType(_lTypes[i], _lTypes[j]);
                        if (compRes == -2)
                        {
                            return false;
                        }
                        if (compRes == -1)
                        {
                            int tmp = _lTypes[i];
                            _lTypes[i] = _lTypes[j];
                            _lTypes[j] = tmp;
                            changes = true;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Insert link by type in configuration
        /// </summary>
        /// <param name="t">Link type. Absolute value determines a dictionary type of link and sign determines a direction: + - forward, - - back</param>
        public bool addLinkType(int t)
        {
            if (!_lTypes.Contains(t))
            {
                _lTypes.Add(t);
            }

            return reSort();
        }

        /// <summary>
        /// Delete link by type from configuration
        /// </summary>
        /// <param name="t">Link type. Absolute value determines a dictionary type of link and sign determines a direction: + - forward, - - back</param>
        public void removeLinkType(int t)
        {
            _lTypes.Remove(t);
        }

        /// <summary>
        /// Set link configuration.
        /// </summary>
        /// <param name="lT">List of link types. Absolute value determines a dictionary type of link and sign determines a direction: + - forward, - - back</param>
        /// <returns></returns>
        public bool setConf(List<int> lT)
        {
            _lTypes = lT;
            _lTypes = lTypes.Distinct().ToList();
            return reSort();
        }

        /// <summary>
        /// Create empty configuration
        /// </summary>
        public LinkConfiguration()
        {
            _lTypes = new List<int>();
        }
    };
    

    /// <summary>
    /// Class contains methods of normalize and aggregate separate words/lemmas by opencorpora's dictionary.
    /// </summary>
    public static class Normalizer
    {
        static private Dictionary<string, List<Form>> DictWord;
        static private Dictionary<int, Lemma> DictId;
        
        public static void Initalize() { }            
        static Normalizer()
        {
            DictWord = new Dictionary<string, List<Form>>();
            DictId = new Dictionary<int, Lemma>();

            // Read dictionary
            using (MemoryStream dictStr = new MemoryStream(Dict.dict_txt))
            using (GZipStream dictGzip = new GZipStream(dictStr, CompressionMode.Decompress))
            using (StreamReader dictRdr = new StreamReader(dictGzip, Encoding.UTF8))
            {
                int lemmaId;
                string lemmaForm;
                string formWord;
                string[] lemmaTags, formTags, allTags;

                while (dictRdr.Peek() >= 0)
                {
                    lemmaId = Convert.ToInt32(dictRdr.ReadLine());

                    string[] formParams = dictRdr.ReadLine().Trim().Split();

                    lemmaForm = formParams[0];
                    lemmaTags = formParams[1].Split(',');
                    Lemma curLemma = new Lemma(lemmaForm, lemmaTags, lemmaId);
                    DictId[lemmaId] = curLemma;

                    formWord = formParams[0];
                    formTags = (formParams.Length > 2 ? formParams[2].Split(',') : new string[0]);
                    allTags = formTags.Concat(lemmaTags).ToArray();
                    Form curForm = new Form(formWord, allTags, curLemma);
                    curLemma.addForm(curForm);

                    if (!DictWord.ContainsKey(formWord))
                        DictWord[formWord] = new List<Form>();
                    DictWord[formWord].Add(curForm);

                    string formRef = dictRdr.ReadLine().Trim();
                    while (formRef.Length > 0)
                    {
                        formParams = formRef.Split();
                        formWord = formParams[0];
                        formTags = (formParams.Length > 2 ? formParams[2].Split(',') : new string[0]);
                        allTags = formTags.Concat(lemmaTags).ToArray();
                        curForm = new Form(formWord, allTags, curLemma);
                        curLemma.addForm(curForm);

                        if (!DictWord.ContainsKey(formWord))
                            DictWord[formWord] = new List<Form>();
                        DictWord[formWord].Add(curForm);
                        
                        formRef = dictRdr.ReadLine().Trim();
                    }
                }
            }

            // Read links
            string[] linksRef = Dict.links.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            int from, to, type;
            for (int i = 0; i < linksRef.Length; i += 3)
            {
                from = Convert.ToInt32(linksRef[i]);
                to = Convert.ToInt32(linksRef[i + 1]);
                type = Convert.ToInt32(linksRef[i + 2]);
                LemmaLink curLink = new LemmaLink(DictId[to], DictId[from], type);
                DictId[from].addLink(curLink);
                curLink = new LemmaLink(DictId[from], DictId[to], -type);
                DictId[to].addLink(curLink);
            }
        }

        /// <summary>
        /// Normalize one word
        /// </summary>
        /// <param name="word">Word as string</param>
        /// <returns>List of possible forms</returns>
        public static List<Form> normalize(string word)
        {
            word = word.ToUpper();
            if (DictWord.ContainsKey(word))
            {
                return DictWord[word];
            }
            else
            {
                return new List<Form>();
            }
        }

        /// <summary>
        /// Aggregate lemma by links configuration.
        /// </summary>
        /// <param name="l">Lemma</param>
        /// <param name="conf">Links configuration</param>
        /// <returns></returns>
        public static Lemma aggregate(Lemma l, LinkConfiguration conf)
        {
            Lemma curLemma = l;
            Lemma nextLemma = l;
            while (nextLemma != null)
            {
                curLemma = nextLemma;
                nextLemma = null;
                foreach (int link in conf.lTypes)
                {
                    if (curLemma.links.ContainsKey(link))
                    {
                        nextLemma = curLemma.links[link][0].to;
                        break;
                    }
                }
            }
            return curLemma;
        }

        /// <summary>
        /// Get lemma by its ID in Opencorpora's dictionary.
        /// </summary>
        /// <param name="id">Lemma's id</param>
        /// <returns></returns>
        public static Lemma getLemmaById(int id)
        {
            if (DictId.ContainsKey(id))
            {
                return DictId[id];
            }
            return null;
        }
    }
}
