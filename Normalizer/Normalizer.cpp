#include <msclr\marshal_cppstd.h>
#include "Normalizer.h"

namespace Normalizer
{
	ConnectType::ConnectType(unsigned char _type, unsigned char _direction) :
		type(_type),
		direction(_direction)
	{ }

	Configuration::Configuration() :
		cfg(new UnmanagedNorm::Configuration())
	{ }

	bool Configuration::setConf(List<ConnectType ^> ^connections)
	{
		std::vector<UnmanagedNorm::connectType> v;
		v.resize(connections->Count, UnmanagedNorm::connectType(0, 0));
		for (int i = 0; i < connections->Count; ++i)
		{
			v[i].dir = connections[i]->direction;
			v[i].dir = connections[i]->type;
		}
		return cfg->setConf(v);
	}

	Normalizer::Normalizer() :
		norm(new UnmanagedNorm::Norm())
	{ }

	bool Normalizer::openHashTable(String ^filename)
	{
		msclr::interop::marshal_context context;
		std::string stdFilename = context.marshal_as<std::string>(filename);
		return norm->openHashTable(stdFilename);
	}

	bool Normalizer::openLinks(String ^filename)
	{
		msclr::interop::marshal_context context;
		std::string stdFilename = context.marshal_as<std::string>(filename);
		return norm->openLinks(stdFilename);
	}

	List<unsigned int> ^Normalizer::getLemmas(String ^word)
	{
		msclr::interop::marshal_context context;
		std::string stdWord = context.marshal_as<std::string>(word);
		std::vector<unsigned int> v = norm->getLemmas(stdWord);
		List<unsigned int> ^result = gcnew List<unsigned int>(v.size());
		for (unsigned i = 0; i < v.size(); ++i)
		{
			result[i] = v[i];
		}
		return result;
	}

	int Normalizer::walkLemma(int lemma, const Configuration ^c)
	{
		return norm->walkLemma(lemma, c->cfg);
	}
}