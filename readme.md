# NWord2Vec

[![Build status](https://ci.appveyor.com/api/projects/status/f5xq503sdm2704kd?svg=true)](https://ci.appveyor.com/project/richorama/nword2vec)

C# library for working with [Word2Vec](https://code.google.com/p/word2vec/) models.

## Usage

First build your model with the word2vec command line tools.

You can then load the model into the `Model` class:

```cs
// load the model from a file
var model = Model.Load("model.txt");

// Find the simliarity between words
model.Distance("whale", "boat"); // 2.718597934021814

model.Nearest("whale").Take(10); 

/*
// returns
[
	{
		Word : "first",
		Distance : 2.0338702394548
	},
	{
		Word : "most",
		Distance : 2.06254369516037
	},
	...
]
*/


// Add and subtract vectors

var king = model.GetByWord("king");
var man = model.GetByWord("man");
var woman = model.GetByWord("woman");

var vector = king.Subtract(man).Add(woman);

model.NearestSingle(vector); // queen

```

## License

MIT
