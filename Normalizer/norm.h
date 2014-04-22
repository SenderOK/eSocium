#include <string>
#include <vector>

#define HASH_RANGE 77003
#define HASH_BASE 53
#define MAX_LINK_ARRAY_SIZE 100000
#define MAX_LINK_TYPES 30

namespace UnmanagedNorm {

	using namespace std;

	typedef vector<unsigned int> VecUint;

	struct element {
		element(string w) {
			word = w;
			lemmas.clear();
		}

		string word;
		vector<unsigned int> lemmas;
	};

	struct connect {
		connect(unsigned int f, unsigned int t, unsigned char tp, unsigned char d) : from(f), to(t), type(tp), dir(d) {};

		unsigned int from, to;
		unsigned char type;
		unsigned char dir; // 'f' or 'b'
	};

	struct lemma {
		unsigned int id;
		string nameType, posType;
	};



	/* Класс конфигурации связей */
	struct connectType {
		connectType(unsigned char t, unsigned char d) : type(t), dir(d) {};
		unsigned char type, dir;
	};

	class Configuration
	{
	public:
		Configuration();
		~Configuration();

		bool setConf(vector<connectType> c);
		vector<connectType> sortedConnections;
	private:
		/// Сравнить приоритеты 2 связей. 0 - они несравнимы, -1 - приоритет первой больше,
		/// 1 - приоритет второй больше. 2 - ошибка, такой конфигурации быть не может.
		signed char compare(const connectType &a, const connectType &b);
	};

	/* Класс нормализатора */
	class Norm
	{
	public:
		Norm();
		~Norm();

		/// Считать файл с хеш-таблицей форм.
		/** Принимает путь к файлу. Возвращает true, если считывание прошло успешно,
		иначе false. */
		bool openHashTable(string filename);

		/// Считать файл со списком ссылок.
		/** Принимает путь к файлу. Возвращает true, если считывание прошло успешно,
		иначе false. */
		bool openLinks(string filename);

		/// Получить множество возможных номеров лемм.
		/** Принимает слово - строку, состоящую из русских букв и дефиса. Выдаёт множество
		лемм, которые содержат данное слово в виде одной из форм, либо пустое множество,
		если таковых нет. */
		VecUint getLemmas(string word);

		/// Получить множество лемм, по которым можно перейти по ссылкам с данной.
		/** Принимает номер леммы и тип ссылки. Возвращает множество лемм,
		по которым можно перейти из данной по ссылкам указанных типов. */
		long getLinks(long lemma, int type, char dir);

		long walkLemma(long lemma, const Configuration *c);

		/// Считать файл с формами лемм (полученный из xml-словаря скриптом)
		/** Принимает путь к файлу. Возвращает true, если считывание прошло успешно,
		иначе false. */
		bool importForms(string filename);

		/// Сохранить хеш-таблицу (в том формате, который считывается ф-цией openHashTable)
		/** Принимает путь к файлу. Возвращает true, если считывание прошло успешно,
		иначе false. */
		bool saveHashTable(string filename);

		bool saveLemmasAttrTable(string filename);
		bool openLemmasAttrTable(string filename);

		string getLemmaNameType(unsigned int id);
		string getLemmaPosType(unsigned int id);

		void setYoRule(bool rule);
	private:
		vector<element> table[HASH_RANGE];
		vector<connect> links[MAX_LINK_ARRAY_SIZE];
		vector<lemma> lemmasArr[MAX_LINK_ARRAY_SIZE];
		bool useYo;

		unsigned int char2number(const char& c);
		unsigned long getHash(const char *s);
		string tolowercase(string s);
		string tolowercaseDYo(string s);
	};

}