# Features 

This tool allows for easy integration of Wwise Soundbanks into Anno 1800. Once you exported your soundbank, you can run this tool to autogenerate 
- Audio Assets 
- and optionally linked AudioText Assets

from the bnk (or more precise: the bnk and it's json documentation) directly, so no manual work will be required. 

The tool uses Mozillas DeepSpeech with pretrained english models to autotranscribe the Audio. The result won't be perfect, but it will save you a lot of time.

# Dependencies 

This tool is powered by: 

- ![eXpl0it3r/bnkextr](https://github.com/eXpl0it3r/bnkextr)
- ![mozilla/DeepSpeech](https://github.com/mozilla/DeepSpeech)
- ![vgmstream](https://github.com/vgmstream/vgmstream/)