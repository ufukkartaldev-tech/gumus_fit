# GümüşFit

Modern .NET 9 backend uygulaması - Clean Architecture ve SOLID prensipleriyle inşa edilmiş.

## Teknoloji Yığını

- .NET 9
- C# 13 (File-scoped namespaces, Primary Constructors)
- Clean Architecture
- SOLID Prensipleri

## Proje Yapısı

```
src/
├── GumusFit.Domain/      # Domain katmanı (Entities, Enums, Interfaces)
├── GumusFit.Data/        # Data katmanı (Repositories, DbContext)
└── GumusFit.API/         # API katmanı (Controllers, DTOs)
```

## Kurulum

```bash
# Repository'yi klonla
git clone <repository-url>

# Solution dosyasını aç
dotnet build
```

## Development

Proje Clean Architecture prensiplerine göre organize edilmiştir. Her katman kendi sorumluluğunda, bağımsız ve test edilebilir şekilde geliştirilmektedir.
