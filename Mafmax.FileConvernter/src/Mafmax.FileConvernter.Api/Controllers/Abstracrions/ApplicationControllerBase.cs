using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using static Microsoft.Net.Http.Headers.ContentDispositionHeaderValue;

namespace Mafmax.FileConvernter.Api.Controllers.Abstracrions
{
    /// <summary>
    /// Base class for controllers in current project APIs.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApplicationControllerBase : ControllerBase
    {
        /// <summary>
        /// Loads file and binds result to model.
        /// </summary>
        /// <param name="model">Model to bind loaded file.</param>
        /// <param name="fileContentBinding">
        /// Action to bind file content to model.
        /// Second parameter of delegate represents file data provider part by part.
        /// </param>
        /// <param name="cancellationToken"></param>
        protected async Task LoadFileAsync<TModel>(TModel model,
            Action<TModel, IAsyncEnumerable<ReadOnlyMemory<byte>>> fileContentBinding,
            CancellationToken cancellationToken = default)
            where TModel : class, new()
        {
            var reader = OpenUploadMultipartFile();
            var (formValueProvider, section) = await GetFormValuesAsync(reader, cancellationToken);

            fileContentBinding(model, GetUploadingFileContentProvider(reader, section, cancellationToken));

            await TryUpdateModelAsync(model, prefix: "", formValueProvider);
        }

        private static async Task<(FormValueProvider FormValuesProvider, MultipartSection? LastReadSection)> GetFormValuesAsync(MultipartReader reader, CancellationToken cancellationToken = default)
        {
            var formAccumulator = new KeyValueAccumulator();

            var section = await reader.ReadNextSectionAsync(cancellationToken);

            while (section != null)
            {
                TryParse(section.ContentDisposition, out var contentDisposition);

                if (IsCorrectContentDispositionHeader(contentDisposition))
                {
                    if (IsFileContent(contentDisposition))
                    {
                        formAccumulator.Append("Name", contentDisposition.FileName.Value!);

                        break;
                    }

                    var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name).Value;
                    using var streamReader = new StreamReader(section.Body);

                    var value = await streamReader.ReadToEndAsync(cancellationToken);
                    formAccumulator.Append(key!, value);
                }

                section = await reader.ReadNextSectionAsync(cancellationToken);
            }

            var valueProvider = new FormValueProvider(
                BindingSource.Form,
                new FormCollection(formAccumulator.GetResults()), CultureInfo.CurrentCulture);

            return (valueProvider, section);
        }

        private static async IAsyncEnumerable<ReadOnlyMemory<byte>> GetUploadingFileContentProvider(
            MultipartReader reader, MultipartSection? startSection,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            for (var s = startSection; s != null; s = await NextSection())
            {
                if (!IsCanRead(s)) continue;

                using var memoryStream = new MemoryStream();

                await s.Body.CopyToAsync(memoryStream, cancellationToken);
                
                yield return memoryStream.ToArray();
            }

            async Task<MultipartSection?> NextSection()
            {
                cancellationToken.ThrowIfCancellationRequested();

                return await reader.ReadNextSectionAsync(cancellationToken);
            }
        }

        private MultipartReader OpenUploadMultipartFile()
        {
            var boundary = HeaderUtilities.RemoveQuotes(
                MediaTypeHeaderValue.Parse(Request.ContentType).Boundary
            ).Value;

            return new(boundary!, Request.Body);
        }
        private static bool CheckFileName(ContentDispositionHeaderValue header)
        {
            return !string.IsNullOrEmpty(header.FileName.Value) ||
                   !string.IsNullOrEmpty(header.FileNameStar.Value);
        }

        private static bool IsFileContent(
            [NotNullWhen(returnValue: true)] ContentDispositionHeaderValue? contentDisposition) =>
            IsCorrectContentDispositionHeader(contentDisposition)
            && CheckFileName(contentDisposition);

        private static bool IsCorrectContentDispositionHeader(
            [NotNullWhen(returnValue: true)] ContentDispositionHeaderValue? contentDisposition) =>
            contentDisposition != null
            && contentDisposition.DispositionType.Equals("form-data");

        private static bool IsCanRead(MultipartSection section)
        {
            TryParse(section.ContentDisposition, out var contentDisposition);

            return IsFileContent(contentDisposition) && section.Body.CanRead;
        }
    }
}
