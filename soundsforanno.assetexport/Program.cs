/*using System;
using System.IO; 
using System.Text.Json;
using soundsforanno.assetexport.JsonWWISEConvert;
using SoundsForAnno.Assetexport;
using SoundsForAnno.Serializable;
using SoundsForAnno.Transcription;

MultiLanguageMapService c = new MultiLanguageMapService();
var english = JsonSerializer.Deserialize<WWISEJsonObject>(File.ReadAllText("sound.json"));
var german = JsonSerializer.Deserialize<WWISEJsonObject>(File.ReadAllText("sound_de.json"));
c.AddSoundbankInfo(english);
c.AddSoundbankInfo(german);
AudioExporter exporter = new AudioExporter();
exporter.AddAssets(c.GetEvents());
exporter.GetResult().Save("audio.xml");

var guids = exporter.GetGuidMapping();

TranscriptorService transcriptor = new TranscriptorService();
transcriptor.AddSoundbanks("sound.bnk");
await transcriptor.ProcessAsync();
var transcripts = transcriptor.GetResult();

AudioTextExporter audioText = new AudioTextExporter(transcripts, guids);
audioText.AddAssets(c.GetEvents());
audioText.GetResult().Save("audiotext.xml");
*/

using System;

Console.WriteLine("Hello world");