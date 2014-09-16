using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextNormalizer;
using TextTokenizer;

namespace TextTagger
{
    /// <summary>
    /// Class contains highlevel text tagging methods.
    /// </summary>
    public static class Tagger
    {
        private static Dictionary<string, int> _modelTagPairProb;

        static Tagger()
        {
            _modelTagPairProb = new Dictionary<string, int>();
            string[] modelStrs = Model.mmodel.Split();
            FormTagCollection prev, next;

            for (int i = 0; i < modelStrs.Length; i += 3)
            {
                prev = new FormTagCollection();
                foreach (string tag in modelStrs[i].Split(','))
                {
                    prev.setByReference(true, tag);
                }
                next = new FormTagCollection();
                foreach (string tag in modelStrs[i + 1].Split(','))
                {
                    next.setByReference(true, tag);
                }

                _modelTagPairProb[prev.tagsToString() +  next.tagsToString()] = Convert.ToInt32(modelStrs[i + 2]);
            }
        }

        /// <summary>
        /// Represent a sentence as list of forms. 
        /// </summary>
        /// <param name="sentence">Sentence as string.</param>
        /// <returns>List of forms</returns>
        public static List<Form> tagSentence(string sentence)
        {
            string[] wordsStrs = WordPunctuationTokenizer.tokenize(sentence);
            List<FormTagCollection> wordsTags = new List<FormTagCollection>();
            List<Form> wordsForms = new List<Form>();

            foreach (string wordStr in wordsStrs)
            {
                FormTagCollection curTags = new FormTagCollection();
                if (wordStr[0] <= 'z' && wordStr[0] >= 'a')
                {
                    curTags.setByReference(true, "LATN");
                    Form form = new Form(wordStr, new string[1] {"LATN"}, new Lemma(wordStr, new string[0], -1));
                    wordsTags.Add(curTags);
                    wordsForms.Add(form);
                } else if (Char.IsDigit(wordStr[0]))
                {
                    curTags.setByReference(true, "NUMB");
                    Form form = new Form(wordStr, new string[1] {"NUMB"}, new Lemma(wordStr, new string[0], -1));
                    wordsTags.Add(curTags);
                    wordsForms.Add(form);
                } else if (Char.IsPunctuation(wordStr[0]))
                {
                    curTags.setByReference(true, "PNCT");

                    Form form = new Form(wordStr, new string[1] {"PNCT"}, new Lemma(wordStr, new string[0], -1));
                    wordsTags.Add(curTags);
                    wordsForms.Add(form);
                }
                else
                {
                    List<Form> normalizerRes = Normalizer.normalize(wordStr);
                    if (normalizerRes.Count == 0)
                    {
                        curTags.setByReference(true, "UNKN");
                        Form form = new Form(wordStr, new string[1] {"UNKN"}, new Lemma(wordStr, new string[0], -1));
                        wordsTags.Add(curTags);
                        wordsForms.Add(form);
                    }
                    else
                    {
                        FormTagCollection last = new FormTagCollection();
                        if (wordsTags.Count > 0)
                        {
                            last = wordsTags.Last();
                        }
                        else
                        {
                            last.setByReference(true, "NONE");
                        }
                        int maxProbIdx = 0;
                        int maxProb;
                        _modelTagPairProb.TryGetValue(last.tagsToString() + normalizerRes[maxProbIdx].tags.tagsToString(), out maxProb);

                        for (int i = 1; i < normalizerRes.Count; ++i)
                        {
                            int curProb;
                            _modelTagPairProb.TryGetValue(last.tagsToString() + normalizerRes[i].tags.tagsToString(), out curProb);
                            if (curProb > maxProb)
                            {
                                maxProbIdx = i;
                                maxProb = curProb;
                            }
                        }
                        wordsTags.Add(normalizerRes[maxProbIdx].tags);
                        wordsForms.Add(normalizerRes[maxProbIdx]);
                    }
                }
            }
            List<Form> result = new List<Form>();
            foreach (Form f in wordsForms)
            {
                if (!f.isPunct())
                {
                    result.Add(f);
                }
            }
            return result;
        }
    }
}
