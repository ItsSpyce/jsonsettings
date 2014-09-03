<h1>JsonSettings</h1>
Simple to use and flexible as ever, JsonSettings is a continuation of SimpleSettings.

<h2>Creating a settings file for your project</h2>
Just reference the JsonSettings library and use

<code>Settings = new Settings("C:\{USER}\location\of\file");</code>

<h2>What's cool about JsonSettings?</h2>
- Comments ('//' and '/**/' typed)
- Lightweight and non-File-IO based. This means instead of constantly reading and writing from
the disk, it reads once to an object, then edits the object. No un-needed saves occure to the file.
When you want the settings saved, just run <code>Settings.Save();</code>
