WMPlayer.Web
============

Uses MVC4, SignalR, KendoUI and WMPLib to expose an "Office Friendly" shared jukebox-esque music server

Build machine requirements:
- .Net 4.5
- Visual Studio 2012

Server requiremnets (known): 
- .Net 4.0
- IIS 7
- Windows Media Player

IIS Config:
- Configure App Pool identity to have credentials to mp3 files root dir
- Enable Anonymous Authentication and Edit to use Application pool identity
- Edit Web.config and set "MediaPath" path to root dir of mp3 files. 
- Edit Web.config and set "LibraryFilePath" path to deploy folder or other dir with app pool identity r/w access

