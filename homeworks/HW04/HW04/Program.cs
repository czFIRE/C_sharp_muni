using HW04;
using System.Text;

var watch = new System.Diagnostics.Stopwatch();
watch.Start();

var stegoObject = StegoObject.LoadObject(Samples.StringSample(), (s) => Encoding.Default.GetBytes(s));

// Images to encode the stegoObject into
// Each image must be used to encode a part of the stegoObject
var imageNames = new[]
{
    "John_Martin_-_Belshazzar's_Feast.jpg",
    "John_Martin_-_Pandemonium.jpg",
    "John_Martin_-_Sodom_and_Gomorrah.jpg",
    "John_Martin_-_The_Great_Day_of_His_Wrath.jpg",
    "John_Martin_-_The_Last_Judgement.jpg",
    "John_Martin_-_The_Plains_of_Heaven.jpg"
};

var chunks = stegoObject.GetDataChunks(imageNames.Count()).ToArray();

// parameter for parralelization
int maxTasks = imageNames.Length;

// Do the magic...

Console.WriteLine($"MainThread: {Thread.CurrentThread.ManagedThreadId}");

// Encode it

await ApplicationLogic.encodeEverything(imageNames, chunks, maxTasks);

Console.WriteLine("\nEncoding succesfull!\n");

// Decode it

List<int> precomputedStats = chunks.Select(s => s.Length).ToList();

byte[] decodedData = await ApplicationLogic.decodeEverything(imageNames, precomputedStats, maxTasks);

Console.WriteLine("\nDecoding succesfull!\n");

watch.Stop();
Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms\n");

string resString = Encoding.Default.GetString(decodedData);
Console.WriteLine(resString);

Console.WriteLine($"Are they equal tho?: {resString == Samples.StringSample()}");