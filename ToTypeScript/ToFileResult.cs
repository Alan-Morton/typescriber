namespace ToTypeScript
{
    using Microsoft.AspNetCore.Mvc;

    public static class ToFileResult
    {
        public static FileContentResult ToFile<T>(string fileName)
        {
            TypeScriber typeScriber = new TypeScriber();
            string convertedTs = typeScriber.AddType(typeof(T)).ToString();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                StreamWriter streamWriter = new StreamWriter(memoryStream);
                streamWriter.Write(convertedTs);
                streamWriter.Flush();
                byte[] content = memoryStream.ToArray();
                return File(content, "application/x-typescript", fileName + ".ts");
            }
        }

        private static FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName)
        {
            return new FileContentResult(fileContents, contentType) { FileDownloadName = fileDownloadName };
        }
    }
}
