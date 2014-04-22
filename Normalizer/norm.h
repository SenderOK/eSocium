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



	/* ����� ������������ ������ */
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
		/// �������� ���������� 2 ������. 0 - ��� ����������, -1 - ��������� ������ ������,
		/// 1 - ��������� ������ ������. 2 - ������, ����� ������������ ���� �� �����.
		signed char compare(const connectType &a, const connectType &b);
	};

	/* ����� ������������� */
	class Norm
	{
	public:
		Norm();
		~Norm();

		/// ������� ���� � ���-�������� ����.
		/** ��������� ���� � �����. ���������� true, ���� ���������� ������ �������,
		����� false. */
		bool openHashTable(string filename);

		/// ������� ���� �� ������� ������.
		/** ��������� ���� � �����. ���������� true, ���� ���������� ������ �������,
		����� false. */
		bool openLinks(string filename);

		/// �������� ��������� ��������� ������� ����.
		/** ��������� ����� - ������, ��������� �� ������� ���� � ������. ����� ���������
		����, ������� �������� ������ ����� � ���� ����� �� ����, ���� ������ ���������,
		���� ������� ���. */
		VecUint getLemmas(string word);

		/// �������� ��������� ����, �� ������� ����� ������� �� ������� � ������.
		/** ��������� ����� ����� � ��� ������. ���������� ��������� ����,
		�� ������� ����� ������� �� ������ �� ������� ��������� �����. */
		long getLinks(long lemma, int type, char dir);

		long walkLemma(long lemma, const Configuration *c);

		/// ������� ���� � ������� ���� (���������� �� xml-������� ��������)
		/** ��������� ���� � �����. ���������� true, ���� ���������� ������ �������,
		����� false. */
		bool importForms(string filename);

		/// ��������� ���-������� (� ��� �������, ������� ����������� �-���� openHashTable)
		/** ��������� ���� � �����. ���������� true, ���� ���������� ������ �������,
		����� false. */
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