# PdfText

Projeto ASP.NET Core MVC para extração de texto de arquivos PDF via OCR, geração de PDF pesquisável e interface web para upload e download.

## Funcionalidades

- Upload de arquivos PDF
- Extração de texto via OCR (Tesseract)
- Geração de PDF pesquisável (searchable PDF) mantendo o layout original
- Suporte a múltiplos idiomas (tessdata)
- Download do PDF pesquisável
- Interface web simples e intuitiva

## Estrutura do Projeto

PdfText/ ├── Controllers/ ├── Helpers/ ├── Models/ ├── OcrOutput/ ├── tessdata/ ├── Views/ ├── x64/ ├── x86/ ├── appsettings.json ├── Program.cs

## Requisitos

## Requisitos

- .NET Core SDK
- Visual Studio 2022 ou superior
- Tesseract (tessdata)
- PdfiumViewer
- iText7
- BouncyCastle.NetCore

## Como rodar o projeto

1. Clone o repositório:

git clone https://github.com/Matheussandrade22/PdfText.git

2. Abra a solução no Visual Studio.
3. Restaure os pacotes NuGet.
4. Execute o projeto (F5).
5. Acesse a interface web, faça upload de um PDF e baixe o PDF pesquisável.

## Como contribuir

1. Faça um fork do projeto.
2. Crie uma branch para sua feature:

git checkout -b minha-feature

3. Faça commit das suas alterações.
4. Envie um pull request.

## Licença

Este projeto está sob a licença MIT.
