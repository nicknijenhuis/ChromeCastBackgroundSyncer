# ChromeCastBackgroundSyncer
A .NET Core 2.0 console application that syncs the ChromeCast backdrop wallpapers (backgrounds) in [dconnolly's chromecast-backgrounds](https://github.com/dconnolly/chromecast-backgrounds) to a folder of your choice.

* Used as example for .NET core 2.0 configuration
* Used as example for C# 7.1 async main methods

This project can also be used as file downloader for any other services as long as the JSON contract is:

```
[
    {
        "url": "http://base.com/example1.jpg",
        "author": "Jane Doe"
    },
    {
        "url": "http://base.com/example2.jpg",
        "author": "John Doe"
    }
]
```
