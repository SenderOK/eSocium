#pragma once

#include "norm.h"

using namespace System;
using namespace System::Collections::Generic;

namespace Normalizer
{
	public ref class ConnectType
	{
	public:
		unsigned char type;
		unsigned char direction;
		ConnectType(unsigned char type, unsigned char direction);
	};

	public ref class Configuration
	{
	public:
		UnmanagedNorm::Configuration *cfg;
		Configuration();
		bool setConf(List<ConnectType ^> ^connections);		
	};

	public ref class Normalizer
	{
	private:
		UnmanagedNorm::Norm *norm;
	public:
		Normalizer();
		bool openHashTable(String ^filename);
		bool openLinks(String ^filename);
		List<unsigned int> ^getLemmas(String ^word);
		int walkLemma(int lemma, const Configuration ^c);
	};
}