namespace FileOrbis.File.Management.Backend.DTO.Responses
{
    public class FavoriteFileResponse
    {
        public int Id { get; set; }
        public FileResponse File { get; set; }

        public FavoriteFileResponse(int id, FileResponse file)
        {
            Id = id;
            File = file;
        }

    }

}
