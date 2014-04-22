#include "norm.h"
#include <cstdlib>
#include <set>
#include <fstream>
#include <iostream>
#include <algorithm>

namespace UnmanagedNorm {

	Norm::Norm()
	{
		setlocale(LC_ALL, "ru_RU.CP1251");
		useYo = 1;
	}

	Norm::~Norm()
	{
		for (unsigned long i = 0; i < HASH_RANGE; ++i) {
			table[i].clear();
		}

		for (unsigned long i = 0; i < MAX_LINK_ARRAY_SIZE; ++i) {
			links[i].clear();
			lemmasArr[i].clear();
		}
	}

	string
		Norm::tolowercaseDYo(string s)
	{
			string::iterator it = s.begin();
			for (; it != s.end(); ++it) {
				if (*it >= 'А' && *it <= 'Я') {
					*it = 'а' + (*it - 'А');
				}
				else if (*it == 'Ё') {
					*it = 'е';
				}
				else if (*it == 'ё') {
					*it = 'е';
				}
			}
			return s;
		}

	string
		Norm::tolowercase(string s)
	{
			string::iterator it = s.begin();
			for (; it != s.end(); ++it) {
				if (*it >= 'А' && *it <= 'Я') {
					*it = 'а' + (*it - 'А');
				}
				else if (*it == 'Ё') {
					*it = 'ё';
				}
			}
			return s;
		}

	bool
		Norm::openHashTable(string filename)
	{
			setlocale(LC_ALL, "ru_RU.CP1251");

			ifstream file(filename.c_str());
			if (!file) { return false; }
			unsigned long hash_cur;
			unsigned int lemma_num;
			string form_cur, tmp;
			char *err;

			while (!file.eof()) {
				file >> tmp;
				if (file.eof()) break;
				hash_cur = strtol(tmp.c_str(), &err, 10);
				if (*err) {
					return false;
				}
				table[hash_cur].clear();

				file >> tmp; // :
				if (tmp != ":") {
					return false;
				}

				file >> tmp;
				while (tmp != "|") {
					form_cur = tmp;
					table[hash_cur].push_back(element(tolowercase(form_cur)));
					file >> tmp;
					do {
						lemma_num = strtol(tmp.c_str(), &err, 10);
						if (*err) {
							return false;
						}
						table[hash_cur].back().lemmas.push_back(lemma_num);
						file >> tmp;
					} while (tmp != "&");
					file >> tmp;
				}
			}
			return true;
		}

	bool
		Norm::importForms(string filename)
	{
			ifstream file(filename.c_str());
			if (!file) return false;

			string form_cur;
			lemma tmp;
			unsigned int id_cur;
			unsigned long hash;
			char *err;

			while (!file.eof()) {
				getline(file, form_cur);
				if (file.eof()) break;
				id_cur = strtol(form_cur.c_str(), &err, 10);
				if (*err) {
					file.close();
					return false;
				}
				tmp.id = id_cur;
				getline(file, tmp.nameType);
				getline(file, tmp.posType);
				lemmasArr[id_cur % MAX_LINK_ARRAY_SIZE].push_back(tmp);

				getline(file, form_cur);
				form_cur = tolowercase(form_cur);
				while (form_cur.length() > 0) {
					hash = getHash(tolowercaseDYo(form_cur).c_str());
					if (hash == HASH_RANGE) {
						file.close();
						return false;
					}
					vector<element>::iterator it;
					for (it = table[hash].begin(); it != table[hash].end(); ++it) {
						if (it->word == form_cur) {
							it->lemmas.push_back(id_cur);
							break;
						}
					}
					if (it == table[hash].end()) {
						table[hash].push_back(element(form_cur));
						table[hash].back().lemmas.push_back(id_cur);
					}

					getline(file, form_cur);
					form_cur = tolowercase(form_cur);
				}
			}
			file.close();
			return true;
		}

	bool
		Norm::openLinks(string filename)
	{
			ifstream file(filename.c_str());
			if (!file) return false;
			for (unsigned long i = 0; i < MAX_LINK_ARRAY_SIZE; ++i) {
				links[i].clear();
			}

			int from, to, type;
			while (!file.eof()) {
				file >> from >> to >> type;
				links[from % MAX_LINK_ARRAY_SIZE].push_back(connect(from, to, type, 'f'));
				links[to % MAX_LINK_ARRAY_SIZE].push_back(connect(to, from, type, 'b'));
			}
			file.close();
			return true;
		}

	bool
		Norm::openLemmasAttrTable(string filename)
	{
			ifstream file(filename.c_str());
			if (!file) return false;
			for (unsigned long i = 0; i < MAX_LINK_ARRAY_SIZE; ++i) {
				lemmasArr[i].clear();
			}

			lemma L;
			while (!file.eof()) {
				file >> L.id >> L.nameType >> L.posType;
				lemmasArr[L.id % MAX_LINK_ARRAY_SIZE].push_back(L);
			}
			file.close();
			return true;
		}

	bool
		Norm::saveLemmasAttrTable(string filename)
	{
			ofstream file(filename.c_str());
			if (!file) return false;

			for (unsigned int i = 0; i < MAX_LINK_ARRAY_SIZE; ++i) {
				for (vector<lemma>::iterator itr = lemmasArr[i].begin(); itr != lemmasArr[i].end(); ++itr) {
					file << itr->id << ' ' << itr->nameType << ' ' << itr->posType << ' ';
				}
			}
			file.close();
			return true;
		}


	long
		Norm::getLinks(long l, int type, char dir)
	{
			long result = -1;

			for (vector<connect>::iterator itr = links[l % MAX_LINK_ARRAY_SIZE].begin();
				itr != links[l % MAX_LINK_ARRAY_SIZE].end(); ++itr) {
				if (itr->from == l && itr->type == type && itr->dir == dir) {
					if (result != -1) {
						cout << "Warning: link " << type << dir << " from " << l << " not unique." << endl;
					}
					result = itr->to;
				}
			}

			return result;
		}

	long
		Norm::walkLemma(long l, const Configuration *c)
	{
			long curL = l, nextL = l;
			while (nextL != -1) {
				curL = nextL;
				nextL = -1;
				for (int i = 0; i < c->sortedConnections.size(); ++i) {
					nextL = getLinks(curL, c->sortedConnections[i].type, c->sortedConnections[i].dir);
					if (nextL != -1) {
						break;
					}
				}
			}
			return curL;
		}

	string
		Norm::getLemmaNameType(unsigned int id)
	{
			for (vector<lemma>::iterator itr = lemmasArr[id % MAX_LINK_ARRAY_SIZE].begin();
				itr != lemmasArr[id % MAX_LINK_ARRAY_SIZE].end(); ++itr) {
				if (itr->id == id) {
					return itr->nameType;
				}
			}

			return string("");
		}

	string
		Norm::getLemmaPosType(unsigned int id)
	{
			for (vector<lemma>::iterator itr = lemmasArr[id % MAX_LINK_ARRAY_SIZE].begin();
				itr != lemmasArr[id % MAX_LINK_ARRAY_SIZE].end(); ++itr) {
				if (itr->id == id) {
					return itr->posType;
				}
			}

			return string("");
		}

	bool
		Norm::saveHashTable(string filename)
	{
			ofstream file(filename.c_str());
			if (!file) return false;
			for (unsigned int i = 0; i < HASH_RANGE; ++i) {
				file << i << " : ";
				for (vector<element>::iterator itr = table[i].begin();
					itr != table[i].end();
					++itr) {

					file << itr->word << ' ';
					for (vector<unsigned int>::iterator lemma = itr->lemmas.begin();
						lemma != itr->lemmas.end();
						++lemma) {
						file << *lemma << ' ';
					}
					file << "& ";
				}
				file << "|" << endl;
			}
			file.close();
			return true;
		}

	unsigned int
		Norm::char2number(const char& c)
	{
			if (c >= 'а' && c <= 'я')
				return (c - 'а' + 1);
			else if (c == 'ё')
				return 33;
			else if (c == 'Ё')
				return 34;
			else if (c >= 'А' && c <= 'Я')
				return (c - 'А' + 1);
			else if (c == '-')
				return 33;
			else
				return 35;
		}

	unsigned long
		Norm::getHash(const char *s)
	{
			unsigned long long res = 0;
			unsigned int cn;
			while (*s) {
				if ((cn = char2number(*s)) == 35) return HASH_RANGE;
				res += cn;
				res *= HASH_BASE;
				++s;
			}
			return res % HASH_RANGE;
		}



	vector<unsigned int>
		Norm::getLemmas(string word)
	{
			set<unsigned int> result;
			unsigned long hash = getHash(tolowercaseDYo(word).c_str());
			vector<element>::iterator it;
			for (it = table[hash].begin(); it != table[hash].end(); ++it) {
				if ((tolowercase(it->word) == tolowercase(word) && useYo) || (tolowercaseDYo(it->word) == tolowercaseDYo(word) && !useYo)) {
					result.insert(it->lemmas.begin(), it->lemmas.end());
				}
			}
			vector<unsigned int> rtrn(result.begin(), result.end());
			return rtrn;
		}

	void
		Norm::setYoRule(bool rule)
	{
			useYo = rule;
		}

	/**
	  *
	  * Реализация конфигурации
	  */

	Configuration::Configuration() {}
	Configuration::~Configuration() {}

	signed char
		Configuration::compare(const connectType &a, const connectType &b)
	{
			if (a.type == 11 && a.dir == 'b' && (b.type == 3 || b.type == 4 || b.type == 5)) {
				return -1;
			}
			if (b.type == 11 && b.dir == 'b' && (a.type == 3 || a.type == 4 || a.type == 5)) {
				return 1;
			}

			if (a.type == 11 && a.dir == 'f' && (b.type == 3 || b.type == 4 || b.type == 5)) {
				return 1;
			}
			if (b.type == 11 && b.dir == 'f' && (a.type == 3 || a.type == 4 || a.type == 5)) {
				return -1;
			}

			if ((a.type == 21 || a.type == 22) && (b.type == 2 || b.type == 12)) {
				return -1;
			}
			if ((b.type == 21 || b.type == 22) && (a.type == 2 || a.type == 12)) {
				return 1;
			}

			if ((a.type == 7 || a.type == 8) && b.type == 23) {
				return 2;
			}
			if ((b.type == 7 || b.type == 8) && a.type == 23) {
				return 2;
			}
			return 0;
		}

	bool
		Configuration::setConf(vector<connectType> c)
	{
			bool allRight = false;
			connectType tmp(0, 0);
			while (!allRight) {
				allRight = true;
				for (int i = 0; i < c.size(); ++i) {
					for (int j = i + 1; j < c.size(); ++j) {
						signed char resC = compare(c[i], c[j]);
						if (resC == 2) {
							return false;
						}
						if (resC == 1) {
							allRight = false;
							tmp = c[i];
							c[i] = c[j];
							c[j] = tmp;
						}
					}
				}
			}
			sortedConnections = c;
			return true;
		}

}