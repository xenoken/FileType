# FileType 

> Detect the file type of a byte buffer.

The file type is detected by checking the [magic number](http://en.wikipedia.org/wiki/Magic_number_(programming)#Magic_numbers_in_files) of the buffer.

## Notes
This project is a .NET port of [file-type](https://github.com/sindresorhus/file-type).


## Build
The project has been compiled against **.NET Framework 2.0**. Thus, it can easily be recompiled for other platforms (Mono, .NET Core, etc...) without major changes.

## Usage


```csharp
FileTypeInfo info;
info = FileType.Get(buffer);

string extension = info.Extension;
string mime = info.MimeType;
```

---

## API

### FileType.Get(input)

Returns a `FileTypeInfo` with:

- `Extension` - One of the [supported file types](https://github.com/sindresorhus/file-type#supported-file-types).
- `MimeType` - The [MIME type](http://en.wikipedia.org/wiki/Internet_media_type).

Or `null` when FileType can't detect the file type.

#### input

Type: `byte[]`

See [input](https://github.com/sindresorhus/file-type#supported-file-types) for more information.

---

## Supported file types

See [Supported file types](https://github.com/sindresorhus/file-type#supported-file-types) for more information.


## Related

- [file-type](https://github.com/sindresorhus/file-type-cli) - The original javascript library this project is a port of.


## Ported by

- [xenoken](https://github.com/xenoken)


## License

MIT

